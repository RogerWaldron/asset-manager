using Postgrest.Attributes;

namespace BlazorApp.Models;

[Table("work_sites")]
public sealed class WorkSite
{
  [PrimaryKey("id")]
  public string Id { get; set; } = string.Empty;

  [Column("parent_site_id")]
  public string? ParentSiteId { get; set; }

  [Column("user_id")]
  public string UserId { get; set; } = string.Empty;

  [Column("name")]
  public string Name { get; set; } = string.Empty;

  [Column("created_at")] public DateTime CreatedAt { get; set; }

  [Column("updated_at")] public DateTime UpdatedAt { get; set; }

}