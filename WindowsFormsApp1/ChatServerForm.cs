using App;
using App.Server;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Enum;

namespace WindowsFormsApp1
{
    // 聊天服务器
    public partial class ChatServerForm : Form
    {
        private TcpListener listener;
        private Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        private Thread listenThread;
        private bool isRunning = false;
        private object lockObj = new object();

        public ChatServerForm()
        {
            InitializeComponent();
        }
        //开启服务
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                IPAddress ip = IPAddress.Parse(IP.Text);
                int port = int.Parse(Port.Text);
                listener = new TcpListener(ip, port);
                listener.Start();
                isRunning = true;

                listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();
                LogServer.Info($"Server started on port {port}");
                Log($"Server started on port {port}");
                btnStart.Text = "StopServer";
            }
            else
            {
                StopServer();
                btnStart.Text = "StartServer";
            }
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    string endPoint = client.Client.RemoteEndPoint.ToString();

                    lock (lockObj)
                    {
                        clients[endPoint] = client;
                    }
                    LogServer.Info($"Client connected: {endPoint}");
                    Log($"Client connected: {endPoint}");
                    UpdateClientList();
                    BroadcastUserList();

                    Thread clientThread = new Thread(HandleClient);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
                catch { }
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            string clientEndPoint = client.Client.RemoteEndPoint.ToString();
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];

            try
            {
                while (true)
                {
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount <= 0) break;

                    string json = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    ChatMessage msg = JsonConvert.DeserializeObject<ChatMessage>(json);

                    if (msg == null) continue;



                    if (msg.Type == MessageType.Broadcast)
                    {
                        LogServer.Info($"{msg.From}" + ":" + $"{msg.Data}");
                        Broadcast(msg.Data, clientEndPoint);
                        Log($"{msg.From}" + ":" + $"{msg.Data}");

                    }
                    else if (msg.Type == MessageType.Private)
                    {
                        LogServer.Info($"[{msg.From}]" + "SendTo" + $"[{msg.To}]" + ":" + $"{msg.Data}");
                        SendPrivate(msg);
                        Log($"[{msg.From}]" + "SendTo" + $"[{msg.To}]" + ":" + $"{msg.Data}");
                    }
                }
            }
            catch { }
            finally
            {
                lock (lockObj)
                {
                    clients.Remove(clientEndPoint);
                }
                client.Close();
                LogServer.Info($"Client disconnected: {clientEndPoint}");
                Log($"Client disconnected: {clientEndPoint}");
                UpdateClientList();
                BroadcastUserList();
            }
        }
        //私聊
        private void SendPrivate(ChatMessage msg)
        {
            lock (lockObj)
            {
                if (clients.TryGetValue(msg.To, out TcpClient receiver))
                {
                    string json = JsonConvert.SerializeObject(msg);
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    receiver.GetStream().Write(data, 0, data.Length);
                }
            }
        }
        //广播
        private void Broadcast(string message, string sender)
        {
            ChatMessage msg = new ChatMessage
            {
                Type = MessageType.Broadcast,
                From = sender,
                Data = message
            };

            string json = JsonConvert.SerializeObject(msg);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            lock (lockObj)
            {
                foreach (var c in clients)
                {
                    try
                    {
                        c.Value.GetStream().Write(buffer, 0, buffer.Length);
                    }
                    catch { }
                }
            }
        }

        private void BroadcastUserList()
        {
            ChatMessage msg = new ChatMessage
            {
                Type = MessageType.UserList,
                Data = string.Join(",", clients.Keys)
            };

            string json = JsonConvert.SerializeObject(msg);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            lock (lockObj)
            {
                foreach (var c in clients.Values)
                {
                    try
                    {
                        c.GetStream().Write(buffer, 0, buffer.Length);
                    }
                    catch { }
                }
            }
        }

        private void StopServer()
        {
            isRunning = false;
            listener?.Stop();
            lock (lockObj)
            {
                foreach (var c in clients.Values)
                {
                    c.Close();
                }
                clients.Clear();
            }
            LogServer.Info("Server stopped");
            Log("Server stopped");
            UpdateClientList();
        }

        private void ChatServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(Log), message);
            }
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }

        private void UpdateClientList()
        {
            if (lstClients.InvokeRequired)
            {
                lstClients.Invoke(new Action(UpdateClientList));
            }
            else
            {
                lstClients.Items.Clear();
                lock (lockObj)
                {
                    foreach (var c in clients.Keys)
                    {
                        lstClients.Items.Add(c);
                    }
                }
            }
        }

        private void NewChatForm_Click(object sender, EventArgs e)
        {
            //ChatForm form = new ChatForm();
            //form.Show();

            using (var loginForm = new LoginForm())
            {
                if(loginForm.ShowDialog() == DialogResult.OK)
                {
                    string username = loginForm.UserName;
                    ChatForm form = new ChatForm(username);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("登录已取消");
                }
            }
        }

        public class ChatMessage
        {
            public MessageType Type { get; set; }
            public string From { get; set; }  // 发送方
            public string To { get; set; }    // 接收方（仅私聊）
            public string Data { get; set; }  // 内容
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var DBType = InIServer.Read("Server", "DbType");
            Log("DBType" + DBType);
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                var response = await HttpUtilsServer.PostAsync("http://127.0.0.1:4523/m2/2728289-2828826-default/341070881", new { Name = "Alice", Age = 25 }, // 匿名对象自动序列化为 JSON
                headers: new Dictionary<string, string> { { "Authorization", "Bearer token123" } }
                );
                Log(response.ToString());


            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }


        }



    }
}
