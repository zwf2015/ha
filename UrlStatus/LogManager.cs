using System;
using log4net;

namespace UrlStatus
{
    public static class LogManager
    {
        private readonly static ILog log = log4net.LogManager.GetLogger(typeof(LogManager));

        public static void Info(string msg)
        {
            log.Info(msg);
        }

        public static void Error(string msg, Exception ex)
        {
            log.Error(msg, ex);
        }

    }
}
