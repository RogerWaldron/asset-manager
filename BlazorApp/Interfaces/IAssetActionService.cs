using System.Collections.ObjectModel;
using System.ComponentModel;
using BlazorApp.Models;

namespace BlazorApp.Interfaces;

public interface IAssetActionService : INotifyPropertyChanged
{
  bool IsLoading { get; }
  ObservableCollection<AssetAction> AssetActions { get; set; }
  Task<bool> Create(AssetAction assetAction);
  Task<bool> Delete(AssetAction assetAction);
  Task<bool> Duplicate(AssetAction assetAction);
  Task<bool> MarkCompleted(AssetAction assetAction, bool isCompleted);
  Task<bool> Restore(AssetAction assetAction);
  Task<bool> Trash(AssetAction assetAction);
  Task<bool> Update(AssetAction assetAction);
}