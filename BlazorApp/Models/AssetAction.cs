using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Postgrest.Attributes;

namespace BlazorApp.Models;

[Table("asset_actions")]
public sealed class AssetAction : Postgrest.Models.BaseModel, INotifyPropertyChanged
{
  private string _siteId = string.Empty;
  private string _title = string.Empty;
  private string? _description;
  private DateTime _enteredAt;
  private DateTime? _displayAt;
  private DateTime? _dueAt;
  private DateTime? _startedAt;
  private DateTime? _completedAt;
  private DateTime? _trashedAt;

  [PrimaryKey("id")] public string Id { get; set; } = string.Empty;

  [Column("user_id")] public string UserId { get; set; } = string.Empty;

  [Column("site_id")]
  public string SiteId 
  {
    get => _siteId;
    set => SetField(ref _siteId, value);
  }

  [Column("title")]
  public string Title
  {
    get => _title;
    set => SetField(ref _title, value);
  }

  [Column("description")]
  public string? Description
  {
    get => _description;
    set => SetField(ref _description, value);
  }

  [Column("entered_at", nullValueHandling: NullValueHandling.Include)]
  public DateTime EnteredAt
  {
    get => _enteredAt;
    set => SetField(ref _enteredAt, value);
  }

  [Column("display_at", nullValueHandling: NullValueHandling.Include)] 
  public DateTime? DisplayAt
  {
    get => _displayAt;
    set => SetField(ref _displayAt, value);
  }

  [Column("due_at", nullValueHandling: NullValueHandling.Include)] 
  public DateTime? DueAt
  {
    get => _dueAt;
    set => SetField(ref _dueAt, value);
  }

  [Column("started_at", nullValueHandling: NullValueHandling.Include)] 
  public DateTime? StartedAt
  {
    get => _startedAt;
    set => SetField(ref _startedAt, value);
  }

  [Column("completed_at", nullValueHandling: NullValueHandling.Include)] 
  public DateTime? CompletedAt
  {
    get => _completedAt;
    set => SetField(ref _completedAt, value);
  }

  [Column("trashed_at", nullValueHandling: NullValueHandling.Include)] 
  public DateTime? TrashedAt
  {
    get => _trashedAt;
    set => SetField(ref _trashedAt, value);
  }

  [Column("created_at")] public DateTime CreatedAt { get; set; }
  [Column("updated_at")] public DateTime UpdatedAt { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

  public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
  {
    if (EqualityComparer<T>.Default.Equals(field, value)) 
      return false;

    OnPropertyChanged(propertyName);

    return true;
  }
}
