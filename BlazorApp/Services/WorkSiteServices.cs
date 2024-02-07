
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BlazorApp.Services;

public sealed class WorkSite : IWorkSiteService
{
    public bool IsLoading => throw new NotImplementedException();

    public ObservableCollection<WorkSite> WorkSites { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    public Task<bool> Create(WorkSite workSite)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(WorkSite workSite)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Duplicate(WorkSite workSite)
    {
        throw new NotImplementedException();
    }

    public Task<bool> MarkCompleted(WorkSite workSite, bool isCompleted)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Restore(WorkSite workSite)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Trash(WorkSite workSite)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(WorkSite workSite)
    {
        throw new NotImplementedException();
    }
} 