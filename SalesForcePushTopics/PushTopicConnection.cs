using Cometd.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForcePushTopics
{
    public class PushTopicConnection
    {
        BayeuxClient _bayeuxClient = null;
        string channel = ConfigurationManager.AppSettings["channel"];

        public PushTopicConnection(BayeuxClient bayeuxClient)
        {
            _bayeuxClient = bayeuxClient;
        }

        public void Connect()
        {
            Console.WriteLine("Handshaking.");

            _bayeuxClient.handshake();
            _bayeuxClient.waitFor(1000, new[] { BayeuxClient.State.CONNECTED });

            Console.WriteLine("Connected.");

            _bayeuxClient.getChannel(channel).subscribe(new Listener());
            Console.WriteLine("Waiting for data from server...");
        }

        public void Disconect()
        {
            _bayeuxClient.disconnect();
            _bayeuxClient.waitFor(1000, new[] { BayeuxClient.State.DISCONNECTED });
        }
    }
}
