using System;
using System.Windows.Forms;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace UrlStatus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // LoadFileAppender();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogUnhandledException(e.ExceptionObject);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogUnhandledException(e.Exception);
        }

        static void LogUnhandledException(object exceptionobj)
        {
            LogManager.WriteLog("程序异常终止:" + exceptionobj);
        }

        /// <summary>
        /// 初始化日志文件配置
        /// </summary>
        static void LoadFileAppender()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;

            log4net.Repository.Hierarchy.Hierarchy hier =
                log4net.LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;

            RollingFileAppender fileAppender = new RollingFileAppender();
            fileAppender.Name = "RollingFileAppender";
            fileAppender.File = "logs/log-";
            fileAppender.AppendToFile = true;
            fileAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            fileAppender.LockingModel = new log4net.Appender.RollingFileAppender.MinimalLock();
            fileAppender.MaxSizeRollBackups = -1;
            fileAppender.MaximumFileSize = "2MB";
            fileAppender.DatePattern = "'APP-'yyyyMMdd'.log'";
            fileAppender.StaticLogFileName = false;
            fileAppender.Encoding = System.Text.Encoding.UTF8;

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level%n %message%n %exception%n";
            patternLayout.ActivateOptions();

            fileAppender.Layout = patternLayout;

            fileAppender.ActivateOptions();

            BasicConfigurator.Configure(fileAppender);
        }
    }
}
