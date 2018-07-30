using System.Threading.Tasks;

namespace SmtpSender.Infrastructure
{
    public interface ISmtpSender: IEmailMessage
    {
        /// <summary>
        /// Отправка текстового файла на указанный Email
        /// </summary>
        /// <param name="filePath">Путь к файлу отправки</param>
        /// <returns>Task</returns>
        Task SendEmailAsync(string filePath);
    }
}