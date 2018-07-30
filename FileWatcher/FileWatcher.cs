using SmtpSender.Infrastructure;
using System;
using System.ComponentModel;
using System.IO;
using NLog;

namespace FileWatcher
{
    /// <summary>
    /// Класс для мониторинга директории.
    /// </summary>
    public partial class FileWatcher : IFileWatcher
    {
        [Description("Просматриваемая директория.")]
        public string Directory { get; private set; }

        [Description("Объект для отправки Email.")]
        public ISmtpSender SmtpSender { get; private set; }

        [Description("Объект логирования")]
        public ILogger Logger { get; private set; }

        /// <param name="directory">Просматриваемая директория.</param>
        /// <param name="smtpSender">Объект для отправки Email.</param>
        /// <param name="logger">Объект для логирования.</param>
        public FileWatcher(string directory, ISmtpSender smtpSender, ILogger logger)
        {
            Directory = directory;
            SmtpSender = smtpSender;
            Logger = logger;
        }

        /// <summary>
        /// Запускает мониторинг директории.
        /// </summary>
        public void Run()
        {
            try
            {
                Logger.Info("Запуск наблюдения за директорией...");            
                if (SmtpSender == null )
                {
                    Logger.Error("Пустое значение: SmtpSender");
                    throw new ArgumentNullException(message: "Пустое значение: SmtpSender", paramName : "SmtpSender");
                }

                if (Directory == null)
                {
                    Logger.Error("Пустое значение: директории");
                    throw new ArgumentNullException(message: "Пустое значение: директории", paramName: "Directory");
                }

                FileSystemWatcher _watcher = new FileSystemWatcher();
                _watcher.Path = this.Directory;
                _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite| NotifyFilters.FileName | NotifyFilters.DirectoryName;
                _watcher.Filter = "*.txt";
                _watcher.Created += new FileSystemEventHandler(OnCreated);
                _watcher.EnableRaisingEvents = true;
            Console.WriteLine("Press q to close app.");
            }
            catch (ArgumentNullException e)
            {
                Logger.Error(e.Message);
                Logger.Trace(e.StackTrace);
                throw;
            }
            catch (ArgumentException ex)
            {
                Logger.Error("Ошибка пути файла." + ex.Message);
                Logger.Trace(ex.StackTrace);
                throw;
            }

            while (Console.Read() != 'q');
        }
    }
}
