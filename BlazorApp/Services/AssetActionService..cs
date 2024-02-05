using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BlazorApp.Extensions;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using Postgrest.Exceptions;
using Postgrest.Interfaces;
using Supabase;
using Supabase.Realtime;
using Supabase.Realtime.Exceptions;
using Supabase.Realtime.Interfaces;
using Supabase.Realtime.PostgresChanges;

namespace BlazorApp.Services;

public class AssetActionService : IAssetActionService
{
  private bool _isLoading;

    private IAppStateService _appStateService { get; }
    private Supabase.Client _client { get; }
    private RealtimeChannel? Listener { get; set; }
    public IPostgrestCacheProvider _postgrestCacheProvider { get; }
    public IPostgrestTableWithCache<AssetAction> Ref => _client.Postgrest.Table<AssetAction>(_postgrestCacheProvider);

    private ILogger<AssetActionService> _logger;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsLoading 
    {
      get => _isLoading;
      private set => SetField(ref _isLoading, value);
    }
    public ObservableCollection<AssetAction> AssetActions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public AssetActionService(
      IAppStateService appStateService,
      Supabase.Client client, 
      IPostgrestCacheProvider postgrestCacheProvider,
      ILogger<AssetActionService> logger) 
    {
        _appStateService = appStateService;
        _client = client;
        _postgrestCacheProvider = postgrestCacheProvider;
        _logger = logger;
    }

    public async Task<bool> Create(AssetAction assetAction) 
    {
      try
      {
        if (_appStateService.User?.Id is null)
          return false;
          
        assetAction.UserId = _appStateService.User.Id;
        assetAction.CreatedAt = DateTime.UtcNow;
        assetAction.UpdatedAt = DateTime.UtcNow;

        AssetActions.Add(assetAction);

        try 
        {
          var inserted = await Ref.Insert(assetAction);
          assetAction.Id = inserted.Model!.Id;
          
          return true;
        }
        catch (PostgrestException ex)
        {
          _logger.LogDebug(ex, ex.GetType().FullName);
          _logger.LogError(ex.Message);

          return false;
        }
      }
      catch (Exception ex)
      {
        _logger.LogDebug(ex, ex.GetType().FullName);
        _logger.LogError(ex.Message);

        return false;
      }
    }

    public async Task<bool> Delete(AssetAction assetAction) 
    {
      try 
      {
        await Ref.Delete(assetAction);
        AssetActions.Remove(assetAction);
        
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogDebug(ex, ex.GetType().FullName);
        _logger.LogError(ex.Message);

        return false;
      }
    }
  
      public async Task<bool> Duplicate(AssetAction assetAction)
    {
        try 
        {
          var duplicatedAction = assetAction.DeepCopy();
          assetAction.Id = string.Empty;
          assetAction.UpdatedAt = DateTime.UtcNow;
          await Ref.Insert(duplicatedAction);

          return true;
        }
        catch (Exception ex)
        {
          _logger.LogDebug(ex, ex.GetType().FullName);
          _logger.LogError(ex.Message);

          return false;
        }
    }

    public async Task<bool> MarkCompleted(AssetAction assetAction, bool isCompleted)
    {
      try
      {
        assetAction.CompletedAt = isCompleted ? DateTime.UtcNow : null;
        await Ref.Update(assetAction);

        return true;
      }
      catch (Exception ex)
      {
        _logger.LogDebug(ex, ex.GetType().FullName);
        _logger.LogError(ex.Message);

        return false;
      }
    }

    public async Task<bool> Restore(AssetAction assetAction)
    {
      try 
      {
        assetAction.TrashedAt = null;
        await Ref.Insert(assetAction);

        return true;
      }
      catch (Exception ex)
      {
        _logger.LogDebug(ex, ex.GetType().FullName);
        _logger.LogError(ex.Message);

        return false;
      }
    }

    public async Task<bool> Trash(AssetAction assetAction)
    {
      try 
      {
        assetAction.TrashedAt = DateTime.UtcNow;
        await Ref.Insert(assetAction);

        return true;
      }
      catch (Exception ex)
      {
        _logger.LogDebug(ex, ex.GetType().FullName);
        _logger.LogError(ex.Message);

        return false;
      }
    }

    public async Task<bool> Update(AssetAction assetAction)
    {
        try
        {
          await Ref.Update(assetAction);

          return true;
        }
        catch (Exception ex)
        {
          _logger.LogDebug(ex, ex.GetType().FullName);
          _logger.LogError(ex.Message);

          return false;
        }
    }

  private async void Register() 
  {
    IsLoading = true;

    await _client.InitializeAsync();

    try {
      Listener ??= await _client.From<AssetAction>().On(
        PostgresChangesOptions.ListenType.All,
        OnAssetActionModelChanges
        );
    }
    catch (RealtimeException ex)
    {
      _logger.LogDebug(ex, ex.GetType().FullName);
      _logger.LogError(ex.Message);
    }

    // populate local cache with user's actions
    await Populate();

    IsLoading = false;
  }

    private void OnAssetActionModelChanges(IRealtimeChannel sender, PostgresChangesResponse change)
    {
        var model = change.Model<AssetAction>();

        if (model is null) return;

        switch (change.Payload?.Data?._type) 
        {
          case "INSERT":
            _logger.LogInformation($"AssetAction has been inserted: {model.Id}");
            var toInsert = AssetActions.FirstOrDefault(a => a.Id == model.Id);
            if (toInsert is null)
              AssetActions.Add(model);
            break;
          case "UPDATE":
            _logger.LogInformation($"AssetAction has been updated: {model.Id}");
            var toUpdate = AssetActions.FirstOrDefault(a => a.Id == model.Id);
            if (toUpdate is null) 
              return;
            AssetActions[AssetActions.IndexOf(toUpdate)] = model;
            break;
          case "DELETE":
            _logger.LogInformation($"AssetAction has been deleted: {model.Id}");
            var toDelete = AssetActions.FirstOrDefault(a => a.Id == model.Id);
            if (toDelete is null)
              return;
            AssetActions.Remove(toDelete);
            break;
        }
    }

    private void Unregister()
  {
    Listener?.Unsubscribe();
    Listener = null;

  }

  private async Task Populate() 
  {
    try 
    {
      var response = await Ref.Get();

      foreach (var model in response.Models)
        AssetActions.Add(model);

      response.RemoteModelsPopulated += sender => 
      {
        foreach (var model in sender.Models) 
        {
          var exists = AssetActions.FirstOrDefault(a => a.Id == model.Id);
          if (exists is not null)
            AssetActions[AssetActions.IndexOf(exists)] = model;
        }
      };
    }
    catch (PostgrestException ex) 
    {
      _logger.LogDebug(ex, ex.GetType().FullName);
      _logger.LogError(ex.Message);
    }
  }
  private void OnPropertyChanged(
      [CallerMemberName] string? propertyName = null)
  {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) 
  {
      if (EqualityComparer<T>.Default.Equals(field, value)) 
        return false;

      field = value;
      OnPropertyChanged(propertyName);

      return true;
  }
}