using System;
using System.Text;
using System.Net;
using System.IO;
using DroidBeta.Tenky.Extension;

namespace DroidBeta.Tenky.Net
{
    public static class Http
    {

        #region ProxySettings
        private static IWebProxy WebProxy;

        public static void SetWebProxy(IWebProxy webProxy)
        {
            if (webProxy != null)
                Http.WebProxy = webProxy;
        }

        public static void UnsetWebProxy()
        {
            Http.WebProxy = null;
        }
        #endregion

        private const string _defaultUA = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

        #region GetStr Linkers
        public static string GetStr(string url, bool IsUseProxy = true) => GetResponse(url, null, null, null, null, IsUseProxy).GetStreamString();

        public static string GetStr(string url, int timeoutSecond, bool IsUseProxy = true) => GetResponse(url, timeoutSecond, null, null, null, IsUseProxy).GetStreamString();
        
        #endregion

        public static string GetStr(string url, int? timeoutSecond, string userAgent, string referer, CookieCollection cookies, bool IsUseProxy = true)
        {
            WebResponse response = GetResponse(url, timeoutSecond, userAgent, referer, cookies, IsUseProxy);

            return response.GetStreamString();
        }

        #region GetResponse Linkers
        public static WebResponse GetResponse(string url, bool IsUseProxy = true) => GetResponse(url, null, null, null, null, IsUseProxy);

        public static WebResponse GetResponse(string url, int timeoutSecond, bool IsUseProxy = true) => GetResponse(url, timeoutSecond, null, null, null, IsUseProxy);
        #endregion

        public static WebResponse GetResponse(string url, int? timeoutSecond, string userAgent, string referer, CookieCollection cookies, bool IsUseProxy = true)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException(url + " is not a valid url.");
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.UserAgent = _defaultUA;
                request.Timeout = 5000;
                request.ReadWriteTimeout = 5000;

                if (timeoutSecond.HasValue)
                    request.ReadWriteTimeout = request.Timeout = timeoutSecond.Value * 1000;
                if (userAgent != null)
                    request.UserAgent = userAgent;
                if (referer != null)
                    request.Referer = referer;
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }

                if (!IsUseProxy)
                    request.Proxy = null;

                return request.GetResponse();
            }
            catch
            {
                return null;
            }
        }

        private const string _defaultContentType = "application/x-www-form-urlencoded";

        #region PostResponse Linkers
        public static WebResponse PostResponse(string url, string postContent, ICredentials credentials = null, bool IsUseProxy = true) => PostResponse(url, postContent, _defaultContentType, null,  null, null, credentials, IsUseProxy);

        public static WebResponse PostResponse(string url, string postContent, string contentType, bool IsUseProxy = true) => PostResponse(url, postContent, contentType, null, null, null, null, IsUseProxy);

        public static WebResponse PostResponse(string url, string postContent, string contentType, ICredentials credentials = null, bool IsUseProxy = true) => PostResponse(url, postContent, contentType, null, null, null, credentials, IsUseProxy);

        public static WebResponse PostResponse(string url, string postContent, string contentType, int timeoutSecond = 5, bool IsUseProxy = true) => PostResponse(url, postContent, contentType, timeoutSecond, null, null, null, IsUseProxy);

        public static WebResponse PostResponse(string url, string postContent, string contentType, CookieCollection cookies, bool IsUseProxy = true) => PostResponse(url, postContent, contentType, null, null, cookies, null, IsUseProxy);

        #endregion

        public static WebResponse PostResponse(string url, string postContent, string contentType, int? timeoutSecond, string userAgent, CookieCollection cookies, ICredentials credentials, bool IsUseProxy = true)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException(url + " is not a valid url.");


            try
            {
                ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentType = _defaultContentType;
                request.Proxy = Http.WebProxy;
                request.Timeout = 5000;
                request.ReadWriteTimeout = 5000;
                if (timeoutSecond.HasValue)
                    request.ReadWriteTimeout = request.Timeout = timeoutSecond.Value * 1000;
                if (userAgent != null)
                    request.UserAgent = userAgent;
                if (contentType != null)
                    request.ContentType = contentType;
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                if (!IsUseProxy)
                    request.Proxy = null;
                if (credentials != null)
                    request.Credentials = credentials;

                var requestStream = request.GetRequestStream();
                var streamWriter = new StreamWriter(requestStream, Encoding.GetEncoding("utf-8"));
                streamWriter.Write(postContent);
                streamWriter.Flush();
                return request.GetResponse();

            }
            catch
            {
                return null;
            }

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