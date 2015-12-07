using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HttpHelper
{
    public static class WebRequestExtention
    {
        public static HttpWebRequest SetCookieContainer(this HttpWebRequest request, CookieContainer container)
        {
            request.CookieContainer = container;
            return request;
        }

        public static HttpWebRequest ForFormSubmit(this HttpWebRequest request)
        {
            request.ContentType = "application/x-www-form-urlencoded";
            return request;
        }
        public static HttpWebRequest ForImage(this HttpWebRequest request, string imageType)
        {
            request.ContentType = "image/" + imageType;
            return request;
        }
        public static WebResponse Post(this HttpWebRequest request, Dictionary<string, string> data)
        {
            if (String.IsNullOrEmpty(request.ContentType))
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            var stream = request.GetRequestStream();
            
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(ToPostData(data));
                writer.Flush();
            }
            return request.GetResponse();
        }

        private static string ToPostData(Dictionary<string, string> postData)
        {
            var ary = new string[postData.Count];
            var index = 0;
            foreach (var key in postData.Keys)
            {
                ary[index] = key + "=" + postData[key];
                index++;
            }
            return string.Join("&", ary);
        }
    }
}