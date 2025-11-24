using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace App.Server
{
    public static class InIServer
    {
        private static readonly string filePath =
         Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/config.ini");



        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(
            string section, string key, string defaultValue,
            StringBuilder result, int size, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool WritePrivateProfileString(
            string section, string key, string value, string filePath);

        /// <summary>
        /// 读取INI值（带默认值和错误处理）
        /// </summary>
        public static string Read(string section, string key, string defaultValue = "")
        {
            try
            {
                var sb = new StringBuilder(255);
                GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, filePath);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                // 记录日志或抛出自定义异常
                LogServer.Warn("INI文件读取异常:" + ex.ToString());
                return defaultValue;
            }
        }

        /// <summary>
        /// 读取整数型配置
        /// </summary>
        public static int ReadInt(string section, string key, int defaultValue = 0)
        {
            string val = Read(section, key);
            return int.TryParse(val, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// 写入INI值
        /// </summary>
        public static bool Write(string section, string key, string value)
        {
            try
            {
                return WritePrivateProfileString(section, key, value, filePath);
            }
            catch(Exception ex)
            {
                LogServer.Warn("写入INI文件异常:" +ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 删除某个Key
        /// </summary>
        public static bool DeleteKey(string section, string key)
        {
            return WritePrivateProfileString(section, key, null, filePath);
        }

        /// <summary>
        /// 删除整个Section
        /// </summary>
        public static bool DeleteSection(string section)
        {
            return WritePrivateProfileString(section, null, null, filePath);
        }

        /// <summary>
        /// 检查Section是否存在
        /// </summary>
        public static bool SectionExists(string section)
        {
            // 尝试读取该Section下的一个不存在的Key
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, null, "", sb, sb.Capacity, filePath);
            return sb.Length > 0;
        }
    }
}