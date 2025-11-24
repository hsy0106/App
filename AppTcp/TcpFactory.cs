using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTcp
{
    public static class TcpFactory
    {
        public static ITcpService Create()
        {
            return new TcpServer();
        }
        public static async Task<ITcpService> CreateAndConnectAsync(TcpOptions options)
        {
            var service = new TcpServer();
            if (!await service.ConnectAsync(options))
                throw new System.Exception("TCP连接失败");
            return service;
        }
    }
}
