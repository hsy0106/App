using System;

namespace AppMqtt
{
    public class MqttMessageEventArgs : EventArgs
    {
        public string Topic { get; set; }

        public string Payload { get; set; }

        public byte[] PayloadBytes { get; set; }

        public int QoS { get; set; }

        public bool Retain { get; set; }

        public MqttMessageEventArgs(string topic, string payload, byte[] payloadBytes, int qos, bool retain)
        {
            Topic = topic;
            Payload = payload;
            PayloadBytes = payloadBytes;
            QoS = qos;
            Retain = retain;
        }
    }
}
