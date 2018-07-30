using System.ComponentModel;
using NLog;

namespace SmtpSender.Infrastructure
{    /// <summary>
     /// Общий интерфейс логгера
     /// </summary>
     public abstract class AbstractLogger: ILoggerControl
     {
        [Description("Объект используемый для логирования")]
        private Logger _loger { get; set; }

        /// <summary>
        /// Логируем сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>void</returns>
        public abstract void Log(string message);

        /// <summary>
        /// Получаем текущий объект логгера
        /// </summary>
        /// <returns>Logger</returns>
        public Logger GetLogger()
        {
            return _loger;
        }

        /// <summary>
        /// Устанавлеваем текущий объект логгера
        /// </summary>
        /// <param name="logger">объект используемый для логирования</param>
        /// <returns>void</returns>
        public void SetLogger(Logger logger)
        {
            this._loger = logger;
        }

    }
}
