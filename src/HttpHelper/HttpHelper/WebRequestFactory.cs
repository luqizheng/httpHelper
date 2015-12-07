using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HttpHelper
{
    public static class WebRequestFactory
    {
        public static void AcceptCer()
        {
            ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
        }

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        public static HttpWebRequest Create(string url, string method)
        {
            var result = WebRequest.CreateHttp(url);
            result.Method = method;
            result.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            result.Date = DateTime.Now;
            return result;
        }
    }
}