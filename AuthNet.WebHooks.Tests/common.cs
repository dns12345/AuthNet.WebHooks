using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks.Tests
{
    public class common
    {

        public static void DebugLine(string key, string value)
        {
            Debug.WriteLine(String.Format("{0} = {1}", key, value));
        }

        public static void DisplayResponse(WebHook webHook)
        {
            if (webHook == null) return;
            if (String.IsNullOrEmpty(webHook.LastResponse)) return;

            Debug.WriteLine("\n\nAuthorize.Net JSON Response:");
            Debug.WriteLine(webHook.LastResponse);
            
        }


        public static void DisplayError(Exception err)
        {
            if (err == null) return;
            Debug.WriteLine("ERROR:");
            Debug.WriteLine(err);
            if (err.Data != null)
            {
                if (err.Data != null && err.Data.Keys != null)
                {
                    foreach (var key in err.Data.Keys)
                    {                        
                        DebugLine(key.ToString(), err.Data[key].ToString());
                    }
                }
            }

            if (err.InnerException != null) DisplayError(err.InnerException);
        }


        public static void DisplayItems(EventTypes eventTypes)
        {
            if (eventTypes != null && eventTypes.Any())
                foreach (var item in eventTypes) DisplayItem(item);            
        }


        public static void DisplayItem(EventType eventType)
        {
            if (eventType != null) DebugLine("eventType.name", eventType.name);
        }


        public static void DisplayItems(WebHooks webHooks)
        {
            if (webHooks != null && webHooks.Any())
                foreach (var item in webHooks) DisplayItem(item);

        }


        public static void DisplayItem(WebHook webHook)
        {
            if (webHook != null)
            {
                DebugLine("webHook.name", webHook.name);
                DebugLine("webHook.webhookId", webHook.webhookId);
                DebugLine("webHook.url", webHook.url);
                if (webHook._links != null && webHook._links.self != null)
                    DebugLine("webHook._links.self.href", webHook._links.self.href);

                if (webHook.eventTypes != null & webHook.eventTypes.Any())
                {
                    Debug.WriteLine("Subscribed Event Type:");
                    foreach (var eventType in webHook.eventTypes)
                    {
                        Debug.WriteLine("    " + eventType);                        
                    }

                }
                Debug.WriteLine("");
            }
        }


        public static void DisplayItem(customerEvent customerEvent)
        {
            if (customerEvent != null)
            {
                Debug.WriteLine("Event Object:  customerEvent");
                DisplayItem((eventResponse)customerEvent);

                if (customerEvent.payload != null)
                {
                    DebugLine("   payload.description", customerEvent.payload.description);
                    DebugLine("   payload.entityName", customerEvent.payload.entityName);
                    DebugLine("   payload.id", customerEvent.payload.id);
                    DebugLine("   payload.merchantCustomerId", customerEvent.payload.merchantCustomerId);
                    if (customerEvent.payload.paymentProfiles != null)
                    {
                        foreach (var paymentProfile in customerEvent.payload.paymentProfiles)
                        {
                            DebugLine(String.Format("   customerEvent.payload.paymentProfiles.{0}", paymentProfile.id), String.Format("customerEvent.payload.paymentProfiles.{0}", paymentProfile.customerType));
                        }
                    }
                }


                Debug.WriteLine("Raw JSON:");
                Debug.WriteLine(customerEvent.responseBody);
                Debug.WriteLine("");
            }
        }

        public static void DisplayItem(fraudEvent fraudEvent)
        {
            if (fraudEvent != null)
            {
                Debug.WriteLine("Event Object:  fraudEvent");
                DisplayItem((eventResponse)fraudEvent);
                if (fraudEvent.payload != null)
                {
                    DebugLine("   payload.authAmount", fraudEvent.payload.authAmount);
                    DebugLine("   payload.authCode", fraudEvent.payload.authCode);
                    DebugLine("   payload.avsResponse", fraudEvent.payload.avsResponse);
                    DebugLine("   payload.responseCode", fraudEvent.payload.responseCode);
                    if (fraudEvent.payload.fraudList != null)
                    {
                        foreach (var fraudItem in fraudEvent.payload.fraudList)
                        {
                            DebugLine("   payload.fraudList (fraudAction, fraudFilter)", fraudItem.fraudAction + " - " + fraudItem.fraudFilter);
                        }
                    }
                }

                Debug.WriteLine("Raw JSON:");
                Debug.WriteLine(fraudEvent.responseBody);

                Debug.WriteLine("");
            }
        }

        public static void DisplayItem(paymentEvent paymentEvent)
        {
            if (paymentEvent != null)
            {
                Debug.WriteLine("Event Object:  paymentEvent");
                DisplayItem((eventResponse)paymentEvent);
                if (paymentEvent.payload != null)
                {
                    DebugLine("   payload.authAmount", paymentEvent.payload.authAmount);
                    DebugLine("   payload.authCode", paymentEvent.payload.authCode);
                    DebugLine("   payload.avsResponse", paymentEvent.payload.avsResponse);
                    DebugLine("   payload.entityName", paymentEvent.payload.entityName);
                    DebugLine("   payload.id", paymentEvent.payload.id);
                    DebugLine("   payload.responseCode", paymentEvent.payload.responseCode);
                }

                Debug.WriteLine("Raw JSON:");
                Debug.WriteLine(paymentEvent.responseBody);

                Debug.WriteLine("");
            }
        }

        public static void DisplayItem(paymentProfileEvent paymentProfileEvent)
        {
            if (paymentProfileEvent != null)
            {
                Debug.WriteLine("Event Object:  paymentProfileEvent");
                DisplayItem((eventResponse)paymentProfileEvent);
                if (paymentProfileEvent.payload != null)
                {
                    DebugLine("   payload.customerProfileId", paymentProfileEvent.payload.customerProfileId);
                    DebugLine("   payload.customerType", paymentProfileEvent.payload.customerType);
                    DebugLine("   payload.entityName", paymentProfileEvent.payload.entityName);
                    DebugLine("   payload.id", paymentProfileEvent.payload.id);
                }

                Debug.WriteLine("Raw JSON:");
                Debug.WriteLine(paymentProfileEvent.responseBody);

                Debug.WriteLine("");
            }
        }

        public static void DisplayItem(subscriptionEvent subscriptionEvent)
        {
            if (subscriptionEvent != null)
            {
                Debug.WriteLine("Event Object:  subscriptionEvent");
                DisplayItem((eventResponse)subscriptionEvent);
                if (subscriptionEvent.payload != null)
                {
                    DebugLine("   payload.amount", subscriptionEvent.payload.amount);
                    DebugLine("   payload.entityName", subscriptionEvent.payload.entityName);
                    DebugLine("   payload.id", subscriptionEvent.payload.id);
                    DebugLine("   payload.name", subscriptionEvent.payload.name);
                    DebugLine("   payload.status", subscriptionEvent.payload.status);
                    if (subscriptionEvent.payload.profile != null)
                    {
                        DebugLine("   payload.profile.customerPaymentProfileId", subscriptionEvent.payload.profile.customerPaymentProfileId);
                        DebugLine("   payload.profile.customerProfileId", subscriptionEvent.payload.profile.customerProfileId);
                        DebugLine("   payload.profile.customerShippingAddressId", subscriptionEvent.payload.profile.customerShippingAddressId);
                    }
                }

                Debug.WriteLine("Raw JSON:");
                Debug.WriteLine(subscriptionEvent.responseBody);

                Debug.WriteLine("");
            }
        }

        private static void DisplayItem(eventResponse eventResponse)
        {
            if (eventResponse != null)
            {                
                DebugLine("   eventDate", eventResponse.eventDate);
                DebugLine("   eventType", eventResponse.eventType);
                DebugLine("   notificationId", eventResponse.notificationId);
                DebugLine("   webhookId", eventResponse.webhookId);                                
            }
        }



        public static List<Tuple<string, string, string>> EventData()
        {

            /*
                Currently missing test/response data fr these events:
                    net.authorize.customer.subscription.expiring
                    net.authorize.customer.subscription.suspended
                    net.authorize.payment.capture.created
                    net.authorize.payment.fraud.approved
                    net.authorize.payment.fraud.declined
                    net.authorize.payment.fraud.held
                    net.authorize.payment.priorAuthCapture.created
            */
            var results = new List<Tuple<string, string, string>>();

            /*
                The tuple<> is configured as follows
                    1) the first field is the name of the event, example net.authorize.payment.authcapture.created
                    2) X-Anet-Signature (do not include the "sha512=") from the header;  https://requestb.in,
                    3) the event Raw Body/Raw Json
            */



            // This event has an invaid SHA Token
            results.Add(new Tuple<string, string, string>(
                "net.authorize.payment.authcapture.created",
                "BADDATA174451A2CF45EAA26697FE2429503FDF8DF582C34F945DB41902CE0B3F4494B878CF0B5F73606C2CC32520170AC10DB8533AFBC6DEA5ED83BA319861B75DA7B7",
                "{\"notificationId\":\"70295d62-a6fd-4fb8-b7b7-247c769baad1\",\"eventType\":\"net.authorize.payment.authcapture.created\",\"eventDate\":\"2017-04-15T21:13:43.2977159Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"responseCode\":1,\"authCode\":\"ODAY02\",\"avsResponse\":\"Y\",\"authAmount\":7.25,\"entityName\":\"transaction\",\"id\":\"60022194830\"}}"
            ));


            // This event has a valid SHA Token, but it's an unknow event
            results.Add(new Tuple<string, string, string>(
                "net.authorize.payment.invalid.event",
                "05d0894437b5b40198e443b098fdb790e6b4e33907c5e8409b3122e693091469fe2f55760a2a23cb284358ae27daaea36a3fcaac1e6beca166396bcfd3d45c52",
                "{\"notificationId\":\"70295d62-a6fd-4fb8-b7b7-247c769baad1\",\"eventType\":\"net.authorize.payment.invalid.event\",\"eventDate\":\"2017-04-15T21:13:43.2977159Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"responseCode\":1,\"authCode\":\"ODAY02\",\"avsResponse\":\"Y\",\"authAmount\":7.25,\"entityName\":\"transaction\",\"id\":\"60022194830\"}}"
            ));


            ///////////////////////////////////////////////
            //  The remainder of these events are valid
            ///////////////////////////////////////////////
            results.Add(new Tuple<string, string, string>(
                "net.authorize.payment.authcapture.created",
                "174451A2CF45EAA26697FE2429503FDF8DF582C34F945DB41902CE0B3F4494B878CF0B5F73606C2CC32520170AC10DB8533AFBC6DEA5ED83BA319861B75DA7B7",
                "{\"notificationId\":\"70295d62-a6fd-4fb8-b7b7-247c769baad1\",\"eventType\":\"net.authorize.payment.authcapture.created\",\"eventDate\":\"2017-04-15T21:13:43.2977159Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"responseCode\":1,\"authCode\":\"ODAY02\",\"avsResponse\":\"Y\",\"authAmount\":7.25,\"entityName\":\"transaction\",\"id\":\"60022194830\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.deleted",
                "0BB9379EB6F2FFC6DD53AEE5F30899303D4A49784B2268B4C93788ED7F89242E1261C390853166C9791160755910E0CB2AB3971799F0A45E25BD16EC1E12E603",
                "{\"notificationId\":\"ff8acc67-a473-4550-8e13-da30ed54ab7d\",\"eventType\":\"net.authorize.customer.deleted\",\"eventDate\":\"2017-04-15T20:39:48.68994Z\",\"webhookId\":\"5eca3570-70a4-4293-aec1-5fa7bdf0183b\",\"payload\":{\"entityName\":\"customerProfile\",\"id\":\"1811525753\"}}"
            ));
        
            results.Add(new Tuple<string, string, string>(
                "net.authorize.payment.void.created",
                "C885FB046C1A5B046A98C85971FCA7B6CBDE4F021CFA4EF4DF3E17F39401187EE165A94D42C85FAE556EFAB008806136DEE68CA7F7548E830F221F1553880BAF",
                "{\"notificationId\":\"73680caf-fa74-4934-9313-3bf853c1aa4f\",\"eventType\":\"net.authorize.payment.void.created\",\"eventDate\":\"2017-04-15T21:15:24.57954Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"responseCode\":1,\"avsResponse\":\"Y\",\"authAmount\":7.25,\"entityName\":\"transaction\",\"id\":\"60022194830\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.payment.refund.created",
                "4266AA0B2584265AD53D1E3F0257214637BD61DAB85908D8FD7832E909DE83B2D3F1B90556DAEFD0AA24DCC9A2AA54E422816F850E6479C6C943F6D4E9C2D8BB",
                "{\"notificationId\":\"33df04c4-cb5d-4598-84fc-a1517ce49a45\",\"eventType\":\"net.authorize.payment.refund.created\",\"eventDate\":\"2017-04-15T21:19:34.2043415Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"responseCode\":1,\"avsResponse\":\"P\",\"authAmount\":0.72,\"entityName\":\"transaction\",\"id\":\"60022194896\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.payment.authorization.created",
                "75A03BC0C03B0859340D8D5F5C5E2C7AB4139C412521BF78290508931CA167588C34FF1AFB1B2E2320CAFCD5D7CD99418E770870E93BA81BE75F87B00CF97336",
                "{\"notificationId\":\"8f72f0a3-414f-4772-8368-18a553570ab0\",\"eventType\":\"net.authorize.payment.authorization.created\",\"eventDate\":\"2017-04-15T21:22:24.54714Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"responseCode\":1,\"avsResponse\":\"P\",\"authAmount\":0.01,\"entityName\":\"transaction\",\"id\":\"60022194974\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.deleted",
                "0BB9379EB6F2FFC6DD53AEE5F30899303D4A49784B2268B4C93788ED7F89242E1261C390853166C9791160755910E0CB2AB3971799F0A45E25BD16EC1E12E603",
                "{\"notificationId\":\"ff8acc67-a473-4550-8e13-da30ed54ab7d\",\"eventType\":\"net.authorize.customer.deleted\",\"eventDate\":\"2017-04-15T20:39:48.68994Z\",\"webhookId\":\"5eca3570-70a4-4293-aec1-5fa7bdf0183b\",\"payload\":{\"entityName\":\"customerProfile\",\"id\":\"1811525753\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.created",
                "AE91620C0955A101A8923986663C504E5E3B030FB7A17D959F2842FFDC11678431CB676A43FA251EA2DEC2FA64ADFF40DFFF0F71F146486CAA40EC643AF83041",
                "{\"notificationId\":\"791fcad9-afa9-43fe-a04d-8b19cabb6063\",\"eventType\":\"net.authorize.customer.created\",\"eventDate\":\"2017-04-15T21:22:25.0900277Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"paymentProfiles\":[{\"customerType\":\"individual\",\"id\":\"1806085692\"},{\"customerType\":\"individual\",\"id\":\"1806085691\"}],\"merchantCustomerId\":\"SUB_001\",\"description\":\"Subscription Test 001\",\"entityName\":\"customerProfile\",\"id\":\"1811525852\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.paymentProfile.created",
                "FC874C15CF3503135BCB09C634B4AEF847ECB6D2B11FA431FF6A5AB3B364BE4CDEDA2245BFCC2E2EC6470E33944391883342BD7B7E9B12EED007285CA0CB2BC2",
                "{\"notificationId\":\"855eab46-55b8-40a9-babf-90e6c8b3d93d\",\"eventType\":\"net.authorize.customer.paymentProfile.created\",\"eventDate\":\"2017-04-15T21:22:25.0900277Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"customerProfileId\":1811525852,\"customerType\":\"individual\",\"entityName\":\"customerPaymentProfile\",\"id\":\"1806085692\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.paymentProfile.deleted",
                "BCA235CBA2A831E9FC7DCFC888F1B969AB6C8AF0203641A1B7DEF3349FCE6AE0CE4C1824B04E2707DD540F66D8E9441D701CDE5EF936920D133FE8D423B4A99E",
                "{\"notificationId\":\"3f9ad3cf-875f-48de-a6d5-0aaffb00abea\",\"eventType\":\"net.authorize.customer.paymentProfile.deleted\",\"eventDate\":\"2017-04-15T21:27:32.4003374Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"customerProfileId\":1811525852,\"entityName\":\"customerPaymentProfile\",\"id\":\"1806085691\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.updated",
                "C13F10CD6BA824167FBBECB06C50AA9D806CDB1D0C2476DF11E52066AD116FCC81C47585C00DE8244C8E8CCF8FB290ACF5AD654E030BD62D14FBC0DFB8346C55",
                "{\"notificationId\":\"4783a185-36f3-46a4-a154-fe43cbfa61e1\",\"eventType\":\"net.authorize.customer.updated\",\"eventDate\":\"2017-04-15T21:34:57.48114Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"paymentProfiles\":[{\"customerType\":\"individual\",\"id\":\"1806085692\"}],\"merchantCustomerId\":\"SUB_001\",\"description\":\"Subscription Test 001EDIT\",\"entityName\":\"customerProfile\",\"id\":\"1811525852\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.updated",
                "4B06E96166440B739DDA8A3881A497CDC0D1084B91706E0AE7762A1B31836D044DDA5093650C59D78D49F9A25B5B8C0434A5C6407E8C37C039C76CAD4EC0CDD1",
                "{\"notificationId\":\"0d2f83a7-bb85-42d7-89df-4e30e45a85b8\",\"eventType\":\"net.authorize.customer.updated\",\"eventDate\":\"2017-04-15T21:43:41.11074Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"paymentProfiles\":[{\"id\":\"1806025108\"},{\"customerType\":\"individual\",\"id\":\"1806025107\"}],\"merchantCustomerId\":\"SUB_001\",\"description\":\"Subscription Test 001\",\"entityName\":\"customerProfile\",\"id\":\"1811467160\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.paymentProfile.updated",
                "7FF1CF5BCEEB44482556CA786AB03A8C81CD7946C080094D4C668224DD6CD0E61A569360E3B13A8AE7C8306A9AB3886560AB1696DBC1E33646D2358EB116F1A1",
                "{\"notificationId\":\"2727107e-6732-4346-9cc9-1ef1dec4d787\",\"eventType\":\"net.authorize.customer.paymentProfile.updated\",\"eventDate\":\"2017-04-15T21:38:15.77274Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"customerProfileId\":1811525852,\"customerType\":\"individual\",\"entityName\":\"customerPaymentProfile\",\"id\":\"1806085692\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.subscription.created",
                "F49D3C596A61DCAB74EC7A6504165A672E64AEE33C7E4A21796EAE29EB246EBD67A77E73C17C430BAD9661CC8E1DAAF9247330671954D99668864097BAC37397",
                "{\"notificationId\":\"c20328a0-396a-40bd-bc56-ac93ee061792\",\"eventType\":\"net.authorize.customer.subscription.created\",\"eventDate\":\"2017-04-15T21:41:45.62394Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"name\":\"Subscription Test 005\",\"amount\":11.55,\"status\":\"active\",\"profile\":{\"customerProfileId\":1811467160,\"customerPaymentProfileId\":1806025108,\"customerShippingAddressId\":1810248201},\"entityName\":\"subscription\",\"id\":\"4415473\"}}"
            ));

            results.Add(new Tuple<string, string, string>(
                "net.authorize.customer.subscription.cancelled",
                "9641C9F77B9A6EC0DBC6C040A6CD38643E43031C22716B738D760D0BAC6DF73BCF191CD77BF7D51AAE5F8BA32DC07C3BE10D78BB82ED3507BA645F3A78DB313A",
                "{\"notificationId\":\"237db31e-2ac5-48ba-86fa-cc2d90a0b626\",\"eventType\":\"net.authorize.customer.subscription.cancelled\",\"eventDate\":\"2017-04-15T21:45:15.02274Z\",\"webhookId\":\"dd9e42e4-605f-47ae-ba05-990300bd77e0\",\"payload\":{\"name\":\"Subscription Test 005\",\"amount\":12.71,\"status\":\"canceled\",\"profile\":{\"customerProfileId\":1811467160,\"customerPaymentProfileId\":1806025108,\"customerShippingAddressId\":1810248201},\"entityName\":\"subscription\",\"id\":\"4415473\"}}"
            ));


            return results;

        }

    }
}
