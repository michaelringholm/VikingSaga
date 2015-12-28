using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VikingSaga.Code.BattleNs.Players.Network;

namespace VikingCmd
{
    public static class NetworkTest
    {
        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("Format: [0-4],[PO | PE | BO0 | BO1 | BO2 | BO3 | BO4 | BE0 | BE1 | BE2 | BE3 | BE4] - ex. '0,BO1'\n");
                Console.Write("Please enter game address: ");
                string ip = Console.ReadLine();

                IRemoteCommand game;
                try
                {
                    ChannelFactory<IRemoteCommand> factory =
                        new ChannelFactory<IRemoteCommand>
                            (new NetTcpBinding(),
                            new EndpointAddress(string.Format("net.tcp://{0}/VikingSaga", ip)));

                    game = factory.CreateChannel();

                    while (true)
                    {
                        Console.Write("Enter command: ");
                        string command = Console.ReadLine();
                        game.Command(command);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    continue;
                }

            }
        }
    }
}
