using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GameLib.DataStore.DTOs;

namespace GameLib.DataStore
{
    // Possible implementation of efficient remote store:
    //   Key/value DB at server (key = unique ID/[key])
    //   Fetch all at startup.
    //   Send batched changes at intervals or if ForceCommit().

    public interface IGameDataStoreErrorCallback
    {
        // Value in key/value pair could not be serialized or deserialized
        void OnValueError(string data, Exception e);
    }

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
        private IGameDataStoreErrorCallback _errorCallback;

        public static XmlFileGameDataStore LoadOrCreate(string storeFile, IGameDataStoreErrorCallback errorCallback)
        {
            if (!File.Exists(storeFile))
                CreateNew(storeFile);

            return Load(storeFile, errorCallback);
        }

        private static void CreateNew(string storeFile)
        {
            if (File.Exists(storeFile))
                throw new InvalidOperationException("Cannot create new store, file already exists");

            var newStore = new XmlFileGameDataStore(storeFile);
            newStore.Commit();
        }

        private static XmlFileGameDataStore Load(string storeFile, IGameDataStoreErrorCallback errorCallback)
        {
            if (!File.Exists(storeFile))
                throw new InvalidOperationException("Cannot load store, file does not exist");

            var store = new XmlFileGameDataStore(storeFile);
            store._errorCallback = errorCallback;
            store.Load();
            return store;
        }

        private XmlFileGameDataStore(string storeFile)
        {
            StoreFile = storeFile;
        }

        public class MyKeyValue
        {
            public MyKeyValue() { }

            public MyKeyValue(string k, string v)
            {
                Key = k;
                Value = v;
            }

            public string Key { get; set; }
            public string Value { get; set; }
        }

        public void ForceCommit()
        {
            Commit();
        }

        private void Commit()
        {
            var pairs = new List<MyKeyValue>();
            foreach (var pair in _root.Values)
            {
                pairs.Add(new MyKeyValue(pair.Key.ToString(), pair.Value));
            }
            string xml = Serialize(pairs);
            if (xml != null)
            {
                File.WriteAllText(StoreFile, xml);
            }
        }

        private void Load()
        {
            _loadErrors = string.Empty;

            try
            {
                string data = File.ReadAllText(StoreFile);
                var pairs = Deserialize<List<MyKeyValue>>(data);
                _root.Values.Clear();

                int errorCount = 0;
                foreach (var pair in pairs)
                {
                    try
                    {
                        var dataStoreKey = (DataStoreKey)Enum.Parse(typeof(DataStoreKey), pair.Key);
                        _root.Values.Add(dataStoreKey, pair.Value);
                    }
                    catch(Exception e)
                    {
                        errorCount++;
                        const int MaxValueLen = 50;
                        string value = Trunc(pair.Value, MaxValueLen);
                        _loadErrors += string.Format("Key: [{0}], Value: [{1}], Exception: {2}\n", pair.Key, value, e.Message);
                    }
                }

                if (errorCount > 0)
                {
                    _loadErrors = string.Format("{0} of {1} items failed to load\n\n", errorCount, pairs.Count) + _loadErrors;
                    _loadErrors = string.Format("Load of {0} failed.\n", StoreFile) + _loadErrors;
                }
            }
            catch(Exception e)
            {
                _loadErrors = string.Format("Load of {0} failed, Exception: {1}", StoreFile, e.Message);
            }
        }

        private string Trunc(string s, int len)
        {
            if (s == null)
                return string.Empty;

            return s.Length <= len ? s : s.Substring(0, len) + "...";
        }

        private string _loadErrors;
        public string GetLoadErrors()
        {
            return _loadErrors;
        }

        public void Set(DataStoreKey key, DTO value = null, bool throwErrorIfExists = false)
        {
            lock (_lock)
            {
                if (throwErrorIfExists && _root.Values.ContainsKey(key))
                    throw new InvalidOperationException("Key already exists: " + key);

                string serialized = Serialize(value);
                if (serialized != null)
                {
                    _root.Values[key] = serialized;
                }
            }
        }

        public T Get<T>(DataStoreKey key) where T : DTO
        {
            lock (_lock)
            {
                string data = _root.Values[key];
                T result = Deserialize<T>(data);
                return result;
            }
        }

        public T GetOrDefault<T>(DataStoreKey key) where T : DTO
        {
            T result;
            if (!TryGet(key, out result))
            {
                result = default(T);
            }
            return result;
        }

        public bool TryGet<T>(DataStoreKey key, out T value) where T : DTO
        {
            lock (_lock)
            {
                value = null;

                if (!Exists(key))
                    return false;

                value = Get<T>(key);
                return true;
            }
        }

        public bool Exists(DataStoreKey key)
        {
            return _root.Values.ContainsKey(key);
        }

        private T Deserialize<T>(string data)
        {
            if (data == string.Empty)
                return default(T);

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                object result = serializer.Deserialize(new StringReader(data));
                return (T)result;
            }
            catch(Exception e)
            {
                _errorCallback.OnValueError(data, e);
                return default(T);
            }
        }

        private string Serialize<T>(T data)
        {
            if (data == null)
                return string.Empty;

            try
            {
                var serializer = new XmlSerializer(data.GetType());
                var stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
            catch (Exception e)
            {
                _errorCallback.OnValueError(typeof(T).Name, e);
                return null;
            }
        }
    }
}
