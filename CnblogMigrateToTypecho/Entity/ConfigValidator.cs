using FluentValidation;

namespace CnblogMigrateToTypecho.Entity;

public class ConfigValidator : AbstractValidator<Config>
{
    public ConfigValidator()
    {
        RuleFor(x => x.SqliteFilePath).NotEmpty()
            .WithMessage("sqlite 文件路径不可为空");
        RuleFor(x => x.MysqlHost).NotEmpty()
            .WithMessage("mysql 数据库地址不可为空");
        RuleFor(x => x.MysqlPort)
            .Must(x => x is >= 1 and <= 65535)
            .WithMessage("mysql 数据库端口必须在 1 到 65535 之间");
        RuleFor(x => x.MysqlUser).NotEmpty()
            .WithMessage("mysql 数据库用户名不可为空");
        RuleFor(x => x.MysqlPassword).NotEmpty()
            .WithMessage("mysql 数据库密码不可为空");
        RuleFor(x => x.MysqlDatabase).NotEmpty()
            .WithMessage("mysql 数据库名不可为空");
    }
}