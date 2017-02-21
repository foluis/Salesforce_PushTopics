
using Salesforce.Common;
using Salesforce.Force;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImplementingForceToolkit
{
    class Program
    {
        //private static readonly string SecurityToken = ConfigurationManager.AppSettings["SecurityToken"];
        private static readonly string ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
        private static readonly string ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
        private static readonly string Username = ConfigurationManager.AppSettings["Username"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];// + SecurityToken;
        private static readonly string IsSandboxUser = ConfigurationManager.AppSettings["IsSandboxUser"];
        private static AuthenticationClient auth = null;

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                auth = RunSample().Result;

                var accounts = GetAccounts().Result;

               
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                var innerException = e.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);
                    Console.WriteLine(innerException.StackTrace);

                    innerException = innerException.InnerException;
                }

                Console.ReadLine(); // Prevent the app from closing so the error can be seen
            }
        }

        private static async Task<List<Account>> GetAccounts()
        {
            var client = new ForceClient(auth.InstanceUrl, auth.AccessToken, auth.ApiVersion);
            const string qry = "SELECT ID, Name FROM Account";


            var results = await client.QueryAsync<Account>(qry);

            var acaunts = new List<Account>();
            acaunts.AddRange(results.Records);
            return acaunts;
        }

        private static async Task<AuthenticationClient> RunSample()
        {
            var auth = new AuthenticationClient();

            // Authenticate with Salesforce
            Console.WriteLine("Authenticating with Salesforce");
            var url = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)
                ? "https://test.salesforce.com/services/oauth2/token"
                : "https://login.salesforce.com/services/oauth2/token";

            await auth.UsernamePasswordAsync(ConsumerKey, ConsumerSecret, Username, Password, url);
            Console.WriteLine("Connected to Salesforce");
            return auth;
        }
    }
}
