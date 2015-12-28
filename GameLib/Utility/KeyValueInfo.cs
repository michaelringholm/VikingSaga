using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Utility
{
    public class KeyValueInfo
    {
        public string Time { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public int Count { get; set; }
    }

    public class AutoTimer : IDisposable
    {
        private Stopwatch _sw = Stopwatch.StartNew();
        private string _name;

        public AutoTimer(string name)
        {
            _name = name;
        }

        void IDisposable.Dispose()
        {
            long ms = _sw.ElapsedMilliseconds;
            KeyValueDebugInfo.SetItem(_name, string.Format("{0} ms", ms));
        }
    }

    public static class KeyValueDebugInfo
    {
        private static DateTime StartTime = DateTime.UtcNow;

        public static readonly ObservableCollection<KeyValueInfo> Items = new ObservableCollection<KeyValueInfo>();

        private static Dictionary<string, int> s_counts = new Dictionary<string, int>();

        public static void SetItem(string key, object value)
        {
            if (!s_counts.ContainsKey(key))
                s_counts[key] = 0;

            int count = s_counts[key] + 1;
            s_counts[key] = count;

            var existing = Items.FirstOrDefault(i => i.Key == key);
            Items.Remove(existing);
            var span = DateTime.UtcNow - StartTime;
            string timeStr = string.Format("{0:0.000}", span.TotalSeconds);
            Items.Add(new KeyValueInfo { Time = timeStr, Key = key, Value = value, Count = count });
        }
    }
}
