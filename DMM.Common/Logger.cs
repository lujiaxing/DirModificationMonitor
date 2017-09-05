using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace DMM.Common
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger
    {
        private static Logger instance = new Logger();
        private ILog infoLogger, errorLogger;

        /// <summary>
        /// 获取日志记录器实例
        /// </summary>
        public static Logger Instance
        {
            get{return instance;}
        }

        private Logger()
        {
            ReloadLogger();
        }

        /// <summary>
        /// 重置日志记录并应用当前配置文件
        /// </summary>
        public void ReloadLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
            infoLogger = log4net.LogManager.GetLogger("loginfo");
            errorLogger = log4net.LogManager.GetLogger("logerror");
        }

        /// <summary>
        /// 写入普通消息日志
        /// </summary>
        /// <param name="message">消息内容</param>
        public void WriteInfoLog(String message)
        {
            infoLogger.Info(message);
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="message">错误描述</param>
        public void WriteErrorLog(String message)
        {
            errorLogger.Error(message);
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="exception">异常实例</param>
        public void WriteErrorLog(Exception exception)
        {
            errorLogger.Error(exception.Message + Environment.NewLine + exception.StackTrace);
        }


    }
}
