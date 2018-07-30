using System;
using System.ComponentModel;
using SmtpSender.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Net.Mail;
using NLog;

namespace SmtpSender
{
    /// <summary>
    /// Класс для отправки Email.
    /// </summary>
    public class SmtpSender : ISmtpSender
    {

        /// <param name="emailFrom">Email с которого быдет происходить оправка.</param>
        /// <param name="password">Пароль для почты с которой будет происходить отправка.</param>
        /// <param name="nameFrom">Имя отправителя.</param>
        /// <param name="emailTo">Email кому будет отправленно.</param>
        /// <param name="header">Заголовок сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="logger">Объект для логирования.</param>

        public SmtpSender(string emailFrom, string password, string nameFrom, string emailTo, string header, string message, ILogger logger)
        {
            EmailFrom = emailFrom;
            Password = password;
            NameFrom = nameFrom;
            EmailTo = emailTo;
            Message = message;
            Header = header;
            Logger = logger;
        }

        [Description("Текст сообщения.")]
        public string Message { get; private set; }

        [Description("Заголовок сообщения.")]
        public string Header { get; private set; }

        [Description("Email с которого быдет происходить оправка.")]
        public string EmailFrom { get; private set; }

        [Description("Пароль для почты с которой будет происходить отправка.")]
        public string Password { get; private set; }

        [Description("Имя отправителя.")]
        public string NameFrom { get; private set; }

        [Description("Email кому будет отправленно.")]
        public string EmailTo { get; private set; }

        [Description("Объект логирования")]
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Отправка текстового файла на указанный Email
        /// </summary>
        /// <param name="filePath">Путь к файлу отправки</param>
        /// <returns>Task</returns>
        public async Task SendEmailAsync(string filePath)
        {
            try
            {
                Logger.Info("Начало отправки...");
                Attachment _attachedFile = null;
                MailAddress _froMailAddress = null;
                MailAddress _toMailAddress = null;

                     _froMailAddress = new MailAddress(EmailFrom, NameFrom);
                     _toMailAddress = new MailAddress(EmailTo);

                Logger.Info("Формирование сообщения...");
                using (MailMessage _mailMessage = new MailMessage(_froMailAddress, _toMailAddress))
                {
                    _mailMessage.Subject = Header;
                    _mailMessage.Body = Message;
                    Logger.Info("Прикрепление файла...");
                    _attachedFile = new Attachment(filePath);
                    _mailMessage.Attachments.Add(_attachedFile);
                    try
                    {
                        Logger.Info("Отправка файла...");
                        using (SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", 587))
                        {
                            _smtpClient.Timeout = 1000000;
                            _smtpClient.Credentials = new NetworkCredential(EmailFrom, Password);
                            _smtpClient.EnableSsl = true;
                            await _smtpClient.SendMailAsync(_mailMessage);
                        }

                        _attachedFile.Dispose();
                    }
                    catch (SmtpFailedRecipientException e)
                    {
                        Logger.Error("Ошибка отправки файла:\r\n" + filePath + "\r\n" + e.Message);
                        Logger.Trace(e.StackTrace);
                    }
                    catch (SmtpException e)
                    {
                        Logger.Error("Ошибка отправки файла:\r\n" + filePath + "\r\n" + e.Message);
                        Logger.Trace(e.StackTrace);
                    }
                    catch (System.FormatException e)
                    {
                        Logger.Error("Ошибка отправки файла:\r\n" + filePath + "\r\n" + e.Message);
                        Logger.Trace(e.StackTrace);
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Logger.Error(e.Message);
                Logger.Trace(e.StackTrace);
                throw;
            }
            catch (ArgumentException e)
            {
                Logger.Error(e.Message);
                Logger.Trace(e.StackTrace);
                throw;
            }
        }
    }
}
