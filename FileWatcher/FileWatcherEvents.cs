using SmtpSender.Infrastructure;
using System.IO;
using System.Threading.Tasks;

namespace FileWatcher
{
    public partial class FileWatcher : IFileWatcher
    {
        /// <summary>
        /// Запускается после создания файла
        /// </summary>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            Logger.Info("Обнаружен новый документ: " + e.FullPath);
            FileSystemWatcher _watcher = null;
            try
            {
                Logger.Info("Процесс отправки и удаления: " + e.FullPath);
                _watcher = (FileSystemWatcher)source;
                var _task = SmtpSender.SendEmailAsync(e.FullPath);
                Task.WaitAll(_task);
                Logger.Info("Удалении файла: " + e.FullPath);
                File.Delete(e.FullPath);
                Logger.Info("Файл удален: " + e.FullPath);
            }
            catch (FileNotFoundException ex)
            {
                Logger.Error("Ошибка при удалении файла." + ex.Message);
                Logger.Trace(ex.StackTrace);
            }
            finally
            {
                if (_watcher != null)
                {
                    _watcher.Created -= new FileSystemEventHandler(OnCreated);
                    _watcher.Created += new FileSystemEventHandler(OnCreated);
                }
            }
        }
    }
}
