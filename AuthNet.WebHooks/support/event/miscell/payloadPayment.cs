using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{

    public class payloadPayment
    {
        public string responseCode { get; set; }
        public string authCode { get; set; }
        public string avsResponse { get; set; }
        public string authAmount { get; set; }
        public string entityName { get; set; }
        public string id { get; set; }
    }
}
