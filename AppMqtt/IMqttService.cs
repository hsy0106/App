using System;
using System.Threading.Tasks;

namespace AppMqtt
{
    public interface IMqttService
    {
        bool IsConnected { get; }
        //连接回调
        event EventHandler<bool> ConnectionStatusChanged;

        event EventHandler<MqttMessageEventArgs> MessageReceived;

        Task<bool> ConnectAsync(MqttOptions options);

        Task DisconnectAsync();

        Task<bool> SubscribeAsync(string topic, int qos = 1);

        Task<bool> UnsubscribeAsync(string topic);

        Task<bool> PublishAsync(string topic, string payload, int qos = 1, bool retain = false);

        Task<bool> PublishAsync(string topic, byte[] payload, int qos = 1, bool retain = false);
    }
}
