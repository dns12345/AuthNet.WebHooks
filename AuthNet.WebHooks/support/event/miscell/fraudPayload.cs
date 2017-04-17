using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class fraudPayload
    {
        public string responseCode { get; set; }
        public string authCode { get; set; }
        public string avsResponse { get; set; }
        public string authAmount { get; set; }

        public List<fraudList> fraudList { get; set; }

    }
}
