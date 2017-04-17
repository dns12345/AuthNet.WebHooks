using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class payloadProfile
    {
        public string customerProfileId { get; set; }
        public string entityName { get; set; }
        public string id { get; set; }
        public string customerType { get; set; }
    }
}
