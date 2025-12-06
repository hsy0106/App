using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.MQ.Topic
{
    public class TopicMessageQueue<T>
    {
        public ConcurrentDictionary<string,BlockingCollection<Message<T>>> _topic = 
            new ConcurrentDictionary<string, BlockingCollection<Message<T>>>();


        private BlockingCollection<Message<T>> GetQueue(string topic)
        {
            return _topic.GetOrAdd(topic, new BlockingCollection<Message<T>>());
        }
        public void Publish(string topic,T body)
        {
            var queue = GetQueue(topic);
            queue.Add(new Message<T>() { Body = body });
        }

        public Message<T> Subscribe(string topic)
        {
            var queue  = GetQueue(topic);
            return queue.Take();
        }
    }
        
}
