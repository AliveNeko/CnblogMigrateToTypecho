using FreeSql.DataAnnotations;

namespace CnblogMigrateToTypecho.Entity.Typecho;

[Table(Name = "typecho_users", DisableSyncStructure = true)]
public partial class TypechoUsers
{
    [Column(Name = "uid", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Uid { get; set; }

    [Column(Name = "activated", DbType = "int unsigned")]
    public uint? Activated { get; set; } = 0;

    [Column(Name = "authCode", StringLength = 64)]
    public string AuthCode { get; set; }

    [Column(Name = "created", DbType = "int unsigned")]
    public uint? Created { get; set; } = 0;

    [Column(Name = "group", StringLength = 16)]
    public string Group { get; set; } = "visitor";

    [Column(Name = "logged", DbType = "int unsigned")]
    public uint? Logged { get; set; } = 0;

    [Column(Name = "mail", StringLength = 150)]
    public string Mail { get; set; }

    [Column(Name = "name", StringLength = 32)]
    public string Name { get; set; }

    [Column(Name = "password", StringLength = 64)]
    public string Password { get; set; }

    [Column(Name = "screenName", StringLength = 32)]
    public string ScreenName { get; set; }

    [Column(Name = "url", StringLength = 150)]
    public string Url { get; set; }
}