using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMqtt
{
    public static class MqttFactory
    {
        public static IMqttService Create()
        {
            return new MqttServer();
        }

        public static async Task<IMqttService> CreateAndConnectAsync(MqttOptions options)
        {
            var server = new MqttServer();
            if (!await server.ConnectAsync(options))
                throw new System.Exception("MQTT连接失败");

            return server;
        }
    }
}