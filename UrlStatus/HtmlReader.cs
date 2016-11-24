using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UrlStatus
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
                resstream.ReadTimeout = 10 * 1000;
                StreamReader sr = new StreamReader(resstream, encodetype);
                StringBuilder sb = new StringBuilder();
                sb.Append(sr.ReadToEnd());
                sr.Dispose();

                //去除非必要空格
                return Regex.Replace(sb.ToString(), "\\s{2,}", " ");
            }
        }

        /// <summary>
        /// Regex urls from text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> GetUrls(string text)
        {
            var urls = new List<string>();
            if (!string.IsNullOrWhiteSpace(text))
            {
                Regex rex = new Regex(Resource.UrlRegex1, RegexOptions.IgnoreCase);
                MatchCollection mc = rex.Matches(text);
                foreach (Match m in mc)
                {
                    urls.Add(m.Value);
                }
                return urls.Where(a => StringExtension.IsUrl(a)).Distinct().ToList();
            }
            return urls;
        }
    }
}
