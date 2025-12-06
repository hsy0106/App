using AppTools.MQ.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.MQ
{
    public class Consumer<T>
    {
        private TopicMessageQueue<T> _queue;
        private string _topic;
        private string _name;
        private bool _running = true;

        public Consumer(string name,TopicMessageQueue<T> queue,string topic)
        {
            _name = name;
            _topic = topic;
            _queue = queue;
        }
        public void Start()
        {
            Task.Run(() =>
            {
                while (_running)
                {
                    var msg = _queue.Subscribe(_topic);
                    Console.WriteLine(
                   $"[Consumer {_name}] Topic:{msg.Topic}, Body:{msg.Body}"
               );
                }
            });
        }

        public void Stop() => _running = false;
    }
}
