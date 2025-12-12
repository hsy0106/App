using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace App.Server
{
   public static class SQLServer
    {
        public static class SqlSugarSetup
        {
            // 从配置读取数据库类型和连接字符串
            static readonly string dbTypeStr = InIServer.Read("Server", "DbType");     // 如："Access", "MySql", "SqlServer", "Sqlite"
            static readonly string dbConnectStr = InIServer.Read("Server", "DbSource"); // 对应连接字符串或数据库文件路径

            public static SqlSugarClient AddSqlsugarSetup()
            {
                DbType dbType = ParseDbType(dbTypeStr);

                var config = new ConnectionConfig
                {
                    ConnectionString = BuildConnectionString(dbType, dbConnectStr),
                    DbType = dbType,
                    IsAutoCloseConnection = true,
                };

                return new SqlSugarClient(config, db =>
                {
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
                    };
                    db.Ado.IsDisableMasterSlaveSeparation = true;
                });
            }

            // 将字符串解析为 SqlSugar 的 DbType 枚举
            public static DbType ParseDbType(string type)
            {

                DbType DbTypeTemp = DbType.Access;

                switch (type.Trim().ToLower())
                {
                    case "accesss":
                        DbTypeTemp = DbType.Access;
                        break;
                    case "mysql":
                        DbTypeTemp = DbType.MySql;
                        break;
                    case "sqlserver":
                        DbTypeTemp = DbType.SqlServer;
                        break;
                    case "sqlite":
                        DbTypeTemp = DbType.Sqlite;
                        break;

                }

                return DbTypeTemp;
                //return type.Trim().ToLower() switch
                //{
                //    "access" => DbType.Access,
                //    "mysql" => DbType.MySql,
                //    "sqlserver" => DbType.SqlServer,
                //    "sqlite" => DbType.Sqlite,
                //    _ => throw new NotSupportedException($"不支持的数据库类型: {type}")
                //};
            }

            // 构建对应数据库的连接字符串
            private static string BuildConnectionString(DbType dbType, string baseStr)
            {
                if ((dbType == DbType.Access || dbType == DbType.Sqlite))
                {
                    if (!Path.IsPathRooted(baseStr))
                    {
                        // 将相对路径转换成绝对路径
                        baseStr = Path.GetFullPath(baseStr);
                    }
                }
                string baseTemp = null;

                switch (dbType)
                {
                    case DbType.Access:
                        baseTemp = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={baseStr};Persist Security Info=False;";
                        break;
                    case DbType.MySql:
                        baseTemp = $"Server={InIServer.Read("Server", "Host")};" +
                         $"Database={InIServer.Read("Server", "DbName")};" +
                         $"Uid={InIServer.Read("Server", "UserName")};" +
                         $"Pwd={InIServer.Read("Server", "Password")};" +
                         $"Pooling=true;SslMode=None;";
                        break;
                    case DbType.SqlServer:
                        baseTemp = $"Server={InIServer.Read("Server", "Host").ToString()};Database={InIServer.Read("Server", "DBName").ToString()};User Id={InIServer.Read("Server", "UserName").ToString()};Password={InIServer.Read("Server", "PassWord").ToString()};";
                        break;
                    case DbType.Sqlite:
                        baseTemp = $"Data Source={baseStr};";
                        break;

                }

                return baseTemp;
                //return 


                //    dbType switch
                //    {
                //        DbType.Access => $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={baseStr};Persist Security Info=False;",
                //        DbType.MySql => baseStr,       // 完整连接字符串，如：Server=localhost;Database=test;Uid=root;Pwd=1234;
                //        DbType.SqlServer => baseStr,   // 如：Server=.;Database=MyDb;User Id=sa;Password=1234;
                //        DbType.Sqlite => $"Data Source={baseStr};",
                //        _ => throw new NotSupportedException("无法构建连接字符串")
                //    };
            }
        }

    }
}
