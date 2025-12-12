using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Server
{
    public static class ConnnectServer
    {
        private static bool _isMonitoring = false;
        private static CancellationTokenSource _cts;
        private static int _monitoringInterval = 5000;
        public static event EventHandler<ServerStatusEventArgs> StatusChanged;
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


        // 开始心跳监测
        public static void StartHeartbeatMonitoring(string ipAddress, int interval = 5000)
        {
            if (_isMonitoring) return;

            _isMonitoring = true;
            _monitoringInterval = interval;
            _cts = new CancellationTokenSource();

            Task.Run(() => MonitorServerStatus(ipAddress, _cts.Token));
        }

        // 停止心跳监测
        public static void StopHeartbeatMonitoring()
        {
            _cts?.Cancel();
            _isMonitoring = false;
        }
        private static async Task MonitorServerStatus(string ipAddress, CancellationToken ct)
        {
            bool lastStatus = false;

            while (!ct.IsCancellationRequested)
            {
                bool currentStatus = PingIPAddress(ipAddress);

                if (currentStatus != lastStatus)
                {
                    lastStatus = currentStatus;
                    StatusChanged?.Invoke(null, new ServerStatusEventArgs
                    {
                        IPAddress = ipAddress,
                        IsOnline = currentStatus,
                        Timestamp = DateTime.Now
                    });
                }

                await Task.Delay(_monitoringInterval, ct);
            }
        }
    }
    public class ServerStatusEventArgs : EventArgs
    {
        public string IPAddress { get; set; }
        public bool IsOnline { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
