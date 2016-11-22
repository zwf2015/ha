using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public static class HtmlReader
    {
        public static string GetHtmlByUrl(string urlstring)
        {
            return GetHtmlByUrl(urlstring, Encoding.UTF8);
        }

        public static string GetHtmlByUrl(string urlstring, Encoding encodetype)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlstring);

            request.Timeout = 30 * 1000;
            request.ProtocolVersion = HttpVersion.Version10;
            //request.KeepAlive = false;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return BuildResponse(response, encodetype);
            }
        }

        //根据response获取HTML的单行字符串
        private static string BuildResponse(HttpWebResponse response, Encoding encodetype)
        {
            using (Stream resstream = response.GetResponseStream())
            {
                resstream.ReadTimeout = 30 * 1000;
                StreamReader sr = new StreamReader(resstream, encodetype);
                StringBuilder sb = new StringBuilder();
                sb.Append(sr.ReadToEnd());
                sr.Dispose();

                //去除非必要空格
                return Regex.Replace(sb.ToString(), "\\s{2,}", " ");
            }
        }

        public static List<string> GetHref(string url)
        {
            var urls = new List<string>();
            if (!string.IsNullOrWhiteSpace(url))
            {
                // const string Regex_Href = "href=\".*? \"";
                const string Regex_Src = "src=\".*? \"";

            }

            return urls;
        }

        
    }
}
