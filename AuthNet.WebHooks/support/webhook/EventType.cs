using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class EventType : anResponse
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
    }
}
