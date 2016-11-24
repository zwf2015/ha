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
