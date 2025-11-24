using App;
using MessagePublic;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp1.Enum;
using static WindowsFormsApp1.ChatServerForm;

namespace WindowsFormsApp1
{
    // 聊天框
    public partial class ChatForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private string selectedUser = "";

        public ChatForm(string userName)
        {
            InitializeComponent();
            this.Text = $"聊天客户端 - {userName}"; // 在标题显示用户名
            listBoxUsers.SelectedIndexChanged += listBoxUsers_SelectedIndexChanged;
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[4096];
            int byteCount;

            try
            {
                while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string json = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    ChatMessage msg = JsonConvert.DeserializeObject<ChatMessage>(json);
                    if (msg == null) continue;

                    if (msg.Type == MessageType.UserList)
                    {
                        string[] users = msg.Data.Split(',');
                        UpdateUserList(users);
                    }
                    else if (msg.Type == MessageType.Private)
                    {
                        AppendText($"(Private){msg.From}: {msg.Data}");
                    }
                    else if (msg.Type == MessageType.Broadcast)
                    {
                        AppendText($"{msg.From}: {msg.Data}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogServer.Error($"Receive error: {ex.Message}");
                Log($"Receive error: {ex.Message}");
            }
        }

        private void UpdateUserList(string[] users)
        {
            if (listBoxUsers.InvokeRequired)
            {
                listBoxUsers.Invoke(new Action<string[]>(UpdateUserList), new object[] { users });
            }
            else
            {
                listBoxUsers.Items.Clear();
                foreach (var u in users)
                {
                    if (!string.IsNullOrEmpty(u))
                        listBoxUsers.Items.Add(u);
                }
            }
        }

        private void AppendText(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendText), msg);
            }
            else
            {
                textBoxChat.AppendText(msg + Environment.NewLine);
            }
        }

        private void Log(string message)
        {
            if (textBoxChat.InvokeRequired)
            {
                textBoxChat.Invoke(new Action<string>(Log), message);
            }
            else
            {
                textBoxChat.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }
        //连接到服务器
        private void btnClient_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(IP.Text);
            int port = int.Parse(Port.Text);
            client = new TcpClient(ip.ToString(), port);
            stream = client.GetStream();

            receiveThread = new Thread(ReceiveMessages);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Log($"Connected to Server Port: {port}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                LogServer.Warn("Not connected to server");
                Log("Not connected to server");
                return;
            }

            string msgText = textBoxInput.Text;
            if (string.IsNullOrEmpty(msgText)) return;

            ChatMessage msg = new ChatMessage
            {
                From = client.Client.LocalEndPoint.ToString(),
                Data = msgText
            };

            if (string.IsNullOrEmpty(selectedUser))
            {
                msg.Type = MessageType.Broadcast; // 广播
            }
            else
            {
                msg.Type = MessageType.Private; // 私聊
                msg.To = selectedUser;
            }

            string json = JsonConvert.SerializeObject(msg);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            stream.Write(buffer, 0, buffer.Length);

            textBoxInput.Clear();
        }

        private void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Point mousePos = listBoxUsers.PointToClient(Cursor.Position);

            // 检查是否点击在空白处
            int index = listBoxUsers.IndexFromPoint(mousePos);
            if (index == ListBox.NoMatches)
            {
                selectedUser = "";  // 清空选中用户，切换回广播模式
                Log("已切换至广播模式");
            }
            else
            {
                selectedUser = listBoxUsers.SelectedItem?.ToString();
                Log($"已选择私聊对象: {selectedUser}");
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stream?.Close();
            client?.Close();
            receiveThread?.Abort();
        }

        private void ChatForm_Load_1(object sender, EventArgs e)
        {

            try
            {
                IPAddress ip = IPAddress.Parse(IP.Text);
                int port = int.Parse(Port.Text);
                client = new TcpClient(ip.ToString(), port);
                stream = client.GetStream();

                receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                LogServer.Info($"Connected to Server Port: {port}");
                Log($"Connected to Server Port: {port}");
            }
            catch (Exception ex)
            {
                LogServer.Error($"Connected to Server Port Err: {ex.ToString()}");
            }
         
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
