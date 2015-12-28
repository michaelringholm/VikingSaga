using System;
using GameLib.DataStore.DTOs;

namespace GameLib.DataStore
{
    public interface IGameDataStore
    {
        void Set(DataStoreKey key, DTO value = null, bool throwErrorIfExists = false);
        T Get<T>(DataStoreKey key) where T : DTO;
        T GetOrDefault<T>(DataStoreKey key) where T : DTO;
        bool TryGet<T>(DataStoreKey key, out T value) where T : DTO;
        bool Exists(DataStoreKey key);

        string GetLoadErrors();
        void ForceCommit();
    }
}
