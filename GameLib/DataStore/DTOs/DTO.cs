using GameLib.Utility;
using System;

namespace GameLib.DataStore.DTOs
{
    public abstract class DTO
    {
        private DataStoreKey _storeKey { get; set; }
        private IGameDataStore _dataStore;

        public DataStoreKey GetStoreKey() { return _storeKey; }

        public static T LoadOrCreate<T>(IGameDataStore dataStore, DataStoreKey storeKey) where T : DTO, new()
        {
            T result;
            if (!dataStore.TryGet(storeKey, out result))
            {
                result = new T();
            }

            result._storeKey = storeKey;
            result._dataStore = dataStore;

            return result;
        }

        protected DTO() { }

        public void Store()
        {
            using (new AutoTimer("Store " + _storeKey))
            {
                _dataStore.Set(_storeKey, this);
            }
        }
    }
}
