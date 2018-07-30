using NLog;

namespace SmtpSender.Infrastructure
{
    interface ILoggerControl
    {
        /// <summary>
        /// Получаем текущий объект логгера
        /// </summary>
        /// <returns>Logger</returns>
        Logger GetLogger();

        /// <summary>
        /// Устанавлеваем текущий объект логгера
        /// </summary>
        /// <param name="logger">объект используемый для логирования</param>
        /// <returns>void</returns>
        void SetLogger(Logger logger);
    }
}
