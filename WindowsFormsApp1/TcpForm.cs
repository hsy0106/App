using MessagePublic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class TcpForm : Form
    {
        private TcpListener server = null;
        private TcpClient client = null;
        private NetworkStream clientStream = null;
        private Thread serverThread = null;
        private bool isServerRunning = false;
        private bool isClientConnected = false;

        public TcpForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; // 允许跨线程操作UI（仅用于示例）
        }

        #region 服务器端代码
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (isServerRunning)
            {
                StopServer();
                return;
            }

            try
            {
                IPAddress ip = IPAddress.Parse(txtServerIP.Text);
                int port = int.Parse(txtServerPort.Text);

                server = new TcpListener(ip, port);
                server.Start();
                isServerRunning = true;

                serverThread = new Thread(new ThreadStart(ServerProcess));
                serverThread.IsBackground = true;
                serverThread.Start();

                btnStartServer.Text = "停止服务器";
                AppendServerLog($"服务器已启动，监听 {ip}:{port}");
            }
            catch (Exception ex)
            {
                AppendServerLog($"服务器启动失败: {ex.Message}");
            }
        }

        private void ServerProcess()
        {
            while (isServerRunning)
            {
                try
                {
                    TcpClient connectedClient = server.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.IsBackground = true;
                    clientThread.Start(connectedClient);

                    AppendServerLog($"客户端已连接: {connectedClient.Client.RemoteEndPoint}");
                }
                catch (Exception ex)
                {
                    if (isServerRunning)
                        AppendServerLog($"接受客户端连接时出错: {ex.Message}");
                }
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient connectedClient = (TcpClient)obj;
            NetworkStream stream = connectedClient.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (isServerRunning)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendServerLog($"收到消息: {receivedData}");

                    // 回复客户端
                    string response = $"服务器已收到: {receivedData}";
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseData, 0, responseData.Length);
                }
            }
            catch (Exception ex)
            {
                AppendServerLog($"处理客户端通信时出错: {ex.Message}");
            }
            finally
            {
                stream.Close();
                connectedClient.Close();
                AppendServerLog($"客户端已断开连接");
            }
        }

        private void StopServer()
        {
            isServerRunning = false;
            server?.Stop();
            serverThread?.Join();
            server = null;
            serverThread = null;

            btnStartServer.Text = "启动服务器";
            AppendServerLog("服务器已停止");
        }

        #endregion

        #region 客户端代码

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (isClientConnected)
            {
                Disconnect();
                return;
            }

            try
            {
                string ip = txtClientIP.Text;
                int port = int.Parse(txtClientPort.Text);

                client = new TcpClient();
                client.Connect(ip, port);
                clientStream = client.GetStream();

                Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.IsBackground = true;
                receiveThread.Start();

                isClientConnected = true;
                btnConnect.Text = "断开连接";
                AppendClientLog($"已连接到服务器 {ip}:{port}");
            }
            catch (Exception ex)
            {
                AppendClientLog($"连接服务器失败: {ex.Message}");
            }
        }

        private void ReceiveData()
        {
            byte[] buffer = new byte[1024];

            while (isClientConnected)
            {
                try
                {
                    int bytesRead = clientStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendClientLog($"收到回复: {receivedData}");
                }
                catch (Exception ex)
                {
                    if (isClientConnected)
                        AppendClientLog($"接收数据时出错: {ex.Message}");
                    break;
                }
            }

            Disconnect();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!isClientConnected)
            {
                AppendClientLog("请先连接到服务器");
                return;
            }

            try
            {
                string message = txtMessage.Text;
                if (string.IsNullOrEmpty(message)) return;

                byte[] data = Encoding.UTF8.GetBytes(message);
                clientStream.Write(data, 0, data.Length);
                AppendClientLog($"已发送: {message}");
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                AppendClientLog($"发送消息失败: {ex.Message}");
                Disconnect();
            }
        }

        private void Disconnect()
        {
            isClientConnected = false;
            clientStream?.Close();
            client?.Close();
            clientStream = null;
            client = null;

            btnConnect.Text = "连接服务器";
            AppendClientLog("已断开与服务器的连接");
        }

        #endregion

        #region 日志方法

        private void AppendServerLog(string message)
        {
            rtbServerLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            rtbServerLog.ScrollToCaret();
        }

        private void AppendClientLog(string message)
        {
            rtbClientLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            rtbClientLog.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopServer();
            Disconnect();
        }

        #endregion

        private void TcpForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }



 

    }
}