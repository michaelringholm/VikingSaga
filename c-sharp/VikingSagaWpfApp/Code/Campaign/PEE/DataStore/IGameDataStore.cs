using System.Runtime.Serialization;
using VikingSaga.Code.Campaign.PEE.DataStore;

namespace VikingSaga.Code.Campaign.PEE
{
    public interface IGameDataStore
    {
        void Set(DataStoreKey key, DTO value, bool throwErrorIfExists = false);
        DTO Get(DataStoreKey key);
        DTO GetOrDefault(DataStoreKey key);
        bool TryGet(DataStoreKey key, out DTO value);
        bool Exists(DataStoreKey key);

        void ForceSave();
    }
}
