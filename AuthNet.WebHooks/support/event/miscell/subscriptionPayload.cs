using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class subscriptionPayload
    {
        public string entityName { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string amount { get; set; }
        public string status { get; set; }

        public customerProfile profile { get; set; }

    }
}
