using Cometd.Client;
using Cometd.Client.Transport;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForcePushTopics
{
    public class LocalBayeuxClient
    {
        // long pull durations
        int readTimeOut = 120 * 1000;
        string streamingEndpointURI = ConfigurationManager.AppSettings["streamingEndpointURI"];

        public BayeuxClient CreateClient(string authToken, string instanceUrl)
        {
            Console.WriteLine("Authenticating with Salesforce.");

            var options = new Dictionary<String, Object>
                {
                    { ClientTransport.TIMEOUT_OPTION, readTimeOut }
                };

            var transport = new LongPollingTransport(options);

            // add the needed auth headers
            var headers = new NameValueCollection();
            headers.Add("Authorization", "OAuth " + authToken);
            transport.AddHeaders(headers);

            // only need the scheme and host, strip out the rest
            var serverUri = new Uri(instanceUrl);
            String endpoint = String.Format("{0}://{1}{2}", serverUri.Scheme, serverUri.Host, streamingEndpointURI);

            return new BayeuxClient(endpoint, new[] { transport });
        }
    }
}
