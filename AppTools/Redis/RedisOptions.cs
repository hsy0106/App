using System;

namespace AppTools.Serialize.Server
{
    /// <summary>
    /// Redis配置选项
    /// </summary>
    public class RedisOptions
    {
        /// <summary>
        /// Redis连接字符串 (例如: "localhost:6379,password=123456,defaultDatabase=0")
        /// </summary>
        public string ConnectionString { get; set; } = "localhost:6379";

        /// <summary>
        /// 实例名称前缀
        /// </summary>
        public string InstanceName { get; set; } = "Admin:";

        /// <summary>
        /// 默认过期时间（分钟）
        /// </summary>
        public int DefaultExpiryMinutes { get; set; } = 60;

        /// <summary>
        /// 连接超时时间（秒）
        /// </summary>
        public int ConnectTimeout { get; set; } = 5;

        /// <summary>
        /// 同步超时时间（秒）
        /// </summary>
        public int SyncTimeout { get; set; } = 5;

        /// <summary>
        /// 是否启用管理员权限
        /// </summary>
        public bool AllowAdmin { get; set; } = false;
    }
}