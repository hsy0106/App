using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.MQ
{
    public class MessageQueue<T>
    {
        private BlockingCollection<T> _queue = new BlockingCollection<T>();

        //发送消息
        public void Enqueue(T item)
        {
            _queue.Add(item);
        }
        //取消息
        public T Dequeue()
        {
            return _queue.Take();
        }
    }
}
