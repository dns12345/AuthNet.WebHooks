using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks
{
    public class self
    {
        public string href { get; set; }
    }

    public class links
    {
        public self self { get; set; }
    }

    public class details
    {
        public string message { get; set; }
    }

    public class anResponse
    {
        public string status { get; set; }
        public string reason { get; set; }
        public string message { get; set; }
        public string correlationId { get; set; }
        public List<details> details { get; set; }

        public bool IsError
        {
            get
            {
                // if reason and message are populated, there is an error
                return !String.IsNullOrEmpty(this.reason) || !String.IsNullOrEmpty(this.message);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(String.Format(" + {0}: {1}", "IsError?", this.IsError));
            sb.AppendLine(String.Format(" + {0}: {1}", "status", this.status));
            sb.AppendLine(String.Format(" + {0}: {1}", "reason", this.reason));
            sb.AppendLine(String.Format(" + {0}: {1}", "message", this.message));
            sb.AppendLine(String.Format(" + {0}: {1}", "correlationId", this.correlationId));
            

            if (this.details != null && this.details.Any())
            {
                for (int i = 0; i < this.details.Count; i++)
                {
                    sb.AppendLine(String.Format(" + Details {0}: {1}", (i + 1), this.details[i].message));
                }
            }

            return sb.ToString();

        }

    }


    public class EventName : anResponse
    {
        #region Usage

        /*
        HTTP GET: https://apitest.authorize.net/rest/v1/eventtypes
        Retruns a complete list of possible Authorize.Net WebHooks
        */
        #endregion

        #region Sample Response 
        /*
        [
          {
            "name": "net.authorize.customer.created"
          },
          {
            "name": "net.authorize.customer.deleted"
          },
          {
            "name": "net.authorize.customer.updated"
          },
          {
            "name": "net.authorize.customer.paymentProfile.created"
          },
          {
            "name": "net.authorize.customer.paymentProfile.deleted"
          },
          {
            "name": "net.authorize.customer.paymentProfile.updated"
          },
          {
            "name": "net.authorize.customer.subscription.cancelled"
          },
          {
            "name": "net.authorize.customer.subscription.created"
          },
          {
            "name": "net.authorize.customer.subscription.expiring"
          },
          {
            "name": "net.authorize.customer.subscription.suspended"
          },
          {
            "name": "net.authorize.customer.subscription.terminated"
          },
          {
            "name": "net.authorize.customer.subscription.updated"
          },
          {
            "name": "net.authorize.payment.authcapture.created"
          },
          {
            "name": "net.authorize.payment.authorization.created"
          },
          {
            "name": "net.authorize.payment.capture.created"
          },
          {
            "name": "net.authorize.payment.fraud.approved"
          },
          {
            "name": "net.authorize.payment.fraud.declined"
          },
          {
            "name": "net.authorize.payment.fraud.held"
          },
          {
            "name": "net.authorize.payment.priorAuthCapture.created"
          },
          {
            "name": "net.authorize.payment.refund.created"
          },
          {
            "name": "net.authorize.payment.void.created"
          }
        ]         



        Sample Error Response
        {
          "status": 401,
          "reason": "Unauthorized",
          "message": "Invalid Authentication values provided",
          "correlationId": "89f243c2-dfcd-4d7b-b37c-7dc6692a7de2"
        }

        */
        #endregion

        public string name { get; set; }

        public override string ToString()
        {
            if (IsError)
                return base.ToString();
            else
                return name;
        }
    }

    public class WebHook : anResponse
    {
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

        public links _links { get; set; }
        public string webhookId { get; set; }
        public string name { get; set; }        
        public string url { get; set; }
        public List<String> eventTypes { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(this.name);
            sb.AppendLine("-----------------------------------------");
            sb.AppendLine(base.ToString());
            sb.AppendFormat(" + {0}: {1}\n", "webhookId", this.webhookId);            
            sb.AppendFormat(" + {0}: {1}\n", "url", this.url);

            if (this._links != null)
            {
                sb.AppendFormat(" + {0}: {1}\n", "_links", this._links.ToString());
            }

            if (this.eventTypes != null)
            {
                sb.AppendFormat("{0}: {1}", "eventTypes", String.Join(", ", this.eventTypes.ToArray()));
            }

            

            sb.AppendLine("");

            return sb.ToString();
        }

    }



}
