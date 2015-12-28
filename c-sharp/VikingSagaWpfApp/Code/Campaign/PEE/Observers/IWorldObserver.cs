using System;

namespace VikingSaga.Code.Campaign.PEE.Observers
{
    public interface IWorldObserver
    {
        void OnEnterMap(Map map);
    }
}
