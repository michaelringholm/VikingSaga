using System;
using System.ServiceModel;

namespace VikingSaga.Code.BattleNs.Players.Network
{
    [ServiceContract]
    public interface IRemoteCommand
    {
        [OperationContract]
        void Command(string cmd);
    }
}
