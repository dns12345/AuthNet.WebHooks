using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public enum WebhookEventType
    {
        CustomerEvent,
        FraudEvent,
        PaymentEvent,
        PaymentProfileEvent,
        SubscriptionEvent,
        UnknownEvent,
        InvalidEvent    // this is used if there is a problem comparing the hashes
    }

    public class Event
    {
        private string signatureKey;


        /// <summary>
        /// Initializes a new Event object
        /// </summary>
        /// <param name="SignatureKey">Note:You must have configured a Signature Key in the Authorize.Net Merchant Interface before you can 
        /// receive Webhooks notifications. This signature key is used to create a message hash to be sent with each notification that the 
        /// merchant can then use to verify the notification is genuine. The signature key can be obtained in the Authorize.Net Merchant Interface, 
        /// at Account > Settings > Security Settings > General Security Settings > API Credentials and Keys</param>
        public Event(string SignatureKey)
        {
            signatureKey = SignatureKey;        
        }


        /// <summary>
        /// Evaluate the event data to determine the type of event
        /// </summary>
        /// <param name="jsonRawBody">The WebHook's Raw JSON data posted to the notification URL</param>
        /// <param name="AnetSignature">The X-Anet-Signature (SHA512) value in the header</param>
        /// <returns>The type of WebHook event</returns>
        public WebhookEventType Evaluate(string jsonRawBody, string AnetSignature)
        {                        
            // evaluate the input parameters
            if (String.IsNullOrEmpty(jsonRawBody)) return WebhookEventType.UnknownEvent;
            if (String.IsNullOrEmpty(AnetSignature)) return WebhookEventType.UnknownEvent;
            
            // make sure the tokens match
            if (!CheckTokens(jsonRawBody, AnetSignature)) return WebhookEventType.InvalidEvent;

            // use RestSharp to deserialize
            var eventResponse = this.Deserialize<eventResponse>(jsonRawBody);

            if (eventResponse == null) return WebhookEventType.UnknownEvent;
            if (String.IsNullOrEmpty(eventResponse.eventType)) return WebhookEventType.UnknownEvent;

            return GetEventType(eventResponse.eventType);

        }


        /// <summary>
        /// Parse the event data and return customerEvent, fraudEvent, paymentEcent, paymentProfileEvent, or subscriptionEvent.  This is useful when you know wat type of Json data you have
        /// </summary>
        /// <param name="jsonRawBody">The WebHook's Raw JSON data posted to the notification URL</param>
        /// <param name="AnetSignature">The X-Anet-Signature (SHA512) value in the header</param>        
        /// <returns>customerEvent, fraudEvent, paymentEcent, paymentProfileEvent, or subscriptionEvent</returns>
        public T Evaluate<T>(string jsonRawBody, string AnetSignature) where T : eventResponse
        {
            // evaluate the input parameters
            if (String.IsNullOrEmpty(jsonRawBody)) return null;
            if (String.IsNullOrEmpty(AnetSignature)) return null;

            // make sure the tokens match
            if (!CheckTokens(jsonRawBody, AnetSignature)) return null;

            // use RestSharp to deserialize
            return this.Deserialize<T>(jsonRawBody);

        }

        /// <summary>
        /// Evaluate the event data and return the Event object
        /// </summary>
        /// <param name="jsonRawBody">The WebHook's Raw JSON data posted to the notification URL</param>
        /// <param name="AnetSignature">The X-Anet-Signature (SHA512) value in the header</param>
        /// <returns>The type of WebHook event</returns>
        public WebhookEventType Evaluate(string jsonRawBody, string AnetSignature, out object EventObj)
        {
            EventObj = null;

            // evaluate the input parameters
            if (String.IsNullOrEmpty(jsonRawBody)) return WebhookEventType.UnknownEvent;
            if (String.IsNullOrEmpty(AnetSignature)) return WebhookEventType.UnknownEvent;

            // make sure the tokens match
            if (!CheckTokens(jsonRawBody, AnetSignature)) return WebhookEventType.InvalidEvent;

            // use RestSharp to deserialize
            var eventResponse = this.Deserialize<eventResponse>(jsonRawBody);

            if (eventResponse == null) return WebhookEventType.UnknownEvent;
            if (String.IsNullOrEmpty(eventResponse.eventType)) return WebhookEventType.UnknownEvent;

            var eventType = GetEventType(eventResponse.eventType);
            switch (eventType)
            {
                case WebhookEventType.CustomerEvent:
                    EventObj = (object)this.Deserialize<customerEvent>(jsonRawBody);
                    break;

                case WebhookEventType.FraudEvent:
                    EventObj = (object)this.Deserialize<fraudEvent>(jsonRawBody);
                    break;

                case WebhookEventType.PaymentEvent:
                    EventObj = (object)this.Deserialize<paymentEvent>(jsonRawBody);
                    break;

                case WebhookEventType.PaymentProfileEvent:
                    EventObj = (object)this.Deserialize<paymentProfileEvent>(jsonRawBody);
                    break;

                case WebhookEventType.SubscriptionEvent:
                    EventObj = (object)this.Deserialize<subscriptionEvent>(jsonRawBody);
                    break;


            }

            return eventType;

        }


        /// <summary>
        /// Get the event object based on the event type enumeration
        /// </summary>
        public Type GetEventObjectType(WebhookEventType eventType)
        {
            switch (eventType)
            {
                case WebhookEventType.CustomerEvent:
                    return typeof(customerEvent);

                case WebhookEventType.FraudEvent:
                    return typeof(customerEvent);

                case WebhookEventType.PaymentEvent:
                    return typeof(customerEvent);

                case WebhookEventType.PaymentProfileEvent:
                    return typeof(customerEvent);

                case WebhookEventType.SubscriptionEvent:
                    return typeof(customerEvent);

                default:
                    return null;

            }
        }


        /// <summary>
        /// Get the event object based on the event type name
        /// </summary>
        public Type GetEventObjectType(string eventType)
        {
            return GetEventObjectType(GetEventType(eventType));
        }

     

        /// <summary>
        /// Get's the event type based on the event name.  For example, "net.authorize.customer.updated" will return CustomerEvent
        /// </summary>
        /// <param name="eventType">The name if the event type</param>
        /// <returns>The event type, or UnknownEvent if it cannot be determined</returns>
        public WebhookEventType GetEventType(string eventType)
        {
            if (String.IsNullOrEmpty(eventType)) return WebhookEventType.UnknownEvent;

            eventType = eventType.Trim();

            // DO NOT change the order of these statements
            if (eventType.StartsWith("net.authorize.payment.fraud", StringComparison.InvariantCultureIgnoreCase))
            {
                if (eventType.Equals("net.authorize.payment.fraud.held", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.payment.fraud.approved", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.payment.fraud.declined", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    return WebhookEventType.FraudEvent;
                }
            }
            else if (eventType.StartsWith("net.authorize.customer.paymentProfile", StringComparison.InvariantCultureIgnoreCase))
            {
                if (eventType.Equals("net.authorize.customer.paymentProfile.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.paymentProfile.updated", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.paymentProfile.deleted", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    return WebhookEventType.PaymentProfileEvent;
                }
            }
            else if (eventType.StartsWith("net.authorize.customer", StringComparison.InvariantCultureIgnoreCase))
            {
                if (eventType.Equals("net.authorize.customer.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.updated", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.deleted", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    return WebhookEventType.CustomerEvent;
                }
            }
            else if (eventType.StartsWith("net.authorize.customer.subscription", StringComparison.InvariantCultureIgnoreCase))
            {
                if (eventType.Equals("net.authorize.customer.subscription.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.subscription.updated", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.subscription.suspended", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.subscription.terminated", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.subscription.cancelled", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.customer.subscription.expiring", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    return WebhookEventType.SubscriptionEvent;
                }
            }
            else if (eventType.StartsWith("net.authorize.payment", StringComparison.InvariantCultureIgnoreCase))
            {
                if (eventType.Equals("net.authorize.payment.authorization.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.payment.authcapture.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.payment.capture.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.payment.refund.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.payment.priorAuthCapture.created", StringComparison.InvariantCultureIgnoreCase)
                    || eventType.Equals("net.authorize.payment.void.created", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    return WebhookEventType.PaymentEvent;
                }
            }
            else
                return WebhookEventType.UnknownEvent;


            // could not find a match
            return WebhookEventType.UnknownEvent;

        }


        private T Deserialize<T>(string jsonRawBody) where T : eventResponse            
        {
            RestResponse response = new RestResponse();
            response.Content = jsonRawBody;

            // use the RestSharp JSON deserializer; That way, we don't need another dependency on Newtonsoft.Json
            var deserial = new JsonDeserializer();

            T result = deserial.Deserialize<T>(response);
            var temp = (eventResponse)result;
            temp.responseBody = jsonRawBody;

            return result;

        }



        private bool CheckTokens(string data, string AnetSignature)
        {
            if (String.IsNullOrEmpty(data)) return false;
            if (String.IsNullOrEmpty(AnetSignature)) return false;
            if (String.IsNullOrEmpty(signatureKey)) return false;

            // generate the shaw token
            var token = GetSHAToken(data, signatureKey);
            if (String.IsNullOrEmpty(token)) return false;

            return token.Equals(AnetSignature, StringComparison.InvariantCultureIgnoreCase);

        }

        private string GetSHAToken(string data, string key)
        {
            // use Encoding.ASCII.GetBytes or Encoding.UTF8.GetBytes

            byte[] _key = Encoding.ASCII.GetBytes(key);
            using (var myhmacsha1 = new HMACSHA1(_key))
            {
                var hashArray = new HMACSHA512(_key).ComputeHash(Encoding.ASCII.GetBytes(data));

                return hashArray.Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);                
            }

        }

    }
}
