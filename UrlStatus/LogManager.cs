using System;
using log4net;

namespace UrlStatus
{
    public static class LogManager
    {
        private readonly static ILog log = log4net.LogManager.GetLogger(typeof(LogManager));

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logFile">日志文件名的一部分 前缀和时间中间 </param>
        /// <param name="msg">写入的内容</param>
        public static void WriteLog(string logFile, string msg)
        {
            log.Info(string.Format("type:{0}-msg:{1}", logFile, msg));
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logFile">枚举类型的日志类型作为文件名的一部分</param>
        /// <param name="msg">写入的内容</param>
        public static void WriteLog(LogFile logFile, string msg)
        {
            WriteLog(logFile.ToString(), msg);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void WriteLog(string msg, Exception ex)
        {
            log.Error(msg, ex);
        }
        /// <summary>
        /// 记录 <see cref="LogFile.Logs"/> 级别的日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg)
        {
            // WriteLog("Logs", msg);
            WriteLog(LogFile.Logs, msg);
        }

        internal static void WriteLog(LogFile logFile, string msg, Exception ex)
        {
            WriteLog(string.Format("type:{0}-msg:{1}", logFile, msg), ex);
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        Sql,
        Logs,
        Text
    }
}
