using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class details
    {
        public string message { get; set; }
        public override string ToString()
        {
            return message;
        }
    }
}
