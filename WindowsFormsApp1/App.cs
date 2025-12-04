using App.Server;
using AppMqtt;
using AppTcp;
using AppTools;
using AppTools.ExportExport;
using AppTools.Observe;
using AppTools.Serialize.Server;
using AppTools.Serialize.XmlEntity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AppTools.Serialize.Server.ExcelOperation;

namespace WindowsFormsApp1
{
    public partial class App : Form
    {


        private IMqttService _mqtt;
        private ITcpService _tcp;

        private ConnectionMultiplexer _redis;
        private IDatabase _database;
        bool redisState;

        // Redis配置
        private string _redisConnectionString = "localhost:6379,password=,defaultDatabase=0,connectTimeout=5000,syncTimeout=5000";
        private string _instanceName = "MyApp:";

        public App()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (_mqtt != null && _mqtt.IsConnected)
            {
                MessageBox.Show("已连接 MQTT 服务端");
                return;
            }

            try
            {
                // 创建 MQTT 对象
                _mqtt = new MqttServer();

                // 绑定连接状态事件
                _mqtt.ConnectionStatusChanged += (s, connected) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        MqttState.Text = connected ? "已连接" : "未连接";
                    }));
                };

                // 接收消息
                _mqtt.MessageReceived += (s, msg) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        txtLog.AppendText($"收到 Topic={msg.Topic}, Payload={msg.Payload}\r\n");
                    }));
                };

                // 连接参数
                var options = new MqttOptions
                {
                    Server = "localhost",
                    Port = 1883,
                    ClientId = "1",
                    Username = "",
                    Password = "",
                    AutoReconnect = true
                };

                // 连接
                bool ok = await _mqtt.ConnectAsync(options);

                if (ok)
                {
                    txtLog.AppendText("MQTT 连接成功\r\n");

                    // 订阅
                    string topic = txtSubTopic.Text;
                    await _mqtt.SubscribeAsync(topic);

                    txtLog.AppendText($"已订阅主题：{topic}\r\n");
                }
                else
                {
                    txtLog.AppendText("MQTT 连接失败\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接异常: " + ex.Message);
            }
        }

        //MQTT连接
        private async void button2_Click(object sender, EventArgs e)
        {
            if (_mqtt == null || !_mqtt.IsConnected)
            {
                MessageBox.Show("请先连接 MQTT");
                return;
            }

            string topic = txtSubTopic.Text;
            string payload = txtPubMessage.Text;

            bool ok = await _mqtt.PublishAsync(topic, payload);

            txtLog.AppendText(ok
                ? $"发送成功 → Topic={topic}, Message={payload}\r\n"
                : "发送失败\r\n");
        }

        private void Mqtt_Load(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            _tcp = new TcpServer();

            _tcp.ConnectionStatusChanged += (s, ok) =>
            {
                lblStatus.Text = ok ? "已连接" : "未连接";
            };

            _tcp.MessageReceived += (s, msg) =>
            {
                Invoke(new Action(() =>
                {
                    txtLog.AppendText("收到：" + msg + "\r\n");
                }));
            };

            bool connectOK = await _tcp.ConnectAsync(new TcpOptions
            {
                Ip = txtIP.Text,
                Port = int.Parse(txtPort.Text)
            });

            if (connectOK)
                txtLog.AppendText("连接成功\r\n");
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await _tcp.PublicAsync(txtIP.Text, int.Parse(txtPort.Text) , txtPubMessage.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChatServerForm form = new ChatServerForm();
                        form.Show();
        }
        private string FindFile(string directoryPath, string fileName)
        {
            // 在指定目录下查找文件，按文件名匹配（区分大小写）
            string[] files = Directory.GetFiles(directoryPath, fileName, SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                return files[0];
            }
            return null;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "XML 文件 (*.XML)|*.XML|所有文件 (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullPath = openFileDialog.FileName;
                string fileName = Path.GetFileName(fullPath);
                string directoryPath = Path.GetDirectoryName(fullPath);
                string filePath = FindFile(directoryPath, fileName);


                string err = "";
                XmlOperation xo = new XmlOperation();

                bool result = xo.OpenXML(filePath, ref err);

                if (result == true)
                {
                    KocosResults closeData = xo.GetResult("sn001", "C");
                    KocosResults openData = xo.GetResult("sn001", "O");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "XLS 文件 (*.xls)|*.xls|所有文件 (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullPath = openFileDialog.FileName;
                string fileName = Path.GetFileName(fullPath);
                string directoryPath = Path.GetDirectoryName(fullPath);
                string filePath = FindFile(directoryPath, fileName);


                string err = "";
                ExcelOperation ex = new ExcelOperation();

                bool result = ex.OpenExcel(filePath, ref err);

                if (result == true)
                {
                    KocosData data = ex.GetResult(fullPath);
                }
            }
        }


        public void UpdateConnectionStatus()
        {
            if (redisState)
            {
                RedisState.Text = "Redis状态: 已连接";
                RedisState.ForeColor = System.Drawing.Color.Green;
                button8.Text = "断开连接";
                button9.Enabled = true;
     
            }
            else
            {
                RedisState.Text = "Redis状态: 未连接";
                RedisState.ForeColor = System.Drawing.Color.Red;
                button8.Text = "已连接";

           }
        }
        private void ConnectToRedis()
        {
            try
            {
                // 创建连接配置
                var config = ConfigurationOptions.Parse(_redisConnectionString);
                config.AbortOnConnectFail = false;
                config.ConnectTimeout = 5000;
                config.SyncTimeout = 5000;

                // 连接Redis
                _redis = ConnectionMultiplexer.Connect(config);
                _database = _redis.GetDatabase();

                // 测试连接
                var pingResult = _database.Ping();
                redisState = true;

                // 注册连接事件
                _redis.ConnectionFailed += (sender, e) =>
                {
                    redisState = false;
                    this.Invoke(new Action(() => UpdateConnectionStatus()));
                    MessageBox.Show("Redis连接失败！", "连接错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                };

                _redis.ConnectionRestored += (sender, e) =>
                {
                    redisState = true;
                    this.Invoke(new Action(() => UpdateConnectionStatus()));
                    MessageBox.Show("Redis连接已恢复！", "连接恢复", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                UpdateConnectionStatus();
                MessageBox.Show($"Redis连接成功！\nPing: {pingResult.TotalMilliseconds}ms", "连接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                redisState = false;
                UpdateConnectionStatus();
                throw new Exception($"连接Redis失败: {ex.Message}");
            }
        }

        public void DisconnectFromRedis()
        {

        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (!redisState)
                {
                    // 连接Redis
                    ConnectToRedis();
                }
                else
                {
                    // 断开连接
                    DisconnectFromRedis();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!redisState)
            {
                MessageBox.Show("请先连接Redis！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
            
                SendUserData();

       
                SendMessageData();

      
                SendConfigData();

                MessageBox.Show("数据发送成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendUserData()
        {
            // 发送用户信息到Redis
            var user = new
            {
                Id = 1,
                Name = "张三",
                Email = "zhangsan@example.com",
                Age = 25,
                CreateTime = DateTime.Now
            };

            string userKey = $"{_instanceName}user:{user.Id}";
            string userJson = JsonConvert.SerializeObject(user, Formatting.Indented);

            _database.StringSet(userKey, userJson, TimeSpan.FromHours(1));

        }

        private void SendMessageData()
        {
            // 发送消息到Redis列表
            var messages = new[]
            {
                new { Id = 1, Content = "Hello Redis!", Time = DateTime.Now },
                new { Id = 2, Content = "这是一条测试消息", Time = DateTime.Now.AddMinutes(1) },
                new { Id = 3, Content = "Redis很好用", Time = DateTime.Now.AddMinutes(2) }
            };

            foreach (var message in messages)
            {
                string messageJson = JsonConvert.SerializeObject(message);
                _database.ListRightPush($"{_instanceName}messages", messageJson);
            }
        }

        private void SendConfigData()
        {
            // 发送配置信息到Redis哈希
            var configs = new Dictionary<string, string>
            {
                ["app_name"] = "MyTestApp",
                ["version"] = "1.0.0",
                ["max_connections"] = "100",
                ["timeout"] = "30"
            };

            foreach (var config in configs)
            {
                _database.HashSet($"{_instanceName}config", config.Key, config.Value);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!redisState)
            {
                MessageBox.Show("请先连接Redis！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 清空显示区域
                txtLog.Clear();

                // 读取用户数据
                ReadUserData();

                // 读取消息数据
                ReadMessageData();

                // 读取配置数据
                ReadConfigData();

                // 读取所有键
                ReadAllKeys();

                MessageBox.Show("数据读取完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReadUserData()
        {
            txtLog.AppendText("=== 用户数据 ===\r\n");

            string userKey = $"{_instanceName}user:1";
            var userJson = _database.StringGet(userKey);

            if (userJson.HasValue)
            {
                txtLog.AppendText($"用户数据: {userJson}\r\n\r\n");
            }
            else
            {
                txtLog.AppendText("未找到用户数据\r\n\r\n");
            }
        }

        private void ReadMessageData()
        {
            txtLog.AppendText("=== 消息数据 ===\r\n");

            var messages = _database.ListRange($"{_instanceName}messages", 0, -1);

            if (messages.Length > 0)
            {
                foreach (var messageJson in messages)
                {
                    if (messageJson.HasValue)
                    {
                        txtLog.AppendText($"消息: {messageJson}\r\n");
                    }
                }
                txtLog.AppendText($"总共 {messages.Length} 条消息\r\n\r\n");
            }
            else
            {
                txtLog.AppendText("未找到消息数据\r\n\r\n");
            }
        }

        private void ReadConfigData()
        {
            txtLog.AppendText("=== 配置数据 ===\r\n");

            var configs = _database.HashGetAll($"{_instanceName}config");

            if (configs.Length > 0)
            {
                foreach (var config in configs)
                {
                    txtLog.AppendText($"{config.Name}: {config.Value}\r\n");
                }
                txtLog.AppendText($"总共 {configs.Length} 个配置项\r\n\r\n");
            }
            else
            {
                txtLog.AppendText("未找到配置数据\r\n\r\n");
            }
        }

        private void ReadAllKeys()
        {
            txtLog.AppendText("=== 所有键 ===\r\n");

            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints()[0]);
                var keys = server.Keys(pattern: $"{_instanceName}*");

                int count = 0;
                foreach (var key in keys)
                {
                    txtLog.AppendText($"{key}\r\n");
                    count++;
                }
                txtLog.AppendText($"总共 {count} 个键\r\n");
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"获取键列表失败: {ex.Message}\r\n");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {


            IPublisher<string> publisher = new Publisher<string>();
            ISubscriber<string> sub1 = new Subscriber<string>("小明");
            ISubscriber<string> sub2 = new Subscriber<string>("小红");

            sub1.Subscribe(publisher);
            sub2.Subscribe(publisher);

            publisher.Publish("明天有雨");


        }

        private void button12_Click(object sender, EventArgs e)
        {
            List<ExportData> data = new List<ExportData>();
            for (int i = 1; i <= 3; i++)
            {
                ExportData ed = new ExportData
                {
                    Id = i.ToString(),
                    SN = "SN" + i.ToString("D4"),
                    Strength = 50 + i * 0.5,

                };
                data.Add(ed);
            }


            var exporter = new ExcelExportServer<ExportData>(data);
            exporter.ExportToExcel(data);

        }

        private class ExportData 
        { 

            public string Id { get; set; }
            public string SN { get; set; }
            public double Strength { get; set; }
        }
        //读取INI文件
        private void button13_Click(object sender, EventArgs e)
        {
            string value = InIServer.Read("Server", "Host");
        }
    }
}
