using System;
using System.ServiceModel;
using VikingSaga.Code.Battles.Players.Network;
using GameLib.Battles.Cards;
using GameLib.Battles.Players.AI;

namespace GameLib.Battles.Players
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall)]
    public class NetworkPlayer : Player, IRemoteCommand
    {
        public static NetworkPlayer PlayerHack;

        private static ServiceHost _host;

        private void Listen()
        {
            PlayerHack = this;

            if (_host == null || _host.State != CommunicationState.Opened)
            {
                _host = new ServiceHost(typeof(NetworkPlayer), new Uri("net.tcp://localhost:8080/"));
                _host.AddServiceEndpoint(typeof(IRemoteCommand), new NetTcpBinding(), "VikingSaga");
                _host.Open();
            }
        }

        public void Command(string cmd)
        {
            NetworkPlayer.PlayerHack.RunCommand(cmd);
        }

        // Ex: 0,OP
        public void RunCommand(string cmd)
        {
            try
            {
                char cardPos = cmd[0];
                string strTarget = cmd.Substring(2).ToUpper();
                CardBattle.CardTargetFlags target = (CardBattle.CardTargetFlags)Enum.Parse(typeof(CardBattle.CardTargetFlags), strTarget);

                AiHelper.PlayCommand(this, cardPos, target);
            }
            catch(Exception e)
            {
                Observer.ShowPlayerInfo(this, e.Message);
            }
        }

        public override void TakeTurn()
        {
            Listen();
            Observer.ShowPlayerInfo(this, "Waiting...");

            Observer.WaitForHumanEndTurn();
        }
    }
}
