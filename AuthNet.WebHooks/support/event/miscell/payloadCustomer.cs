using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class payloadCustomer
    {
        public List<paymentProfile> paymentProfiles { get; set; }

        public string merchantCustomerId { get; set; }
        public string description { get; set; }
        public string entityName { get; set; }
        public string id { get; set; }

    }
}
