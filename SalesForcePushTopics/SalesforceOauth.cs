using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalesForcePushTopics
{
    public class SalesforceOauth
    {
        string loginEndpoint = ConfigurationManager.AppSettings["loginEndpoint"];
        string userName = ConfigurationManager.AppSettings["userName"];
        string password = ConfigurationManager.AppSettings["password"];
        string clientId = ConfigurationManager.AppSettings["clientId"];
        string clientSecret = ConfigurationManager.AppSettings["clientSecret"];

        public Dictionary<string, string> Login()
        {
            String jsonResponse;

            using (var client = new HttpClient())
            {
                var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", clientId},
                    {"client_secret", clientSecret},
                    {"username", userName},
                    {"password", password}
                });

                request.Headers.Add("X-PrettyPrint", "1");

                var response = client.PostAsync(loginEndpoint, request).Result;
                jsonResponse = response.Content.ReadAsStringAsync().Result;
            }

            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
            return values;
        }

        static SalesforceOauth()
        {
            // SF requires TLS 1.1 or 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }
    }
}
