# CnblogMigrateToTypecho

从博客园到 typecho 的博客迁移工具

## 使用方式

在博客园[备份/导出](https://i.cnblogs.com/posts/export)页面进行导出，并以 sqlite 格式下载，
重命名位`cnblogs.db`，然后将可执行文件`CnblogMigrateToTypecho`、配置文件`config.json`一同上传至服务器。

上传之前请确保配置文件`config.json`配置正确， 文件字段说明如下：

```json
{
  "SqliteFilePath" : "cnblogs.db",
  "MysqlHost" : "127.0.0.1",
  "MysqlPort" : 3306,
  "MysqlUser" : "root",
  "MysqlPassword" : "root",
  "MysqlDatabase" : "typecho"
}
```

* SqliteFilePath：备份文件路径，默认当前目录下的 cnblogs.db
* MysqlHost：typecho 数据库地址
* MysqlPort：typecho 数据库端口号
* MysqlUser：typecho 数据库用户
* MysqlPassword：typecho 数据库密码
* MysqlDatabase：typecho 数据库名称

给予二进制文件执行权限，并执行：

```shell
chmod +x CnblogMigrateToTypecho && ./CnblogMigrateToTypecho
```