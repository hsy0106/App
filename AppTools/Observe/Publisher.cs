using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
//发布者
namespace AppTools.Observe
{
    public class Publisher<T> : IPublisher<T>
    {
        public event Action<T> MessageEvent;

   
        public void Publish(T message)
        {
            MessageEvent?.Invoke(message);
        }
    }
}
