using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    /// <summary>
    /// Common fields to all event responses
    /// </summary>
    public class eventResponse
    {
        public string notificationId { get; set; }
	    public string eventType { get; set; }
        public string eventDate { get; set; }
        public string webhookId { get; set; }
        public string responseBody { get; set; }
    }
}
