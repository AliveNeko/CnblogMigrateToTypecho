using FreeSql.DataAnnotations;

namespace CnblogMigrateToTypecho.Entity.Typecho;

/// <summary>
/// 表示数据库中的post表
/// </summary>
[Table(Name = "typecho_contents", DisableSyncStructure = true)]
public class TypechoContents
{
    /// <summary>
    /// post表主键
    /// </summary>
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Cid { get; set; }

    /// <summary>
    /// 内容标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 内容缩略名，可为空
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// 内容生成时的GMT unix时间戳，可为空
    /// </summary>
    public int? Created { get; set; }

    /// <summary>
    /// 内容更改时的GMT unix时间戳，可为空
    /// </summary>
    public int? Modified { get; set; }

    /// <summary>
    /// 内容文字，可为空
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// 排序，可为空
    /// </summary>
    public int? Order { get; set; }

    /// <summary>
    /// 内容所属用户id，可为空
    /// </summary>
    public int? AuthorId { get; set; }

    /// <summary>
    /// 内容使用的模板，可为空
    /// </summary>
    public string? Template { get; set; }

    /// <summary>
    /// 内容类别，可为空
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 内容状态，可为空
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 受保护内容,此字段对应内容保护密码，可为空
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 内容所属评论数,冗余字段，可为空
    /// </summary>
    public int? CommentsNum { get; set; }

    /// <summary>
    /// 是否允许评论，可为空
    /// </summary>
    public char? AllowComment { get; set; }

    /// <summary>
    /// 是否允许ping，可为空
    /// </summary>
    public char? AllowPing { get; set; }

    /// <summary>
    /// 允许出现在聚合中，可为空
    /// </summary>
    public char? AllowFeed { get; set; }

    public override string ToString()
    {
        return $"{nameof(Cid)}: {Cid}, {nameof(Title)}: {Title}, {nameof(Slug)}: {Slug}, {nameof(Created)}: {Created}, {nameof(Modified)}: {Modified}, {nameof(Text)}: {Text}, {nameof(Order)}: {Order}, {nameof(AuthorId)}: {AuthorId}, {nameof(Template)}: {Template}, {nameof(Type)}: {Type}, {nameof(Status)}: {Status}, {nameof(Password)}: {Password}, {nameof(CommentsNum)}: {CommentsNum}, {nameof(AllowComment)}: {AllowComment}, {nameof(AllowPing)}: {AllowPing}, {nameof(AllowFeed)}: {AllowFeed}";
    }
}