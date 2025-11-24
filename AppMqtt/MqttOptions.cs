using System;

namespace AppMqtt
{
    public class MqttOptions
    {
        public string Server { get; set; } = "localhost";

        public int Port { get; set; } = 1883;

        public string ClientId { get; set; } = Guid.NewGuid().ToString();

        public string Username { get; set; }

        public string Password { get; set; }

        public bool UseTls { get; set; } = false;

        public int Timeout { get; set; } = 10;

        public int KeepAlive { get; set; } = 60;

        public bool AutoReconnect { get; set; } = true;

        public int ReconnectDelay { get; set; } = 5000;
    }
}
