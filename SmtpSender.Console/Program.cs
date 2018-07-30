using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SmtpSender.Infrastructure;
using NLog;

namespace SmtpSender.Console
{
    class Program
    {
       public static ILogger logger { get; private set; }
       static void Main(string[] args)
       {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            logger = NLog.LogManager.GetCurrentClassLogger();
           
            string _emailFrom = ConfigurationManager.AppSettings.Get("emailFrom");
            string _nameFrom = ConfigurationManager.AppSettings.Get("nameFrom");
            string _password = ConfigurationManager.AppSettings.Get("password");
            string _emailTo = ConfigurationManager.AppSettings.Get("emailTo");
            string _directory = ConfigurationManager.AppSettings.Get("directory");
            string _header = ConfigurationManager.AppSettings.Get("header");
            string _pathToMessage = ConfigurationManager.AppSettings.Get("pathToMessage");
            string _message = "";

            using (StreamReader sr=new StreamReader(_pathToMessage))
            {
                _message = sr.ReadToEnd();
            }

            ISmtpSender smtpSender =new SmtpSender(_emailFrom, _password, _nameFrom , _emailTo, _header,_message, logger); 
            IFileWatcher fileWatcher =new FileWatcher.FileWatcher(_directory, smtpSender, logger);
            fileWatcher.Run();
        }
    }
}
