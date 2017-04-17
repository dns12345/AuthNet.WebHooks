using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{

    /// <summary>
    /// This class is a collection of EventType objects.  It contains extra methods to search for errors in the response
    /// </summary>
    public class EventTypes : List<EventType>
    {

        public bool AnyError()
        {
            return this.Count(p => p.IsError == true) > 0;
        }

        public EventType FirstError()
        {            
            return this.FirstOrDefault(p => p.IsError == true);
        }

        public List<EventType> AllErrors()
        {
            var errorList = this.Where(p => p.IsError == true);
            if (errorList == null) return new List<EventType>();   // no errors
            return errorList.ToList();
        }


    }
}
