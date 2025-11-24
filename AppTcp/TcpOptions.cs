using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTcp
{
    public class TcpOptions
    {

        public string Ip { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; } = 3000; // 连接超时
        public bool AutoReconnect { get; set; } = true; // 自动重连
    }
}
