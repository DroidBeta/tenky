using System;
using System.Text;
using System.Net;
using System.IO;
using DroidBeta.Tenky.Extension;

namespace DroidBeta.Tenky.Net
{
    public static class Http
    {
        private const string _defaultUA = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
        
        public static string GetStr(string url) => GetStr(url, null, null, null, null);

        public static string GetStr(string url, int timeoutSecond) => GetStr(url, timeoutSecond, null, null, null);

        public static string GetStr(string url, int? timeoutSecond, string userAgent, string referer, CookieCollection cookies)
        {
            WebResponse response = GetResponse(url, timeoutSecond, userAgent, referer, cookies);

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

        public static WebResponse GetResponse(string url) => GetResponse(url, null, null, null, null);

        public static WebResponse GetResponse(string url, int timeoutSecond) => GetResponse(url, timeoutSecond, null, null, null);

        public static WebResponse GetResponse(string url, int? timeoutSecond, string userAgent, string referer, CookieCollection cookies)
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

            return request.GetResponse();
        }

    }

    class IP
    {
        private static int _ipv;
        private static string _ip;
        IP(string ip)
        {
            _ipv = ip.GetIpFamily().Replace("ipv", "").ToInt();
            _ip = ip;
        }



    }

}