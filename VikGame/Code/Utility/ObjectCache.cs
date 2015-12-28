using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vik.Code.Utility
{
    class ObjectCache<T>
    {
        private Dictionary<string, List<T>> _controlMap = new Dictionary<string, List<T>>();
        private object _lock = new object();
        private Func<string, T> _objectCreateMethod;

        public ObjectCache(Func<string, T> ObjectCreateMethod)
        {
            _objectCreateMethod = ObjectCreateMethod;
        }

        public void Clear()
        {
            lock (_lock)
            {
                _controlMap.Clear();
            }
        }

        private List<T> GetItemsList(string id)
        {
            List<T> items;
            if (!_controlMap.TryGetValue(id, out items))
            {
                // First use of key, add empty list
                items = new List<T>();
                _controlMap[id] = items;
            }
            return items;
        }

        public void Return(T item, string id)
        {
            lock (_lock)
            {

            }
        }

        public T Get(string id)
        {
            lock (_lock)
            {
                List<T> items = GetItemsList(id);
                T result;
                if (items.Count == 0)
                {
                    result = _objectCreateMethod(id);
                }
                else
                {
                    result = items[0];
                    items.RemoveAt(0);
                }

                return result;
            }
        }
    }
}
