using Cometd.Bayeux;
using Cometd.Bayeux.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForcePushTopics
{
    class Listener : IMessageListener
    {
        public void onMessage(IClientSessionChannel channel, IMessage message)
        {
            Console.WriteLine(message.JSON);
        }
    }
}
