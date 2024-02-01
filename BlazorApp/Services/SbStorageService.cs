using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Supabase.Storage;
using Supabase.Storage.Interfaces;
using Client = Supabase.Client;

namespace BlazorApp.Services;

public class SbStorageService
{
    private readonly IStorageClient<Bucket, FileObject?> _storage;
    private readonly Client _client;
    private readonly ILogger<SbStorageService> _logger;

    public SbStorageService(
        Supabase.Client client,
        ILogger<SbStorageService> logger
    )
    {
        _client = client;
        _logger = logger;
        _storage = _client.Storage;
    }

    public async Task<string> UploadFile(String bucketName, Stream streamData, String fileName)
    {
        var bucket = _storage.From(bucketName);

        var dataAsBytes = await StreamToBytesAsync(streamData);

        var fileExtension = fileName.Split(".").Last();
        var userId = _client?.Auth?.CurrentUser?.Id;

        var saveName = $"{userId}-" + Guid.NewGuid();
        saveName = saveName
            .Replace("/", "_")
            .Replace(" ", "_")
            .Replace(":", "_");
        saveName = saveName + "." + fileExtension;

        return await bucket.Upload(dataAsBytes, saveName);
    }

    public async Task<List<FileObject?>?> GetFilesFromBucket(string bucketName)
    {
        return await _storage.From(bucketName).List();
    }

    public async Task<byte[]> DownloadFile(string bucketName, string fileName)
    {
        var bucket = _storage.From(bucketName);

        return await bucket.Download(fileName, (_, f) => Debug.WriteLine($"Download Progress: {f}%"));
    }

    public async Task<FileObject?>? DeleteFile(string bucketName, string fileName)
    {
        var bucket = _storage.From(bucketName);
        
        return await bucket.Remove(fileName);
    }

    private async Task<byte[]> StreamToBytesAsync(Stream streamData)
    {
        byte[] bytes;

        using var memoryStream = new MemoryStream();
        await streamData.CopyToAsync(memoryStream);
        bytes = memoryStream.ToArray();

        return bytes;
    }
}