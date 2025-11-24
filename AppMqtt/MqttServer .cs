using MQTTnet;
using MQTTnet.Client;

using MQTTnet.Protocol;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AppMqtt
{
    public class MqttServer : IMqttService, IDisposable
    {
        private IMqttClient _mqttClient;
        private MqttClientOptions _clientOptions;
        private MqttOptions _currentOptions;
        private bool _isDisposed = false;

        public bool IsConnected => _mqttClient?.IsConnected == true;

        public event EventHandler<bool> ConnectionStatusChanged;
        public event EventHandler<MqttMessageEventArgs> MessageReceived;

        public MqttServer()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var factory = new MQTTnet.MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _mqttClient.ConnectedAsync += async e =>
            {
                ConnectionStatusChanged?.Invoke(this, true);
                await Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                ConnectionStatusChanged?.Invoke(this, false);
                await Task.CompletedTask;
            };

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var payload = e.ApplicationMessage.Payload == null
                    ? ""
                    : Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                var args = new MqttMessageEventArgs(
                    e.ApplicationMessage.Topic,
                    payload,
                    e.ApplicationMessage.Payload,
                    (int)e.ApplicationMessage.QualityOfServiceLevel,
                    e.ApplicationMessage.Retain
                );

                MessageReceived?.Invoke(this, args);
                await Task.CompletedTask;
            };
        }

        public async Task<bool> ConnectAsync(MqttOptions options)
        {
            _currentOptions = options;

            var builder = new MqttClientOptionsBuilder()
                .WithClientId(options.ClientId)
                .WithTcpServer(options.Server, options.Port)
                .WithTimeout(TimeSpan.FromSeconds(options.Timeout))
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(options.KeepAlive));

            if (!string.IsNullOrEmpty(options.Username))
            {
                builder.WithCredentials(options.Username, options.Password);
            }

            if (options.UseTls)
            {
                builder.WithTls();
            }

            _clientOptions = builder.Build();

            try
            {
                await _mqttClient.ConnectAsync(_clientOptions);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MQTT连接失败: " + ex.Message);
   
                return false;

            }
        }

        public async Task DisconnectAsync()
        {
            if (_mqttClient?.IsConnected == true)
                await _mqttClient.DisconnectAsync();
        }

        public async Task<bool> SubscribeAsync(string topic, int qos = 1)
        {
            if (!_mqttClient.IsConnected)
                return false;

            try
            {
                MqttQualityOfServiceLevel level;
                if (qos <= 0)
                    level = MqttQualityOfServiceLevel.AtMostOnce;
                else if (qos >= 2)
                    level = MqttQualityOfServiceLevel.ExactlyOnce;
                else
                    level = (MqttQualityOfServiceLevel)qos;

                var filter = new MqttTopicFilterBuilder()
                    .WithTopic(topic)
                    .WithQualityOfServiceLevel(level)
                    .Build();

                await _mqttClient.SubscribeAsync(filter);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnsubscribeAsync(string topic)
        {
            if (!_mqttClient.IsConnected)
                return false;

            await _mqttClient.UnsubscribeAsync(topic);
            return true;
        }

        public async Task<bool> PublishAsync(string topic, string payload, int qos = 1, bool retain = false)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            return await PublishAsync(topic, bytes, qos, retain);
        }

        public async Task<bool> PublishAsync(string topic, byte[] payload, int qos = 1, bool retain = false)
        {
            if (!_mqttClient.IsConnected)
                return false;

            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel)qos)
                .WithRetainFlag(retain)
                .Build();

            await _mqttClient.PublishAsync(msg);
            return true;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _mqttClient?.Dispose();
                _isDisposed = true;
            }
        }
    }
}
