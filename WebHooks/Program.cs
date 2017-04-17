using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks
{
    class Program
    {
        static string signatureKey = "4DE045F02644C02CB16A777282EBA59A51093813C22BAE63030DDC2998524CD44F673DB21699EDAD3F45E534C8ECF48920D98377D7EE5CBE8C90F9333E984664";
        static void Main(string[] args)
        {

            // prod: api.authorize.net
            string baseUrl = @"https://apitest.authorize.net/rest/v1";
            var client = new RestClient();
            client.BaseUrl = new Uri(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator("288mvgDUrL", "423CUehAu3Kw7764");


            //GetListOfWebHooks(client);

            string firstWebHookId = string.Empty;
            //GetMyWebHooks(client, out firstWebHookId);
            CreateWebHooks(client);
            //CreateWebHooks_ERROR(client);

            //GetWebHook(client, firstWebHookId);
            //SetActiveInactive(client, firstWebHookId, false);
            //Delete(client, firstWebHookId);


            Console.ReadLine();
        }


        static void GetListOfWebHooks(RestClient client)
        {
            Console.WriteLine("\nGetting List of WebHooks");
            Console.WriteLine("-------------------------------------");

            var request = new RestRequest("eventtypes", Method.GET);
            
            var response = client.Execute<List<EventName>>(request);

            if (response.ErrorException != null)
            {                
                Console.WriteLine("Error retrieving response:");
                Console.WriteLine(response.ErrorException);
            }
            else
            {
                if (response.Data == null || !response.Data.Any())
                {
                    Console.WriteLine("No data returned");
                }
                else
                {
                    foreach (var item in response.Data)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
            }


            
        }


        static void GetMyWebHooks(RestClient client, out string firstWebHookId)
        {
            firstWebHookId = "";
            Console.WriteLine("\nGetting My WebHooks");
            Console.WriteLine("-------------------------------------");

            var request = new RestRequest("webhooks", Method.GET);
            
            var response = client.Execute<List<WebHook>>(request);

            if (response.ErrorException != null)
            {
                Console.WriteLine("Error retrieving response:");
                Console.WriteLine(response.ErrorException);
            }
            else
            {
                if (response.Data == null || !response.Data.Any())
                {
                    Console.WriteLine("No data returned");
                }
                else
                {
                    foreach (var item in response.Data)
                    {
                        Console.WriteLine(item.ToString());

                        if (String.IsNullOrEmpty(firstWebHookId)) firstWebHookId = item.webhookId;
                    }
                }
            }

            
        }


        static void CreateWebHooks(RestClient client)
        {
            Console.WriteLine("\nCreating WebHooks");
            Console.WriteLine("-------------------------------------");

            var request = new RestRequest("webhooks", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-type", "application/json");

            request.AddBody(new
            {
                url = "http://requestb.in/11dw4o51",
                eventTypes = new[] {
                    "net.authorize.customer.created",
                    "net.authorize.customer.deleted",
                    "net.authorize.customer.updated",
                    "net.authorize.customer.paymentProfile.created",
                    "net.authorize.customer.paymentProfile.deleted",
                    "net.authorize.customer.paymentProfile.updated",
                    "net.authorize.customer.subscription.cancelled",
                    "net.authorize.customer.subscription.created",
                    "net.authorize.customer.subscription.expiring",
                    "net.authorize.customer.subscription.suspended",
                    "net.authorize.customer.subscription.terminated",
                    "net.authorize.customer.subscription.updated",
                    "net.authorize.payment.authcapture.created",
                    "net.authorize.payment.authorization.created",
                    "net.authorize.payment.capture.created",
                    "net.authorize.payment.fraud.approved",
                    "net.authorize.payment.fraud.declined",
                    "net.authorize.payment.fraud.held",
                    "net.authorize.payment.priorAuthCapture.created",
                    "net.authorize.payment.refund.created",
                    "net.authorize.payment.void.created"
                },
                status = "active",
                name = String.Format("WebHooks {0}", DateTime.Now.ToString("yyyyMMddhhmmss"))
            });

            var response = client.Execute<List<WebHook>>(request);

            if (response.ErrorException != null)
            {
                Console.WriteLine("Error retrieving response:");
                Console.WriteLine(response.ErrorException);
            }
            else
            {                
                if (response.Data == null || !response.Data.Any())
                {
                    Console.WriteLine("No data returned");
                }
                else
                {
                    Console.WriteLine("\nResponses");
                    Console.WriteLine("-------------------------------------");


                    foreach (var item in response.Data)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }

            }

            ValidateResponse(response);
        }

        static void CreateWebHooks_ERROR(RestClient client)
        {
            Console.WriteLine("\nCreating WebHooks");
            Console.WriteLine("-------------------------------------");

            var request = new RestRequest("webhooks", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-type", "application/json");

            request.AddBody(new
            {
                url = "http://requestb.in/11dw4o51",
                eventTypes = new[] {
                    "net.authorize.customer.created",
                    "net.authorize.customer.deleted",
                    "net.authorize.customer.updated",
                    "net.authorize.customer.paymentProfile.created",
                    "net.authorize.customer.paymentProfile.deleted",
                    "net.authorize.customer.paymentProfile.updated",
                    "net.authorize.customer.subscription.cancelled",
                    "net.authorize.customer.subscription.created",
                    "net.authorize.customer.subscription.expiring",
                    "net.authorize.customer.subscription.suspended",
                    "net.authorize.customer.subscription.terminated",
                    "net.authorize.customer.subscription.updated",
                    "net.authorize.payment.authcapture.created",
                    "net.authorize.payment.authorization.created",
                    "net.authorize.payment.capture.created",
                    "net.authorize.payment.fraud.approved",
                    "net.authorize.payment.fraud.declined",
                    "net.authorize.payment.fraud.held",
                    "net.authorize.payment.priorAuthCapture.created",
                    "net.authorize.payment.refund.created",
                    "net.authorize.payment.void.created"
                },
                status = "active",
                name = String.Format("WebHooks {0}", "&")   // invalid character in WebHook Name
            });

            var response = client.Execute<List<WebHook>>(request);

            if (response.ErrorException != null)
            {
                Console.WriteLine("Error retrieving response:");
                Console.WriteLine(response.ErrorException);
            }
            else
            {
                if (response.Data == null || !response.Data.Any())
                {
                    Console.WriteLine("No data returned");
                }
                else
                {
                    Console.WriteLine("\nResponses");
                    Console.WriteLine("-------------------------------------");


                    foreach (var item in response.Data)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }

            }
        }


        static void GetWebHook(RestClient client, string WebHookId)
        {
            Console.WriteLine("\nGet WebHook");
            Console.WriteLine("-------------------------------------");

            var request = new RestRequest("webhooks/{WebHookId}", Method.GET);            
            request.AddUrlSegment("WebHookId", WebHookId);

            var response = client.Execute<WebHook>(request);

            if (response.ErrorException != null)
            {
                Console.WriteLine("Error retrieving response:");
                Console.WriteLine(response.ErrorException);
            }
            else
            {
                if (response.Data == null)
                {
                    Console.WriteLine("No data returned");
                }
                else
                {
                    Console.WriteLine(response.Data.ToString());
                }
            }

        }

        static void SetActiveInactive(RestClient client, string WebHookId, bool active)
        {
            Console.WriteLine("\nSet Inactive");
            Console.WriteLine("-------------------------------------");

            var request = new RestRequest("webhooks/{WebHookId}", Method.PUT);            
            request.AddUrlSegment("WebHookId", WebHookId);
            request.AddHeader("Content-type", "application/json");
            request.AddParameter("status", active ? "active" : "inactive");

            var response = client.Execute<WebHook>(request);

            if (response.ErrorException != null)
            {
                Console.WriteLine("Error retrieving response:");
                Console.WriteLine(response.ErrorException);
            }
            else
            {
                if (response.Data == null)
                {
                    Console.WriteLine("No data returned");
                }
                else
                {
                    Console.WriteLine(response.Data.ToString());
                }
            }

        }


        static bool Delete(RestClient client, string WebHookId)
        {
            Console.WriteLine("\nDelete WebHook");
            Console.WriteLine("-------------------------------------");

            var request = new RestRequest("webhooks/{WebHookId}", Method.DELETE);
            request.AddUrlSegment("WebHookId", WebHookId);

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                Console.WriteLine("Error retrieving response:");
                Console.WriteLine(response.ErrorException);
                return false;
            }
            else
            {
                return response.StatusCode == System.Net.HttpStatusCode.OK;   // response code 200 is successfully deleted
            }

        }

        static bool ValidateResponse(IRestResponse response)
        {
            // check if it was a POST

            var hashResponse = HashToString(response.Content, signatureKey);

            var hashHeader = response.Headers.FirstOrDefault(p => p.Name.Equals("X-ANET-Signature"));
            if (hashHeader == null) return false;
            return hashHeader.Value.ToString().Equals(hashResponse, StringComparison.InvariantCultureIgnoreCase);
           
        }

        static string HashToString(string message, string key)
        {
            // user Encoding.ASCII.GetBytes or Encoding.UTF8.GetBytes

            byte[] _key = Encoding.ASCII.GetBytes(key);
            using (var myhmacsha1 = new HMACSHA1(_key))
            {
                var hashArray = new HMACSHA512(_key).ComputeHash(Encoding.ASCII.GetBytes(message));

                //return hashArray.Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
                return Convert.ToBase64String(hashArray);
            }

        }
    }
}
