using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace DroidBeta.Tenky.Net
{
    class Http
    {
        private const string _defaultUA = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
        public static string Get(string url, int? timeoutSecond, string userAgent, string referer, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))  
                throw new ArgumentNullException(url + " is not a valid url.");
             
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = _defaultUA;
            request.Timeout = 5;

            if(timeoutSecond.HasValue)
                request.Timeout = timeoutSecond.Value * 1000;
            if(userAgent != null)
                request.UserAgent = userAgent;
            if(referer != null)
                request.Referer = referer;
            if(cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            string responseStr = "";
            if (stream != null)
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            response.Close();
            return responseStr;
        }

        public static string Get(string url) => Get(url, null, null, null, null);

        public static string Get(string url, int timeoutSecond) => Get(url, timeoutSecond, null, null);
    }

}
