using SmtpSender.Infrastructure;
using NLog;

namespace SmtpLogger
{   /// <summary>
    /// Класс логирующий сообщение
    /// </summary>
    public class SmtpLogger:AbstractLogger
    {
        /// <param name="logger">Объект используемый для логгирования</param>
        public SmtpLogger(Logger logger)
        {
            SetLogger(logger);
        }

        /// <summary>
        /// Логируем сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>void</returns>
        public override void Log(string message)
        {
            var log = GetLogger();
            if(log.IsInfoEnabled)
                log.Info(message);
            if (log.IsErrorEnabled)
                log.Error(message);
            if (log.IsWarnEnabled)
                log.Warn(message);
            if (log.IsDebugEnabled)
                log.Debug(message);
            if (log.IsErrorEnabled)
                log.Fatal(message);
            if (log.IsTraceEnabled)
                log.Trace(message);
        }
    }
}
