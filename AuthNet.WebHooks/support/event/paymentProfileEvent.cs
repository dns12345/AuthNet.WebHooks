using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{

    /// <summary>
    /// net.authorize.customer.paymentProfile.created: Notifies you that a payment profile has been created.
    /// net.authorize.customer.paymentProfile.updated: Notifies you that a payment profile has been updated.
    /// net.authorize.customer.paymentProfile.deleted: Notifies you that a payment profile has been deleted
    /// </summary>
    public class paymentProfileEvent : eventResponse
    {
        public payloadProfile payload { get; set; }
    }
}
