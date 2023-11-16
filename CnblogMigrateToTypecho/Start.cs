using System.CommandLine;
using System.Diagnostics;
using System.Text.Json;
using CnblogMigrateToTypecho.Entity;
using CnblogMigrateToTypecho.Entity.Cnblog;
using CnblogMigrateToTypecho.Entity.Typecho;
using FreeSql;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("out.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var code = 0;

var chinaTimeZoneInfo =
    new Lazy<TimeZoneInfo>(() => TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

var rootCommand = new RootCommand("一个用于将 Cnblogs 博客迁移到 Typecho 的工具");

var configOption = new Option<string>("--config", "json 配置文件");
configOption.AddAlias("-f");
configOption.IsRequired = true;
configOption.SetDefaultValue("config.json");
configOption.AddValidator(result =>
{
    var configPath = result.GetValueForOption(configOption);
    if (configPath is not null)
    {
        if (!File.Exists(configPath))
        {
            result.ErrorMessage = $"文件「{configPath}」不存在";
            return;
        }

        try
        {
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));

            if (config is null)
            {
                result.ErrorMessage = $"文件「{configPath}」解析失败";
                return;
            }

            var validator = new ConfigValidator();
            var validationResult = validator.Validate(config);

            switch (validationResult)
            {
                case { IsValid: false }:
                {
                    var msg = string.Join(Environment.NewLine, validationResult.Errors);
                    result.ErrorMessage = $"配置校验失败: {Environment.NewLine}{msg}";
                    return;
                }
            }
        }
        catch (Exception e)
        {
            result.ErrorMessage = $"文件「{configPath}」读取失败：「{e.Message}」";
            return;
        }
    }
});

rootCommand.AddOption(configOption);
rootCommand.SetHandler(configPath =>
{
    try
    {
        var jsonConfig = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));
        Debug.Assert(jsonConfig != null, nameof(jsonConfig) + " != null");

        Log.Information("配置参数：{@jsonConfig}", jsonConfig);

        Handle(jsonConfig);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "程序执行出错");
        code = 1;
    }
}, configOption);

await rootCommand.InvokeAsync(args);

return code;

#region method definition

void Handle(Config config)
{
    var (sqliteFilePath,
        mysqlHost,
        mysqlPort,
        mysqlUser,
        mysqlPassword,
        mysqlDatabase) = config;

    const string createTimeFormat = "yyyy-MM-dd HH:mm:ss.FFF";
    const string updateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    var sqlLiteFs = new FreeSqlBuilder()
        .UseConnectionString(DataType.Sqlite,
            $"Data Source={sqliteFilePath}; Pooling=true; Min Pool Size=1")
        .UseMonitorCommand(cmd => Log.Debug($"{nameof(DataType.Sqlite)} Sql：{cmd.CommandText}")) //监听SQL语句
        .Build();
    var mysqlFs = new FreeSqlBuilder()
        .UseConnectionString(DataType.MySql,
            $"Data Source={mysqlHost}; Port={mysqlPort};User ID={mysqlUser};Password={mysqlPassword}; Initial Catalog={mysqlDatabase}; Charset=utf8mb4; SslMode=Required;Min pool size=1")
        .UseMonitorCommand(cmd => Log.Debug($"{nameof(DataType.MySql)} Sql：{cmd.CommandText}")) //监听SQL语句
        .Build();

    var existCount = mysqlFs.Select<TypechoContents>()
        .Count();
    Log.Information($"现存博客文章数量：{existCount}");

    var adminUser = mysqlFs.Select<TypechoUsers>()
        .Where(x => x.Group == "administrator")
        .OrderByDescending(x => x.Uid)
        .First();

    if (adminUser is null)
    {
        Log.Error("找不到管理员用户");
        return;
    }

    var cnblogs = sqlLiteFs.Select<BlogContent>()
        .ToList();

    if (cnblogs is null || cnblogs.Count == 0)
    {
        Log.Error("sqlite 中没有文章");
        return;
    }

    var toBeInsert = new List<TypechoContents>();

    foreach (var cnblog in cnblogs)
    {
        var sameTitleCount = mysqlFs.Select<TypechoContents>()
            .Where(x => x.Title == cnblog.Title)
            .Count();

        if (sameTitleCount > 0)
        {
            Log.Information($"标题「{cnblog.Title}」已存在typecho库中，跳过");
            continue;
        }

        var createdTime = DateTime.ParseExact(cnblog.CreatedTime, createTimeFormat, null);
        var dateUpdated = DateTime.ParseExact(cnblog.DateUpdated, updateTimeFormat, null);

        var post = new TypechoContents
        {
            Title = cnblog.Title,
            Created = Convert.ToInt32(GetOffset(createdTime).ToUnixTimeSeconds()),
            Modified = Convert.ToInt32(GetOffset(dateUpdated).ToUnixTimeSeconds()),
            Text = cnblog.IsMarkdown == 1 ? "<!--markdown-->" + cnblog.Body : cnblog.Body,
            AuthorId = Convert.ToInt32(adminUser.Uid),
            Type = "post",
            Status = "publish",
            CommentsNum = 0,
            AllowComment = '1',
            AllowPing = '1',
            AllowFeed = '1',
            Slug = Guid.NewGuid().ToString(),
        };

        toBeInsert.Add(post);
    }

    if (toBeInsert.Count > 0)
    {
        var rowCount = mysqlFs.Insert<TypechoContents>()
            .AppendData(toBeInsert)
            .ExecuteAffrows();

        mysqlFs.Ado.ExecuteNonQuery(
            """
            insert into typecho_relationships (cid, mid)
            SELECT cid, 1 from typecho_contents
            where cid not in (select cid from typecho_relationships)
            """);

        Log.Information($"mysql 中插入了{rowCount}条");
    }
}

DateTimeOffset GetOffset(DateTime dateTime)
{
    return TimeZoneInfo.ConvertTimeToUtc(dateTime, chinaTimeZoneInfo.Value);
}

#endregion