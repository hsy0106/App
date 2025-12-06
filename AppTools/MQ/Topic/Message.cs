using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.MQ
{
    public class Message<T>
    {
        public string Topic { get; set; }

        public T Body { get; set; }
    }
}
