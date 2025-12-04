using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
//订阅者
namespace AppTools.Observe
{
    public class Subscriber<T> : ISubscriber<T>
    {

        public string Name { get; set; }

        public Subscriber(string name)
        {
            Name = name;
        }
        public void Subscribe(IPublisher<T> publisher)
        {
            publisher.MessageEvent += OnMessageReceived;
        }

        public void Unsubscribe(IPublisher<T> publisher)
        {
            publisher.MessageEvent -= OnMessageReceived;
        }

        private void OnMessageReceived(T message)
        {
            Console.WriteLine($"{Name} 收到消息：{message}");
        }

    }
}
