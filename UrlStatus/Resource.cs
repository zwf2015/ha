using System;

namespace UrlStatus
{
    public static class Resource
    {
        // Task: http://www.cnblogs.com/pengstone/archive/2012/12/23/2830238.html

        public const string Test_Url = "http://baidu.com";

        public const string Http_Protocol = "http://";
        public const string Https_Protocol = "https://";
        public const string Free_Protocol = "//";

        public const string OpenFileDialog_Title = "Open file";
        public const string OpenFileDialog_Filter = "Plain Text|*.txt|All Files|*.*";

        /// <summary>
        /// from microsoft: https://msdn.microsoft.com/en-us/library/ff650303.aspx#paght000001_commonregularexpressions
        /// </summary>
        public const string UrlRegex1 = @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?";

        /// <summary>
        /// from http://regexr.com/
        /// </summary>
        public const string UrlRegex2 = @"\w+:\/\/[\w@][\w.:@]+\/?[\w\.?=%&=\-@/$,]*";

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
