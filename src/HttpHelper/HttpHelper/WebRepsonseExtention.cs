using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace HttpHelper
{
    public static class WebRepsonseExtention
    {
        public static string GetResponseString(this WebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public static FileInfo WriteToFile(this WebResponse resposne, string filepath)
        {
            var readStream = resposne.GetResponseStream();
            var Length = 256;
            var buffer = new byte[Length];
            var bytesRead = readStream.Read(buffer, 0, Length);
            var writeStream = File.OpenWrite(filepath);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
            return new FileInfo(filepath);
        }


        public static Dictionary<string, string> GetFormData(this WebResponse resposne)
        {
            var InputPattern = @"<input[^>]+?value=(\""|\')([^>]*)\1[^>]+?";
            var loginPageHtml = resposne.GetResponseString();
            var groups = Regex.Matches(loginPageHtml, InputPattern);
            var formData = new Dictionary<string, string>();

            for (var i = 0; i < groups.Count; i++)
            {
                var inputHtml = groups[i].Value;
                SetInputToDictionary(inputHtml, formData);
            }
            return formData;
        }

        private static void SetInputToDictionary(string input, Dictionary<string, string> data)
        {
            var NamePattern = @"name\s*=\s*('|"")(.*?)\1";
            var ValuePattern = @"value\s*=\s*('|"")(.*?)\1";
            var nameMatch = Regex.Match(input, NamePattern);
            if (!nameMatch.Success)
                return;
            var name = nameMatch.Groups[2].Value;

            var valueMatch = Regex.Match(input, ValuePattern);
            var value = valueMatch.Success ? valueMatch.Groups[2].Value : "";
            data.Add(name, value);
        }
    }
}