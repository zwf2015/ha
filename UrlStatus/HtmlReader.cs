using System;
using System.IO;
using System.Net;
using System.Text;
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
        /// Do request of <paramref name="url"/>.
        /// </summary>
        /// <param name="url">url</param>
        /// <returns><see cref="RequestResult"/></returns>
        public static RequestResult DoRequest(string url)
        {
            WebRequest req = null;
            HttpWebResponse res = null;
            RequestResult result = new RequestResult();
            result.Url = url;
            try
            {
                req = WebRequest.Create(url);
                req.Timeout = 1000 * 10;
                ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };
                res = (HttpWebResponse)req.GetResponse();
                result.HttpStatusCode = res.StatusCode;
            }
            catch (Exception ex)
            {
                result.HttpStatusCode = HttpStatusCode.BadRequest;
                result.Message = ex.Message;
                string errMsg = string.Format("\tRequest of {0} is Error: {1}.", url, ex.Message);
                LogManager.Error(errMsg, ex);
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
                if (req != null)
                {
                    req.Abort();
                }
                res = null;
                req = null;
            }
            return result;
        }
    }
}
