using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{

    /// <summary>
    /// Applies to:
    /// net.authorize.payment.authorization.created:    Notifies you when an authorization transaction is created.
    /// net.authorize.payment.authcapture.created:      Notifies you when an authorization and capture transaction is created.
    /// net.authorize.payment.capture.created:          Notifies you when a capture transaction is created.
    /// net.authorize.payment.refund.created:           Notifies you when a successfully settled transaction is refunded.
    /// net.authorize.payment.priorAuthCapture.created: Notifies you when a previous authorization is captured.
    /// net.authorize.payment.void.created:             Notifies you when an unsettled transaction is voided.
    /// </summary>
    public class paymentEvent : eventResponse
    {
        public payloadPayment payload { get; set; }
    }
}
    