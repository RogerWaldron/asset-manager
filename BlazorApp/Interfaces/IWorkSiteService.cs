using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BlazorApp.Services;

public interface IWorkSiteService : INotifyPropertyChanged
{
  bool IsLoading { get; }
  ObservableCollection<WorkSite> WorkSites { get; set; }
  Task<bool> Create(WorkSite workSite);
  Task<bool> Delete(WorkSite workSite);
  Task<bool> Duplicate(WorkSite workSite);
  Task<bool> MarkCompleted(WorkSite workSite, bool isCompleted);
  Task<bool> Restore(WorkSite workSite);
  Task<bool> Trash(WorkSite workSite);
  Task<bool> Update(WorkSite workSite);
}