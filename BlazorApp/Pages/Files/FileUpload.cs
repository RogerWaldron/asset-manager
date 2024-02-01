namespace BlazorApp.Pages.Files;

public partial class FileUpload
{
    protected override async Task OnInitializedAsync()
    {
        await GetFilesFromBucket();
    }
}