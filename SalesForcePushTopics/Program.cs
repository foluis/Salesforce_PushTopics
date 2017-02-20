using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForcePushTopics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Authenticating.\n");
            var salesforceOauth = new SalesforceOauth();
            Dictionary<string, string> authenticationResult = salesforceOauth.Login();

            Console.WriteLine("Enabling protocol.\n");
            var localBayeuxClient = new LocalBayeuxClient();
            var bayeuxClient = localBayeuxClient.CreateClient(authenticationResult["access_token"], authenticationResult["instance_url"]);

            Console.WriteLine("Connecting to push topic Opportunities\n");
            var pushTopicConnection = new PushTopicConnection(bayeuxClient);
            pushTopicConnection.Connect();

            Console.WriteLine("Press any key to shut down.\n");
            Console.ReadKey();

            Console.WriteLine("Shutting down...");
            pushTopicConnection.Disconect();
        }
    }
}
