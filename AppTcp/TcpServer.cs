using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppTcp
{
    public class TcpServer : ITcpService, IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private CancellationTokenSource _cts;

        public bool IsConnected => _client?.Connected == true;

        public event EventHandler<bool> ConnectionStatusChanged;
        public event EventHandler<string> MessageReceived;

        private bool _isDisposed = false;

        public async Task<bool> ConnectAsync(TcpOptions op)
        {
            try
            {
                _client = new TcpClient();
                var connectTask = _client.ConnectAsync(op.Ip, op.Port);

                if (await Task.WhenAny(connectTask, Task.Delay(op.Timeout)) != connectTask)
                    throw new TimeoutException("TCP 连接超时");

                _stream = _client.GetStream();
                ConnectionStatusChanged?.Invoke(this, true);

                // 开始接收线程
                _cts = new CancellationTokenSource();
                _ = Task.Run(() => ReceiveLoop(_cts.Token));

                return true;
            }
            catch (Exception ex)
            {
                ConnectionStatusChanged?.Invoke(this, false);
                Console.WriteLine("TCP连接失败：" + ex.Message);
                return false;
            }
        }

        private async Task ReceiveLoop(CancellationToken token)
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (!token.IsCancellationRequested)
                {
                    if (!_client.Connected) break;

                    int len = await _stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (len <= 0) break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, len);
                    MessageReceived?.Invoke(this, msg);
                }
            }
            catch { }
            finally
            {
                ConnectionStatusChanged?.Invoke(this, false);
            }
        }

        public async Task DisconnectAsync()
        {
            _cts?.Cancel();

            if (_client != null)
            {
                _client.Close();
                _client.Dispose();
            }

            ConnectionStatusChanged?.Invoke(this, false);
            await Task.CompletedTask;
        }

        public async Task<bool> PublicAsync(string ip, int port, byte[] data)
        {
            if (!IsConnected) return false;

            try
            {
                await _stream.WriteAsync(data, 0, data.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PublicAsync(string ip, int port, string data)
        {
            return await PublicAsync(ip, port, Encoding.UTF8.GetBytes(data));
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _cts?.Cancel();
                _stream?.Dispose();
                _client?.Dispose();
                _isDisposed = true;
            }
        }
    }
}
