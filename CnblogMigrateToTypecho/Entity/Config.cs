namespace CnblogMigrateToTypecho.Entity;

public class Config
{
    public string SqliteFilePath { get; set; } = default!;
    public string MysqlHost { get; set; } = default!;
    public int MysqlPort { get; set; }
    public string MysqlUser { get; set; } = default!;
    public string MysqlPassword { get; set; } = default!;
    public string MysqlDatabase { get; set; } = default!;

    public void Deconstruct(out string sqliteFilePath, out string mysqlHost, out int mysqlPort, out string mysqlUser, out string mysqlPassword, out string mysqlDatabase)
    {
        sqliteFilePath = SqliteFilePath;
        mysqlHost = MysqlHost;
        mysqlPort = MysqlPort;
        mysqlUser = MysqlUser;
        mysqlPassword = MysqlPassword;
        mysqlDatabase = MysqlDatabase;
    }

}