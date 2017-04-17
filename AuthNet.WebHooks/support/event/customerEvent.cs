using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{

    /// <summary>
    /// net.authorize.customer.created: Notifies you that a customer profile has been created.
    /// net.authorize.customer.updated: Notifies you that a customer profile has been updated.
    /// net.authorize.customer.deleted: Notifies you that a customer profile has been deleted.All sub-profiles are deleted and the individual notifications will not be sent
    /// </summary>
    public class customerEvent : eventResponse
    {
        public payloadCustomer payload { get; set; }

    }
}
