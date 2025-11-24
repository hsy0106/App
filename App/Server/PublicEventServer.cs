using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public sealed class IEvent
    {
        // 单例
        private static readonly Lazy<IEvent> _instance = new Lazy<IEvent>(() => new IEvent());
        public static IEvent Instance { get { return _instance.Value; } }

        // 存储 key-value
        private readonly ConcurrentDictionary<string, object> _store = new ConcurrentDictionary<string, object>();

        private IEvent() { }

        /// <summary>
        /// 设置值，如果 key 已存在则覆盖
        /// </summary>
        public void SetValue(string key, object value)
        {
            _store.AddOrUpdate(key, value, (k, old) => value);
        }

        /// <summary>
        /// 获取值，如果不存在返回 null
        /// </summary>
        public object GetRetainValue(string key)
        {
            object value;
            if (_store.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// 获取并尝试转换为指定类型
        /// </summary>
        public T GetRetainValue<T>(string key) where T : class
        {
            object value;
            if (_store.TryGetValue(key, out value))
            {
                return value as T;
            }
            return null;
        }

        /// <summary>
        /// 移除 key
        /// </summary>
        public bool Remove(string key)
        {
            object value;
            return _store.TryRemove(key, out value);
        }
    }
}
