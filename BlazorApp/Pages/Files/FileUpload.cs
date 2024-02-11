using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using Supabase.Storage.Interfaces;

namespace BlazorApp.Pages.Files;

public partial class FileUpload
{
    // [Inject]
    // private IStorageClient<Supabase.Storage.Bucket, Supabase.Storage.FileObject>? _storageClient { get; set; }

    // [Inject] 
    // private SbStorageService? _storageService { get; set; }
    [Inject]
    private  IStorageClient<Supabase.Storage.Bucket, Supabase.Storage.FileObject>? _supabase { get; set; }

    public List<Supabase.Storage.FileObject> FileObjects = new List<Supabase.Storage.FileObject>().ToList();
    static long _MaxFileSizeInMb = 1024 * 1024 * 15;
    
    
    // protected override async void OnInitializedAsync()
    // {
    //     var bucketName = "userfiles";
    //     if (!string.IsNullOrEmpty(bucketName) && !(_supabase is null))
    //     {
    //          FileObjects = await _supabase.From("userfiles").List() ?? new List<Supabase.Storage.FileObject>();
    //     }
    // }

    // private async Task ListBuckets()
    // {
    //     // FileObjects = await Supabase.Storage.Bucket.
    // }

    // private async Task handleUploadFilesAsync(IBrowserFile file)
    // {
    //     try
    //     {
    //         var streamData = file.OpenReadStream(_MaxFileSizeInMb);
    //
    //         var filename = await StorageService.UploadFile("userfiles", streamData, file.Name);
    //         
    //         Toast.Notify(new NotificationMessage
    //         {
    //             Summary = "File uploaded: " + filename.Split("/").Last(),
    //             Duration = 4000,
    //             Severity = NotificationSeverity.Info
    //         });
    //         await GetFilesFromBucket();
    //         await InvokeAsync(StateHasChanged);
    //     }
    //     catch (IOException ex)
    //     {
    //         Toast.Notify(new NotificationMessage
    //         {
    //             Summary = "Error: Maximum supported file upload size was exceeded!",
    //             Duration = 4000,
    //             Severity = NotificationSeverity.Error
    //         });            
    //     }
    // }
    //
    // private async Task handleDownloadClick(Supabase.Storage.FileObject row)
    // {
    // byte[] downloadedBytes = await StorageService.DownloadFile("userfiles", row.Name);
    //
    // await JS.InvokeVoidAsync("downloadFileFromStream", row.Name, downloadedBytes);
    //
    // }
}