using System;
using System.Net;

namespace GWF
{
    public class WebClient : System.Net.WebClient
    {
        public WebClient()
        {
            Timeout = 1000;
        }
        /// <summary>
        /// 1000 = 1초
        /// </summary>
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest lWebRequest = base.GetWebRequest(uri);
            lWebRequest.Timeout = Timeout;
            ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
            return lWebRequest;
        }
    }
}
