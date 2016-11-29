using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace UrlStatus
{
    public static class Resource
    {
        // Task: http://www.cnblogs.com/pengstone/archive/2012/12/23/2830238.html

        public const string Http_Protocol = "http://";
        public const string Https_Protocol = "https://";
        public const string Free_Protocol = "//";

        public const string OpenFileDialog_Title = "Open file";
        public const string OpenFileDialog_Filter = "Plain Text|*.txt|All Files|*.*";

        /// <summary>
        /// from microsoft: https://msdn.microsoft.com/en-us/library/ff650303.aspx#paght000001_commonregularexpressions
        /// </summary>
        public const string UrlRegex1 = @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#=_\|\:]*)?";

        /// <summary>
        /// from http://regexr.com/
        /// </summary>
        public const string UrlRegex2 = @"\w+:\/\/[\w@][\w.:@]+\/?[\w\.?=%&=\-@/$,]*";

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


    public enum SourceType
    {
        File,
        URL
    }

    delegate void SetTextCallback(string text);

    /// <summary>
    /// The common response object of a http(s).
    /// </summary>
    public class RequestResult
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// The thread result object.
    /// </summary>
    public class OutPutList
    {
        public int HttpTotalCount { get; set; }
        public int HttpsTotalCount { get; set; }
        public List<RequestResult> Output_OK { get; set; }
        public List<RequestResult> OutPut_Error { get; set; }
        public int Http_Ok_Count { get { return Output_OK.Count(x => x.Url.StartsWith(Resource.Http_Protocol)); } }
        public int Https_Ok_Count { get { return Output_OK.Count(x => x.Url.StartsWith(Resource.Https_Protocol)); } }
        public int Http_Error_Count { get { return OutPut_Error.Count(x => x.Url.StartsWith(Resource.Http_Protocol)); } }
        public int Https_Error_Count { get { return OutPut_Error.Count(x => x.Url.StartsWith(Resource.Https_Protocol)); } }
        public string GetReport
        {
            get
            {
                string report = "Report:\r\n\tType\tTotal\tOk\tError\r\n\thttp\t{0}\t{1}\t{2}\r\n\thttps\t{3}\t{4}\t{5}";

                return string.Format(report
                    , Http_Ok_Count + Http_Error_Count, Http_Ok_Count, Http_Error_Count
                    , Https_Ok_Count + Https_Error_Count, Https_Ok_Count, Https_Error_Count
                    );
            }
        }

        public string GetErrorUrls
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (OutPut_Error.Count > 0)
                {
                    sb.Append("Error Urls:");
                    sb.Append(Environment.NewLine);
                    for (int i = 0; i < OutPut_Error.Count; i++)
                    {
                        var result = OutPut_Error[i];
                        sb.Append(string.Format("\t{0}: {1}{2}\t    Error:{3}{2}", i + 1, result.Url, Environment.NewLine, result.Message));
                    }
                }
                else
                {
                    sb.Append("No Error Urls.");
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
        }

        public string GetHttpsErrorUrls
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                var diff = new List<RequestResult>();
                var httpsErrors = OutPut_Error.Where(x => x.Url.StartsWith(Resource.Https_Protocol)).ToList();
                var httpOks = Output_OK.Where(x => x.Url.StartsWith(Resource.Http_Protocol)).ToList();

                diff = (from error in httpsErrors
                        join ok in httpOks
                        on error.Url.Remove(4, 1) equals ok.Url
                        select error).ToList();

                if (diff.Any())
                {
                    sb.Append(string.Format("Need to check {0} Urls:(http is ok, but https is error.)", diff.Count));
                    sb.Append(Environment.NewLine);
                    for (int i = 0; i < diff.Count; i++)
                    {
                        var result = diff[i];
                        sb.Append(string.Format("\t{0}: {1}{2}\t    Error: {3}{2}{2}", i + 1, result.Url, Environment.NewLine, result.Message));
                    }
                }
                else
                {
                    sb.Append("All http urls is ok under the https protocol.");
                }
                return sb.ToString();
            }
        }

        public string GetStatus
        {
            get
            {
                string status = string.Format("Status: http{0}/{1}, https{2}/{3}.",
                Output_OK.Count(a => a.Url.StartsWith(Resource.Http_Protocol)) + OutPut_Error.Count(a => a.Url.StartsWith(Resource.Http_Protocol)),
                HttpTotalCount,
                Output_OK.Count(a => a.Url.StartsWith(Resource.Https_Protocol)) + OutPut_Error.Count(a => a.Url.StartsWith(Resource.Https_Protocol)),
                HttpsTotalCount);
                return status;
            }
        }
    }

    public static class StringExtension
    {
        public static bool IsUrl(this string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return (url.ToLower().StartsWith(Resource.Http_Protocol)
                    || url.ToLower().StartsWith(Resource.Https_Protocol)
                    || url.ToLower().StartsWith(Resource.Free_Protocol));
            }

            //Uri uriResult;
            //return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
            //    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return false;
        }
    }
}
