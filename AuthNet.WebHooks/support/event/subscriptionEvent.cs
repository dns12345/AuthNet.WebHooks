using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    /// <summary>
    /// net.authorize.customer.subscription.created:    Notifies you that a subscription has been created.
    /// net.authorize.customer.subscription.updated:    Notifies you that a subscription has been updated.
    /// net.authorize.customer.subscription.suspended:  Notifies you that a subscription has been suspended.
    /// net.authorize.customer.subscription.terminated: Notifies you that a subscription has been terminated.
    /// net.authorize.customer.subscription.cancelled:  Notifies you that a subscription has been cancelled.
    /// net.authorize.customer.subscription.expiring:   Notifies you when a subscription has only one recurrence left to be charged.
    /// </summary>
    public class subscriptionEvent: eventResponse
    {
        public subscriptionPayload payload { get; set; }

    }
}
