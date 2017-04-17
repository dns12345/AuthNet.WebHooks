using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{
    public class WebHooks : List<WebHook>
    {
        public bool AnyError()
        {
            return this.Count(p => p.IsError == true) > 0;
        }

        public WebHook FirstError()
        {
            return this.FirstOrDefault(p => p.IsError == true);
        }

        public List<WebHook> AllErrors()
        {
            var errorList = this.Where(p => p.IsError == true);
            if (errorList == null) return new List<WebHook>();   // no errors
            return errorList.ToList();
        }

    }
}
