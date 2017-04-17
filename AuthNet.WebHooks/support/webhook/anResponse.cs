using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNet.WebHooks
{

    /// <summary>
    /// Common fields to all webhooks
    /// </summary>
    public abstract class anResponse
    {
        public string status { get; set; }
        public string reason { get; set; }
        public string message { get; set; }
        public string correlationId { get; set; }
        public List<details> details { get; set; }

        /// <summary>
        /// Contains the string of the last Rest Response
        /// </summary>
        public string LastResponse { get; set; }

        public bool IsError
        {
            get
            {
                // if reason and message are populated, there is an error
                return !String.IsNullOrEmpty(this.reason) || !String.IsNullOrEmpty(this.message);
            }
        }

        public ApplicationException Exception
        {
            get
            {
                ApplicationException ex = null;

                if (this.IsError)
                {
                    string detailList = string.Empty;
                    if (this.details != null && this.details.Any())
                    {
                        for (int i = 0; i < this.details.Count; i++)
                        {
                            if (detailList.Length > 0) detailList += "; ";
                            detailList += String.Format(String.Format("Details {0} = {1}", (i + 1), this.details[i].message));
                        }
                    }

                    var msg = "";
                    if (!String.IsNullOrEmpty(this.reason)) msg = this.reason;
                    if (!String.IsNullOrEmpty(this.message))
                    {
                        if (msg.Length > 0) msg += ": ";
                        msg += this.message;
                    }
                
                    ex = new ApplicationException(msg);

                    if (!String.IsNullOrEmpty(this.reason)) ex.Data.Add("reason", this.reason);
                    if (!String.IsNullOrEmpty(this.message)) ex.Data.Add("message", this.message);
                    if (!String.IsNullOrEmpty(this.status)) ex.Data.Add("status", this.status);
                    if (!String.IsNullOrEmpty(this.correlationId)) ex.Data.Add("correlationId", this.correlationId);
                    if (!String.IsNullOrEmpty(detailList)) ex.Data.Add("details", detailList);
                    if (!String.IsNullOrEmpty(this.LastResponse)) ex.Data.Add("LastResponse", LastResponse);

                }

                return ex;
            }
        }

    }
}
