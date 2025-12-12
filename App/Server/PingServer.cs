using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace App.Server
{
    public static class PingServer
    {

        public static bool PingIPAddress(string IPAddress, int timeout = 2000)
        {
         
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(IPAddress, timeout);
                    return reply?.Status == IPStatus.Success;

                }
            }
            catch (PingException pex)
            {
                throw new Exception($"Ping{IPAddress}操作失败: {pex.Message}", pex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ping{IPAddress}发生未知错误: {ex.Message}", ex);
            }
        }

    }
}
