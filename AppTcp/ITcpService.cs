using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTcp
{
    public interface ITcpService
    {
        bool IsConnected { get; }

        event EventHandler<bool> ConnectionStatusChanged;

        event EventHandler<string> MessageReceived;

        Task<bool> ConnectAsync(TcpOptions operation);

        Task DisconnectAsync();

        Task<bool> PublicAsync(string ip, int port, byte[] data);

        Task<bool> PublicAsync(string ip, int port, string data);
    }
}
