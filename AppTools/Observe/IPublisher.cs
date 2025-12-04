using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.Observe
{
    public interface IPublisher<T>
    {
        event Action<T> MessageEvent;

        void Publish(T message);
    }

    public interface ISubscriber<T>
    {
        void Subscribe(IPublisher<T> publisher);
        void Unsubscribe(IPublisher<T> publisher);
    }
}
