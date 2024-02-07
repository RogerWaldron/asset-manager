using BlazorApp.Services;

namespace BlazorApp.Pages.Files;

public partial class FileUpload
{
    public List<Supabase.Storage.FileObject> FileObjects = new List<Supabase.Storage.FileObject>().ToList();
    static long _MaxFileSizeInMb = 1024 * 1024 * 15;
    
    
    protected override async Task OnInitializedAsync()
    {
        await GetFilesFromBucket();
    }

    private async Task GetFilesFromBucket()
    {
        FileObjects = await SbStorageService.GetFilesFromBucket("userfiles") ??= Enumerable.Empty<Supabase.Storage.FileObject>().ToList();
    }
    
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