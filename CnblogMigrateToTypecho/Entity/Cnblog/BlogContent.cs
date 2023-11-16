using FreeSql.DataAnnotations;

namespace CnblogMigrateToTypecho.Entity.Cnblog;

[Table(Name = "blog_Content", DisableSyncStructure = true)]
public class BlogContent
{
    [Column(IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }

    [Column(StringLength = -2, IsNullable = false)]
    public string Title { get; set; } = default!;

    [Column(StringLength = -2, IsNullable = false)]
    public string DateAdded { get; set; } = default!;

    [Column(StringLength = -2)]
    public string SourceUrl { get; set; } = default!;

    [Column(StringLength = -2, IsNullable = false)]
    public string PostType { get; set; } = default!;

    [Column(StringLength = -2, IsNullable = false)]
    public string Body { get; set; } = default!;

    public int BlogId { get; set; }

    [Column(StringLength = -2)]
    public string Description { get; set; } = default!;

    [Column(StringLength = -2, IsNullable = false)]
    public string DateUpdated { get; set; } = default!;

    public int IsMarkdown { get; set; }

    [Column(StringLength = -2)]
    public string EntryName { get; set; } = default!;

    [Column(StringLength = -2)]
    public string CreatedTime { get; set; } = default!;

    public int IsActive { get; set; }

    [Column(StringLength = -2)]
    public string AutoDesc { get; set; } = default!;

    [Column(StringLength = -2, IsNullable = false)]
    public string AccessPermission { get; set; } = default!;

    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(DateAdded)}: {DateAdded}, {nameof(SourceUrl)}: {SourceUrl}, {nameof(PostType)}: {PostType}, {nameof(Body)}: {Body}, {nameof(BlogId)}: {BlogId}, {nameof(Description)}: {Description}, {nameof(DateUpdated)}: {DateUpdated}, {nameof(IsMarkdown)}: {IsMarkdown}, {nameof(EntryName)}: {EntryName}, {nameof(CreatedTime)}: {CreatedTime}, {nameof(IsActive)}: {IsActive}, {nameof(AutoDesc)}: {AutoDesc}, {nameof(AccessPermission)}: {AccessPermission}";
    }
}