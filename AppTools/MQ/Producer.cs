using AppTools.MQ.Topic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.MQ
{
    public class Producer<T>
    {
        private TopicMessageQueue<T> _queue;

        public Producer(TopicMessageQueue<T> queue)
        {
            _queue = queue;
        }

        public void Send(string topic, T body)
        {
            Console.WriteLine($"[Producer] Topic:{topic}, Body:{body}");
            _queue.Publish(topic, body);
        }
    }
}
