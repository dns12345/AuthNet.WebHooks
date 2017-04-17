using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AuthNet.WebHooks.Tests
{
    [TestClass]
    public class WebHook_Tests
    {
        // default to testing in the sandbox
        private bool sandbox = true;

        // Your API Login ID and Transaction Key are unique pieces of information specifically associated with your payment gateway account. 
        // However, the API login ID and Transaction Key are NOT used for logging into the Merchant Interface.  
        // Available from the Authorize.net Admin Portal in Settings | Security Settings | General Security Settings | API Credentials & Keys
        private string apiLoginID = "<< Enter your apiLoginID >>";
        private string apiTransactionKey = "<< Enter your apiTransactionKey >>";

        // The URL where Authorize.Net WebHooks will post notifications.
        // For testing purposes, create a Free endpoint at http://requestb.in
        private string notifyUrl = "<< Enter your notify URL >>";


        private WebHook InitWebHook()
        {
            return new WebHook(apiLoginID, apiTransactionKey, sandbox);
        }


        /// <summary>
        /// Get a single webhook id from your collection of WebHooks from your Autorize.Net account
        /// </summary>
        /// <returns></returns>
        private string GetWebhookId()
        {
            string webHookId = String.Empty;

            var _webhook = InitWebHook();
            var webhooks = _webhook.List();

            if (webhooks != null && webhooks.Any() && !webhooks.AnyError())
            {
                webHookId = webhooks[0].webhookId;
            }

            return webHookId;

        }


        /// <summary>
        /// Load the first webhook from your collection of webhooks
        /// </summary>        
        private WebHook LoadFirstWebhook()
        {
            var _webhook = InitWebHook();
            var webhooks = _webhook.List();

            if (webhooks != null && webhooks.Any() && !webhooks.AnyError())
            {
                return webhooks[0];
            }

            return null;

        }


        /// <summary>
        /// Create a new webhook with all the event types
        /// </summary>
        /// <returns>The webHookId of the newly created WebHook.</returns>
        private WebHook createWebhook()
        {
            // List all the WebHooks defined in my Authorize.Net account                       
            var webHook = InitWebHook();

            // get the list of all event Types.  We will create the webhook with all event types
            var eventTypes = webHook.ListEventTypes();

            // create the new web hook
            webHook.name = String.Format("WebHooks {0}", DateTime.Now.ToString("yyyyMMddhhmmss"));
            webHook.url = notifyUrl;
            webHook.status = "active";
            webHook.eventTypes = new List<string>();
            foreach (var item in eventTypes) webHook.eventTypes.Add(item.name);

            // save the webhook
            webHook.Save();
            return webHook;

        }


        /// <summary>
        /// Create a new webhook with all the event types
        /// </summary>
        /// <returns>The webHookId of the newly created WebHook.</returns>
        private WebHook createWebhook(string EventType, bool active)
        {
            // List all the WebHooks defined in my Authorize.Net account                       
            var webHook = InitWebHook();

            // create the new web hook
            webHook.name = EventType.Replace('.', '_');
            webHook.url = notifyUrl;
            webHook.status = active ? "active" : "inactive";
            webHook.eventTypes = new List<string>();
            webHook.eventTypes.Add(EventType);

            // save the webhook
            webHook.Save();
            return webHook;

        }


        [TestMethod]        
        public void ListEventTypes()
        {                        
            // List all the event types from Authorize.Net that you could subscribe to in a web hook
            var webHook = InitWebHook();            
            EventTypes eventTypes = null;
            try
            {
                eventTypes = webHook.ListEventTypes();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            if (eventTypes != null && eventTypes.Any())
            {
                // see if we got any errors back from Authorize.Net that were not exceptions
                if (eventTypes.AnyError())
                {
                    foreach (var eventTypeEx in eventTypes.AllErrors()) common.DisplayError(eventTypeEx.Exception);
                }
                else
                {
                    // no errors, we are good:  Display the eventTypes and set the test as successful.
                    common.DisplayItems(eventTypes);
                    Assert.IsTrue(true);
                }                                
            }
            else
            {
                // We should have multiple event types
                Assert.Fail("Expecting multiple Event Types: Returned null/empty");
            }

            common.DisplayResponse(webHook);
        }


        [TestMethod]       
        public void ListMyWebHooks()
        {
            // List all the WebHooks defined in my Authorize.Net account                       
            var webHook = InitWebHook();

            WebHooks webhooks = null;
            try
            {
                webhooks = webHook.List();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            if (webhooks != null && webhooks.Any())
            {
                // see if we got any errors back from Authorize.Net that were not exceptions
                if (webhooks.AnyError())
                {
                    foreach (var webHookEx in webhooks.AllErrors()) common.DisplayError(webHookEx.Exception);
                }
                else
                {
                    // no errors, we are good:  Display the eventTypes and set the test as successful.
                    common.DisplayItems(webhooks);
                    Assert.IsTrue(true);
                }
            }
            else
            {
                Debug.WriteLine("No webhooks found in account.");
            }

            common.DisplayResponse(webHook);
        }


        [TestMethod]
        public void CreateWebhook()
        {
            WebHook webhook = null;
            try
            {
                webhook = createWebhook();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webhook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            if (webhook == null)
            {
                // failed to create the webhook
                Assert.Fail("Faild to create WebHook.  Webhook object is null.  Please see the output window for more information.");                
            }
            else if (webhook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webhook.Exception);
            }
            else
            {
                common.DisplayItem(webhook);
            }

            Assert.IsNotNull(webhook);
            Assert.IsFalse(webhook.IsError);

            common.DisplayResponse(webhook);
        }


        [TestMethod]
        public void CreateWebhook_SpecificEvent()
        {
            WebHook webhook = null;
            try
            {
                webhook = createWebhook("net.authorize.customer.subscription.expiring", false);
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webhook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            if (webhook == null)
            {
                // failed to create the webhook
                Assert.Fail("Faild to create WebHook.  Webhook object is null.  Please see the output window for more information.");
            }
            else if (webhook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webhook.Exception);
            }
            else
            {
                common.DisplayItem(webhook);
            }

            Assert.IsNotNull(webhook);
            Assert.IsFalse(webhook.IsError);

            common.DisplayResponse(webhook);
        }


        [TestMethod]
        public void CreateWebhook_EachEventType()
        {
            // create a separate webhook for each type of events

            // List all the event types from Authorize.Net that you could subscribe to in a web hook
            var webHook = InitWebHook();
            EventTypes eventTypes = null;
            try
            {
                eventTypes = webHook.ListEventTypes();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            try
            {
                foreach (var item in eventTypes)
                {
                    // List all the WebHooks defined in my Authorize.Net account                       
                    var _webHook = InitWebHook();

                    // create the new web hook
                    _webHook.name = item.name.Replace('.', '_');
                    _webHook.url = notifyUrl;
                    //_webHook.status = "inactive";  // set them inactive so we get the "test" button in authorize.net
                    _webHook.status = "active";
                    _webHook.eventTypes = new List<string>();
                    _webHook.eventTypes.Add(item.name);

                    // save the webhook
                    _webHook.Save();
                }
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            Assert.IsTrue(true);

        }


        [TestMethod]
        public void CreateWebhook_AllEvents()
        {

            // create a single webhook that subscribes to all events

            // List all the event types from Authorize.Net that you could subscribe to in a web hook
            var webHook = InitWebHook();
            EventTypes eventTypes = null;
            try
            {
                eventTypes = webHook.ListEventTypes();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            try
            {
                // List all the WebHooks defined in my Authorize.Net account                       
                var _webHook = InitWebHook();

                // create the new web hook
                _webHook.name = "All Events";
                _webHook.url = notifyUrl;
                _webHook.status = "active";
                _webHook.eventTypes = new List<string>();
                foreach (var item in eventTypes) _webHook.eventTypes.Add(item.name);

                // save the webhook
                _webHook.Save();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            Assert.IsTrue(true);

        }


        [TestMethod]
        public void UpdateWebhook()
        {
            // load the first WebHook found in your collection of WebHooks on Authorize.Net
            var webHook = LoadFirstWebhook();
            if (webHook == null)
            {
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");                
                return;
            }
            else if (webHook.IsError)
            {
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  Please check the output window for more information.");
                return;
            }

            // update the webhook
            webHook.name = String.Format("WebHooks {0}", DateTime.Now.ToString("yyyyMMddhhmmss"));
            webHook.url = notifyUrl;
            webHook.status = webHook.status == "active" ? "inactive" : "active";  // toggle the status value
            if (webHook.eventTypes != null && webHook.eventTypes.Any())
            {
                // remove the first event type from the list;
                webHook.eventTypes.RemoveAt(0);
            }

            // save the changes; simply call Save() again
            try
            {
                webHook.Save();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            if (webHook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webHook.Exception);
            }
            else
            {
                common.DisplayItem(webHook);
            }

            Assert.IsNotNull(webHook);
            Assert.IsFalse(webHook.IsError);

            common.DisplayResponse(webHook);

        }

        [TestMethod]
        public void UpdateWebhook_InvalidName()
        {
            var webHook = LoadFirstWebhook();
            if (webHook == null)
            {
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");
                return;
            }
            else if (webHook.IsError)
            {
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  Please check the output window for more information.");
                return;
            }

            // update the webhook
            webHook.name = "!@#$%^&*()";  // invalid name

            // save the changes; simply call Save() again
            try
            {
                webHook.Save();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.IsTrue(true, "Successfully got an error while trying to update an invalid name.");
                return;
            }

            if (webHook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.IsTrue(true, "Successfully got an error while trying to update an invalid name.");                
            }
            else
            {
                common.DisplayItem(webHook);
                common.DisplayResponse(webHook);

                // this should not happen because we have an invalid name
                Assert.IsTrue(false, "Updated Webhook with an invalid name");
            }

        }

        [TestMethod]
        public void UpdateWebhook_NotExists()
        {

            var webHook = LoadFirstWebhook();
            if (webHook == null)
            {
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");
                return;
            }
            else if (webHook.IsError)
            {
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  Please check the output window for more information.");
                return;
            }

            // update the webhook
            webHook.name = String.Format("WebHooks {0}", DateTime.Now.ToString("yyyyMMddhhmmss"));

            // change the webhookId so we are trying to update a non-existing webhook
            webHook.webhookId = Guid.NewGuid().ToString();

            // save the changes; simply call Save() again
            try
            {
                webHook.Save();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.IsTrue(true, "Successfully got an error while trying to update a non-existing WebHook.");
                return;
            }

            if (webHook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.IsTrue(true, "Successfully got an error while trying to update a non-existing WebHook");
            }
            else
            {
                common.DisplayItem(webHook);
                common.DisplayResponse(webHook);

                // this should not happen because we have an invalid name
                Assert.IsTrue(false, "Updated Webhook with an invalid name");
            }


        }


        [TestMethod]
        public void UpdateWebhook_RemoveAllEventTypes()
        {
            // load the first WebHook found in your collection of WebHooks on Authorize.Net
            var webHook = LoadFirstWebhook();
            if (webHook == null)
            {
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");
                return;
            }
            else if (webHook.IsError)
            {
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  Please check the output window for more information.");
                return;
            }

            // update the webhook
            webHook.name = String.Format("WebHooks {0}", DateTime.Now.ToString("yyyyMMddhhmmss"));
            webHook.url = notifyUrl;
            webHook.status = webHook.status == "active" ? "inactive" : "active";  // toggle the status value
            if (webHook.eventTypes != null && webHook.eventTypes.Any())
            {
                // Remove all the event hooks
                webHook.eventTypes = new List<string>(); 
            }

            // save the changes; simply call Save() again
            try
            {
                webHook.Save();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.IsTrue(true, "Successfully got an error while trying to update a webhook with no eventTypes.");
                return;
            }

            if (webHook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.IsTrue(true, "Successfully got an error while trying to update a webhook with no eventTypes");
            }
            else
            {
                common.DisplayItem(webHook);
                common.DisplayResponse(webHook);
                Assert.IsTrue(false, "Updated Webhook with no eventTypes");
            }

        }


        [TestMethod]
        public void ToggleActive()
        {
            // load the first WebHook found in your collection of WebHooks on Authorize.Net
            var webHook = LoadFirstWebhook();
            if (webHook == null)
            {
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");
                return;
            }
            else if (webHook.IsError)
            {
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  Please check the output window for more information.");
                return;
            }
            
            try
            {

                webHook.SetActiveInactive(webHook.status.Equals("active", StringComparison.InvariantCultureIgnoreCase) ? false : true);
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            if (webHook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
            }
            else
            {
                common.DisplayItem(webHook);
                common.DisplayResponse(webHook);
            }

            Assert.IsNotNull(webHook);
            Assert.IsFalse(webHook.IsError);
        }


        [TestMethod]
        public void DeleteWebHook()
        {
            // load the first WebHook found in your collection of WebHooks on Authorize.Net
            var webHook = LoadFirstWebhook();
            if (webHook == null)
            {
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");
                return;
            }
            else if (webHook.IsError)
            {
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  Please check the output window for more information.");
                return;
            }

            // save the changes; simply call Save() again
            try
            {

                webHook.Delete();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            // delete thors an error if it fails; otherwise it was successful
            Assert.IsTrue(true, "Successfully deleted the webhook");

            common.DisplayResponse(webHook);
        }

        [TestMethod]
        public void DeleteWebHook_InvalidWebhook()
        {
            // load the first WebHook found in your collection of WebHooks on Authorize.Net
            var webHook = LoadFirstWebhook();
            if (webHook == null)
            {
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");
                return;
            }
            else if (webHook.IsError)
            {
                common.DisplayError(webHook.Exception);
                common.DisplayResponse(webHook);
                Assert.Fail("Failed to load a WebHook.  Please check the output window for more information.");
                return;
            }

            // save the changes; simply call Save() again
            try
            {
                // force te webhook to be invalid
                webHook.webhookId += "_INVALID";
                webHook.Delete();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.IsTrue(true, "Successfuly received an excpetion while trying to delete an invalid webhook.");
                return;
            }

            // delete thors an error if it fails; otherwise it was successful
            Assert.IsTrue(false, "No exeception while deleting a non-existing webhook....strange!");

            common.DisplayResponse(webHook);
        }


        [TestMethod]
        public void LoadWebHook()
        {
            // load the first WebHook found in your collection of WebHooks on Authorize.Net
            var webHookId = GetWebhookId();
            if (String.IsNullOrEmpty(webHookId))
            {                
                Assert.Fail("Failed to load a WebHook.  There are no WebHooks defined in your Authorize.Net account.  Please create at least one webhook before you try and load.");
                return;
            }

            // load the webhook
            var webHook = InitWebHook();
            try
            {                
                webHook.Load(webHookId);
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
                return;
            }

            if (webHook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webHook.Exception);
            }
            else
            {
                common.DisplayItem(webHook);
            }

            Assert.IsNotNull(webHook);
            Assert.IsFalse(webHook.IsError);

            common.DisplayResponse(webHook);

        }

        [TestMethod]
        public void LoadWebHook_InvalidWebhookId()
        {

            // Generate a GUID' Hopefully this web hook does not exist!
            var webhookId = Guid.NewGuid().ToString();

            // load the webhook with an invalid ID
            var webHook = InitWebHook();
            try
            {
                webHook.Load(webhookId);
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);

                // treat this as a success...we ar epecgint an error
                Assert.IsTrue(true, "We successfully got an error while try to load an invalid WebHook");
                return;
            }

            if (webHook.IsError)
            {
                // we successfully called Authorize.Net without an exception, however, the response had an error
                common.DisplayError(webHook.Exception);
                Assert.IsTrue(true, "We successfully got an error while try to load an invalid WebHook");
            }
            else
            {
                common.DisplayItem(webHook);
                Assert.IsTrue(false, String.Format("Try this method again; Apparently {0} is a valid webhookid", webhookId));
            }

            common.DisplayResponse(webHook);


        }

        [TestMethod]
        public void InvalidCredentials()
        {
            
            // List all the event types from Authorize.Net that you could subscribe to in a web hook            
            var webHook = InitWebHook();


            // ERROR TEST:  Add an Invalid APILoginId
            webHook.ApiLoginID += "_INVALID";

            EventTypes eventTypes = null;
            try
            {
                eventTypes = webHook.ListEventTypes();
            }
            catch (Exception err)
            {
                common.DisplayError(err);
                common.DisplayResponse(webHook);
                Assert.Fail("Exception from WebHook; Please see the output window for more information.");
            }

            if (eventTypes != null && eventTypes.Any())
            {
                // see if we got an error in any of the event types
                var errorEventType = eventTypes.FirstOrDefault(p => p.IsError == true);
                if (errorEventType != null)
                {
                    // we got an error in at least one of the event types
                    common.DisplayError(errorEventType.Exception);

                    // this is a success because we are expecting an error
                    Assert.IsTrue(true, "Received an error from Authorize.Net because of invalid credentials.");
                }
                else
                {
                    // no errors, we are good:  Display the eventTypes and set the test as successful.
                    common.DisplayItems(eventTypes);
                    Assert.IsFalse(false, "Expecting an error from Authorize.Net for invalid credentials.");
                }
            }
            else
            {
                Assert.Fail("Expecting multiple Event Types: Returned null/empty");
            }

            common.DisplayResponse(webHook);
        }

    }
}
