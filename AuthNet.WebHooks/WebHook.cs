using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class WebHook : anResponse
    {
        #region Documentation

        #region Usage

        /*
         * HTTP GET: https://apitest.authorize.net/rest/v1/webhooks
         * Returns a list of the user's current WebHooks
         * 
         * HTTP POST: https://apitest.authorize.net/rest/v1/webhooks
         * Create new Webhooks
         * 
         * HTTP GET: https://apitest.authorize.net/rest/v1/webhooks/43f10763-b47d-4c7a-b62a-1bd7284bf5f2
         * Get a specific Webhook
        */
        #endregion

        #region Sample Response
        /*
         * Sample Reponse 1
            [
              {
                "_links": {
                  "self": {
                    "href": "/rest/v1/webhooks/2a4b707c-01b7-4918-b02a-8c53c9e3884a"
                  }
                },
                "webhookId": "2a4b707c-01b7-4918-b02a-8c53c9e3884a",
                "name": "WebHooks",
                "status": "active",
                "url": "http://requestb.in/11dw4o51",
                "eventTypes": [
                  "net.authorize.customer.subscription.updated",
                  "net.authorize.customer.subscription.created"
                ]
              }
            ]         

            Sample Response 2 (Invalid)
            {
              "status": 400,
              "reason": "INVALID_DATA",
              "message": "Field validation errors",
              "correlationId": "37a46b89-d08a-4163-8b45-3379fb75a842",
              "details": [
                {
                  "message": "Invalid name entered. Strings containing only letters, numbers, or underscores are supported."
                }
              ]
            }
        */
        #endregion

        #endregion

        #region Private Variables
        private string baseUrl;
        private string apiLoginID;
        private string apiTransactionKey;
        private bool sandbox;        
        #endregion

        #region Construtors

        /// <summary>
        /// Initiate a new Webook object;  Defaults to Sandbox = true
        /// </summary>
        public WebHook():
            this(string.Empty, string.Empty, true)
        {
        }

        /// <summary>
        /// Initiate a new Webook object
        /// </summary>
        /// <param name="ApiLoginID">Your API Login ID and Transaction Key are unique pieces of information specifically associated with your payment gateway account. However, the API login ID and Transaction Key are NOT used for logging into the Merchant Interface.  Available from the Authorize.net Admin Portal in Settings | Security Settings | General Security Settings | API Credentials & Keys</param>
        /// <param name="ApiTransactionKey">Your API Login ID and Transaction Key are unique pieces of information specifically associated with your payment gateway account. However, the API login ID and Transaction Key are NOT used for logging into the Merchant Interface.  Available from the Authorize.net Admin Portal in Settings | Security Settings | General Security Settings | API Credentials & Keys</param>
        /// <param name="Sandbox">True/False indicating if you are accessing the Authorize.Net Sandbox (https://apitest.authorize.net/rest/v1) or Production (https://api.authorize.net/rest/v1)</param>        
        public WebHook(string ApiLoginID, string ApiTransactionKey, bool Sandbox)
        {
            this.ApiLoginID = ApiLoginID;
            this.ApiTransactionKey = ApiTransactionKey;
            this.SandBox = Sandbox;

        }
        #endregion

        #region Properties

        public string BaseUrl { get { return baseUrl; } }
        public string ApiLoginID
        {
            get { return apiLoginID; }
            set { apiLoginID = value; }
        }
        public string ApiTransactionKey
        {
            get { return apiTransactionKey; }
            set { apiTransactionKey = value; }
        }
        public bool SandBox
        {
            get { return sandbox; }
            set
            {
                sandbox = value;

                if (sandbox)
                    baseUrl = @"https://apitest.authorize.net/rest/v1";
                else
                    baseUrl = @"https://api.authorize.net/rest/v1";

            }
        }
        public links _links { get; set; }
        public string webhookId { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public List<String> eventTypes { get; set; }

        #endregion

        #region Public Methods


        /// <summary>
        /// Returns a list of ALL Authorize.Net Webhooks event names that can be subscribed to.
        /// </summary>
        /// <returns>Collection of EventName objects, that contain the names of WebHooks that can be subscribed to.</returns>
        public EventTypes ListEventTypes()
        {
            var request = new RestRequest("eventtypes", Method.GET);
            return Execute<EventTypes>(request);
        }

        /// <summary>
        /// Returns a list of all active and inactive WebHooks in the Authorize.Net account
        /// </summary>
        /// <returns>Returns a collection of WebHooks that contain the details of each individual webhook.</returns>
        public WebHooks List()
        {
            var request = new RestRequest("webhooks", Method.GET);
            var list = Execute<WebHooks>(request);

            // add the credentials back into the webHooks so we can easily call save() again
            if (list != null && list.AnyError() == false 
                && !String.IsNullOrEmpty(this.ApiLoginID) && !String.IsNullOrEmpty(this.ApiTransactionKey))
            {
                foreach (var item in list)
                {
                    item.ApiLoginID = this.ApiLoginID;
                    item.ApiTransactionKey = this.ApiTransactionKey;
                }
            }
            return list;
        }

        /// <summary>
        /// retrieve details of an existing Webhook
        /// </summary>
        /// <param name="webHookId">The Id of the webhook to load</param>
        public void Load(string webHookId)
        {
            var request = new RestRequest("webhooks/{WebHookId}", Method.GET);
            request.AddUrlSegment("WebHookId", webHookId);

            var response = Execute<WebHook>(request);
            processWebHookResponse(response);

        }

        /// <summary>
        /// Save the webhook
        /// </summary>
        /// <returns>True/False indicating if the WebHook was successfully saved</returns>
        public bool Save()
        {
            if (String.IsNullOrEmpty(this.webhookId))
            {
                // Create a new Webhook
                return Create();
            }
            else
            {
                // Update an existing Webhook
                return Update();
            }
        }


        /// <summary>
        /// Easily set this WebHook active or inactive
        /// </summary>
        /// <param name="active">True to set the WebHook active; False to set the WebHook inactive</param>
        /// <returns>True/False indicating if setting the active/inactive status was successful</returns>
        public void SetActiveInactive(bool active)
        {
            var request = new RestRequest("webhooks/{WebHookId}", Method.PUT);
            request.AddUrlSegment("WebHookId", this.webhookId);
            request.AddHeader("Content-type", "application/json");
            request.AddParameter("status", active ? "active" : "inactive");

            var response = Execute<WebHook>(request);
            processWebHookResponse(response);

        }

        /// <summary>
        /// Deletes the current WebHook.  The webHookId property must be set
        /// </summary>
        public void Delete()
        {
            if (String.IsNullOrEmpty(this.webhookId))
            {
                this.LastResponse = "WebHookId property is null or empty;  No Webhook to delete";
                return;
            }

            var request = new RestRequest("webhooks/{WebHookId}", Method.DELETE);
            request.AddUrlSegment("WebHookId", this.webhookId);

            var response = Execute(request);

            if (response != null && response.StatusCode != System.Net.HttpStatusCode.OK)   // response code 200 is successfully deleted
            {
                // failed to delete
                throw new ApplicationException("Failed to delete the WebHook " + this.webhookId + "  Please see the LastResponse property for more information.");
            }
            else
            {
                // we successfully deleted the web hook; clear all the data
                clear();
            }

        }


        public List<String> History(DateTime StartDate, DateTime EndDate, int Page, int ItemsPerPage)
        {
            return null;
        }


        public List<String> History(string DeliveryStatus, int Page, int ItemsPerPage)
        {
            return null;
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Used to create a new web hook.
        /// </summary>
        private bool Create()
        {
            var request = new RestRequest("webhooks", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-type", "application/json");

            request.AddBody(new
            {
                url = this.url,
                eventTypes = eventTypes.ToArray(),
                status = this.status,
                name = this.name
            });

            // the response is a collection of WebHook, however, there should only be 1 WebHook in the collection
            var response = Execute<List<WebHook>>(request);
            return processWebHookResponseList(response);

        }

        /// <summary>
        /// Used to update an existing Webhook
        /// </summary>
        private bool Update()
        {
            var request = new RestRequest("webhooks/{WebHookId}", Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("WebHookId", this.webhookId);
            request.AddHeader("Content-type", "application/json");

            request.AddBody(new
            {
                url = this.url,
                eventTypes = eventTypes.ToArray(),
                status = this.status,
                name = this.name
            });

            // the response is a collection of WebHook, however, there should only be 1 WebHook in the collection
            var response = Execute<List<WebHook>>(request);
            return processWebHookResponseList(response);

        }


        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new System.Uri(BaseUrl);
            client.Authenticator = new HttpBasicAuthenticator(ApiLoginID, ApiTransactionKey);

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info and LastResponse property";
                var anetException = new ApplicationException(message, response.ErrorException);
                throw anetException;
            }

            // save the last response data
            this.LastResponse = response.Content;

            return response.Data;
        }


        private IRestResponse Execute(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = new System.Uri(BaseUrl);
            client.Authenticator = new HttpBasicAuthenticator(ApiLoginID, ApiTransactionKey);

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info and LastResponse property";
                var anetException = new ApplicationException(message, response.ErrorException);
                throw anetException;
            }

            // save the last response data
            this.LastResponse = response.Content;

            return response;
        }


        /// <summary>
        /// Clear the WebHook fields.  Do not clear the base/anReponse fields
        /// </summary>
        private void clear()
        {
            this._links = new links();
            this._links.self = new self();
            this.webhookId = string.Empty;
            this.name = string.Empty;
            this.url = string.Empty;
            this.eventTypes = new List<string>();
        }

        private void copyFrom(WebHook webHook)
        {
            clear();

            if (webHook._links != null && webHook._links.self != null)
            {
                this._links.self.href = webHook._links.self.href;
            }

            this.webhookId = webHook.webhookId;
            this.name = webHook.name;
            this.url = webHook.url;

            if (webHook.eventTypes != null && webHook.eventTypes.Any())
            {
                foreach (var item in webHook.eventTypes) this.eventTypes.Add(item);
            }
        }

        private bool processWebHookResponseList(List<WebHook> webHooks)
        {
            if (webHooks == null || !webHooks.Any())
            {
                throw new ApplicationException("Failed to get a response from Authorize.Net.  Please check the LastResponse property for more information");
            }
            else if (webHooks.Count == 1)
            {
                var _webHook = webHooks[0];
                if (_webHook.IsError)
                {
                    throw _webHook.Exception;
                }
                else
                {
                    copyFrom(_webHook);
                    return true;
                }
            }
            else
            {
                throw new ApplicationException("Invalid response recevied from Authorize.Net.  Expecting 1 WebHook in the response, but multiple WebHooks were returned.  Please check the LastResponse property for more information");
            }
        }

        private bool processWebHookResponse(WebHook webHook)
        {
            if (webHook == null)
            {
                throw new ApplicationException("Failed to get a response from Authorize.Net.  Please check the LastResponse property for more information");
            }
            else
            {                
                if (webHook.IsError)
                {
                    throw webHook.Exception;
                }
                else
                {
                    copyFrom(webHook);
                    return true;
                }
            }
        }
        #endregion

    }
}
