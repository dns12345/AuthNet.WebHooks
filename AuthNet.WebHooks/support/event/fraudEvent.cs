using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    /// <summary>
    /// Fraud Management events allow partners and merchants to be notified when payment transactions are held by the merchant-configured 
    /// Advanced Fraud Detection System (AFDS) filters. Additionally, notifications will be generated when these held transactions are 
    /// approved or declined
    /// net.authorize.payment.fraud.held:       Notifies you that a transaction has been held by the AFDS system.
    /// net.authorize.payment.fraud.approved:   Notifies you that a previously held transaction has been approved.
    /// net.authorize.payment.fraud.declined:   Notifies you that a previously held transaction has been declined.
    /// </summary>
    public class fraudEvent : eventResponse
    {
        public fraudPayload payload { get; set; }
    }
}
