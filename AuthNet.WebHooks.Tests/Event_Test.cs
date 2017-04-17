using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace AuthNet.WebHooks.Tests
{
    [TestClass]
    public class Event_Test
    {

        // Note:You must have configured a Signature Key in the Authorize.Net Merchant Interface before you can receive Webhooks notifications. This signature key is used to create a message hash to be sent with each notification that the merchant can then use to verify the notification is genuine. The signature key can be obtained in the Authorize.Net Merchant Interface, at Account > Settings > Security Settings > General Security Settings > API Credentials and Keys
        string signatureKey = "<<Enter your signature key here >>";



        [TestMethod]
        public void Evalulate_EventTypes()
        {
            // This test only parses the webhook json data enough to determine the event type.

            // get the event data
            var events = common.EventData();

            // loop through and evaluate the event data
            var evt = new Event(signatureKey);

            foreach (var item in events)
            {
                var eventName = item.Item1;
                var X_Anet_Signature = item.Item2;
                var Raw_JSON_Body = item.Item3;

                Debug.WriteLine("Evaluating Event:");
                common.DebugLine("eventName", eventName);
                common.DebugLine("X_Anet_Signature", X_Anet_Signature);
                common.DebugLine("Raw_JSON_Body", Raw_JSON_Body);

                // get the event type from the json data, and the event type name
                var jsonEventType = evt.Evaluate(Raw_JSON_Body, X_Anet_Signature);
                var nameEventType = evt.GetEventType(eventName);

                common.DebugLine("jsonEventType", jsonEventType.ToString());
                common.DebugLine("nameEventType", nameEventType.ToString());
                


                // the event type determined from the Json data, and the name should match
                //Assert.AreEqual<WebhookEventType>(nameEventType, jsonEventType);
                if (jsonEventType == WebhookEventType.UnknownEvent) Debug.WriteLine("UNKNOWN WEBHOOK EVENT??");
                if (jsonEventType == WebhookEventType.InvalidEvent) Debug.WriteLine("INVALID WEBHOOK EVENT: SHA512 TOKEN ERROR");

                Debug.WriteLine(""); ;

            }

        }


        [TestMethod]
        public void Evalulate_EventObjects()
        {
            // This test will process the web hooks and return an object containing the parsed data
            //   We need to cast the generic object into customerEvent, fraudEvent, paymentEcent, paymentProfileEvent, or subscriptionEvent

            // get the event data
            var events = common.EventData();

            // loop through and evaluate the event data
            var evt = new Event(signatureKey);

            foreach (var item in events)
            {
                var eventName = item.Item1;
                var X_Anet_Signature = item.Item2;
                var Raw_JSON_Body = item.Item3;

                Debug.WriteLine("Evaluating Event:");
                common.DebugLine("eventName", eventName);
                common.DebugLine("X_Anet_Signature", X_Anet_Signature);
                
                // get the event type from the json data, and the event type name
                object eventObject = null;
                var jsonEventType = evt.Evaluate(Raw_JSON_Body, X_Anet_Signature, out eventObject);                
                common.DebugLine("jsonEventType", jsonEventType.ToString());
                                
                // convert the generic eventObject into customerEvent, fraudEvent, paymentEcent, paymentProfileEvent, or subscriptionEvent                
                switch (jsonEventType)
                {
                    case WebhookEventType.CustomerEvent:
                        common.DisplayItem((customerEvent)eventObject);
                        break;

                    case WebhookEventType.FraudEvent:
                        common.DisplayItem((fraudEvent)eventObject);
                        break;

                    case WebhookEventType.PaymentEvent:
                        common.DisplayItem((paymentEvent)eventObject);
                        break;

                    case WebhookEventType.PaymentProfileEvent:
                        common.DisplayItem((paymentProfileEvent)eventObject);
                        break;

                    case WebhookEventType.SubscriptionEvent:
                        common.DisplayItem((subscriptionEvent)eventObject);
                        break;

                    default:
                        //Debug.WriteLine ("Object type not found");
                        break;

                }

                Debug.WriteLine("");
            }

        }


        [TestMethod]
        public void Evalulate_SpecificObjects()
        {
            // This test will process the web hooks and return the specific object:customerEvent, fraudEvent, paymentEcent, paymentProfileEvent, or subscriptionEvent


            // get the event data
            var events = common.EventData();

            // loop through and evaluate the event data
            var evt = new Event(signatureKey);

            foreach (var item in events)
            {
                var eventName = item.Item1;
                var X_Anet_Signature = item.Item2;
                var Raw_JSON_Body = item.Item3;

                Debug.WriteLine("Evaluating Event:");
                common.DebugLine("eventName", eventName);
                common.DebugLine("X_Anet_Signature", X_Anet_Signature);                

                // get the type of event
                var nameEventType = evt.GetEventType(eventName);
                switch (nameEventType)
                {
                    case WebhookEventType.CustomerEvent:
                        common.DisplayItem(evt.Evaluate<customerEvent>(Raw_JSON_Body, X_Anet_Signature));
                        break;

                    case WebhookEventType.FraudEvent:
                        common.DisplayItem(evt.Evaluate<fraudEvent>(Raw_JSON_Body, X_Anet_Signature));
                        break;

                    case WebhookEventType.PaymentEvent:
                        common.DisplayItem(evt.Evaluate<paymentEvent>(Raw_JSON_Body, X_Anet_Signature));
                        break;

                    case WebhookEventType.PaymentProfileEvent:
                        common.DisplayItem(evt.Evaluate<paymentEvent>(Raw_JSON_Body, X_Anet_Signature));
                        break;

                    case WebhookEventType.SubscriptionEvent:
                        common.DisplayItem(evt.Evaluate<subscriptionEvent>(Raw_JSON_Body, X_Anet_Signature));
                        break;


                    default:
                        Debug.WriteLine("Object type not found");
                        break;


                }
            }



        }


    }
}
