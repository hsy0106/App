using System;
using System.IO;
using Serilog;
using Serilog.Events;

namespace App
{
    public static class LogServer
    {
        private static readonly ILogger _logger;

        static LogServer()
        {
            try
            {
                // 确保日志目录存在如果不存在就创建一个文件夹
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // 配置 Serilog
                //_logger = new LoggerConfiguration()
                //    .MinimumLevel.Debug()
                //    .WriteTo.File(
                //        path: Path.Combine(logDirectory, $"{DateTime.Now:yyyyMMdd}", "log.txt"),
                //        rollingInterval: RollingInterval.Day, // 每天一个文件夹
                //        restrictedToMinimumLevel: LogEventLevel.Information,
                //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                //    )
                //    .WriteTo.Console() 
                //    .CreateLogger();

                _logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.File(
                         //path: Path.Combine(logDirectory, $"{DateTime.Now:yyyyMMdd}.txt"), // 直接生成日期文件
                         path: Path.Combine(logDirectory, "log.txt"),
                         rollingInterval: RollingInterval.Day, // 每天一个新文件
                         restrictedToMinimumLevel: LogEventLevel.Information,
                         outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                     )
                     .WriteTo.Console()
                     .CreateLogger();
            }
            catch (Exception ex)
            {
               
                throw new Exception("日志系统初始化失败: " + ex.Message, ex);
            }
        }

        // 提供静态方法供外部调用打印日志
        public static void Info(string message) => _logger.Information(message);
        public static void Warn(string message) => _logger.Warning(message);
        public static void Error(string message, Exception ex = null) => _logger.Error(ex, message);
        public static void Debug(string message) => _logger.Debug(message);
    }
}