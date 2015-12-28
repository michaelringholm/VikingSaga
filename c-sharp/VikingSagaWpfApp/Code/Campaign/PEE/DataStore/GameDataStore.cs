using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using VikingSaga.Code.Campaign.PEE.DataStore;

namespace VikingSaga.Code.Campaign.PEE
{
    // Possible implementation of effective remote store:
    //   Key/value DB at server (key = unique ID/[key])
    //   Fetch all at startup.
    //   Send batched changes at intervals or if ForceSave().

    public class XmlFileGameDataStore : IGameDataStore
    {
        public class DataRoot
        {
            public DataRoot()
            {
                Values = new Dictionary<DataStoreKey, string>();
            }

            public Dictionary<DataStoreKey, string> Values { get; set; }
        }

        public string StoreFile { get; private set; }

        private object _lock = new object();
        private DataRoot _root = new DataRoot();

        public static XmlFileGameDataStore CreateNew(string storeFile)
        {
            if (File.Exists(storeFile))
                throw new InvalidOperationException("Cannot create new store, file already exists");

            var newStore = new XmlFileGameDataStore(storeFile);
            newStore.ForceSave();
            return newStore;
        }

        public static XmlFileGameDataStore Load(string storeFile)
        {
            if (!File.Exists(storeFile))
                throw new InvalidOperationException("Cannot load store, file does not exist");

            var existingStore = new XmlFileGameDataStore(storeFile);
            existingStore.Load();
            return existingStore;
        }

        private XmlFileGameDataStore(string storeFile)
        {
            StoreFile = storeFile;
        }

        public void ForceSave()
        {
            var pairs = new List<KeyValuePair<string, string>>();
            foreach(var pair in _root.Values)
            {
                pairs.Add(new KeyValuePair<string, string>(pair.Key.ToString(), pair.Value));
            }
            string xml = Serialize(pairs);
            File.WriteAllText(StoreFile, xml);
        }

        private void Load()
        {
            string data = File.ReadAllText(StoreFile);
            var pairs = Deserialize<List<KeyValuePair<string, string>>>(data);
            _root.Values.Clear();
            foreach(var pair in pairs)
            {
                var dataStoreKey = (DataStoreKey)Enum.Parse(typeof(DataStoreKey), pair.Key);
                _root.Values.Add(dataStoreKey, pair.Value);
            }
        }

        public void Set(DataStoreKey key, DTO value, bool throwErrorIfExists = false)
        {
            lock (_lock)
            {
                if (throwErrorIfExists && _root.Values.ContainsKey(key))
                    throw new InvalidOperationException("Key already exists: " + key);

                string serialized = Serialize(value);
                _root.Values[key] = serialized;
            }
        }

        public DTO Get(DataStoreKey key)
        {
            lock (_lock)
            {
                string data = _root.Values[key];
                DTO result = Deserialize<DTO>(data);
                return result;
            }
        }

        public DTO GetOrDefault(DataStoreKey key)
        {
            DTO result;
            if (!TryGet(key, out result))
            {
                result = default(DTO);
            }
            return result;
        }

        public bool TryGet(DataStoreKey key, out DTO value)
        {
            lock (_lock)
            {
                value = null;

                if (!Exists(key))
                    return false;

                value = Get(key);
                return true;
            }
        }

        public bool Exists(DataStoreKey key)
        {
            return _root.Values.ContainsKey(key);
        }

        private T Deserialize<T>(string data)
        {
            var serializer = new XmlSerializer(typeof(T));
            object result = serializer.Deserialize(new StringReader(data));
            return (T)result;
        }

        private string Serialize<T>(T data)
        {
            var serializer = new XmlSerializer(data.GetType());
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, data);
            return stringWriter.ToString();
        }
    }
}
