using System.ComponentModel;

namespace SmtpSender.Infrastructure
{
    public interface IFileWatcher
    {
        /// <summary>
        /// Запускает мониторинг директории.
        /// </summary>
        void Run();
        [Description("Просматриваемая директория.")]
        string Directory { get; }

    } 
}
