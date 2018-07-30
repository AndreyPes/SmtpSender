using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Ninject;
using SmtpSender.Infrastructure;
using NLog;

namespace SmtpSender.Test
{
    [TestFixture]
    public class SmtpTestSender
    {
        [Test]
        public void InjectorTest()
        {
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            string password = "123456";
            string nameFrom = "Andrey";
            string emailFrom = "test@example.com";
            string emailTo = "andrey321@gmail.com";
            string header = "Hi!";
            string message = "This was a yours message...";
            string directory = "C:\\TestFolder";
            var kernel = new Ninject.StandardKernel();
            kernel.Bind<ISmtpSender>().To<SmtpSender>().Named("SmtpClient")
                .WithConstructorArgument("emailFrom", emailFrom)
                .WithConstructorArgument("password", password)
                .WithConstructorArgument("nameFrom", nameFrom)
                .WithConstructorArgument("emailTo", emailTo)
                .WithConstructorArgument("header", header)
                .WithConstructorArgument("message", message)
                .WithConstructorArgument("logger", mockNlog.Object);
            kernel.Bind<IFileWatcher>().To<FileWatcher.FileWatcher>().Named("FileWatcher").WithConstructorArgument("directory", directory)
                .WithConstructorArgument("smtpSender", kernel.Get<ISmtpSender>()).WithConstructorArgument("logger", mockNlog.Object);
            var fileWatcher = kernel.Get<IFileWatcher>("FileWatcher");
            Assert.NotNull(fileWatcher);
            var smtpSender = kernel.Get<ISmtpSender>("SmtpClient");
            Assert.NotNull(smtpSender);

        }

        [Test]
        public void MoqPathFileWatcherException()
        {
            Mock<ISmtpSender> mockMailClient = new Mock<ISmtpSender>();
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            FileWatcher.FileWatcher fileWatcher = new FileWatcher.FileWatcher("",mockMailClient.Object, mockNlog.Object);
            var ex = Assert.Throws<ArgumentException>(() => fileWatcher.Run());
            Assert.That(ex.Message,Is.EqualTo("Путь имеет недопустимую форму."));
        }

        [Test]
        public void CheckFileWatcherIsPathExist()
        {
            Mock<ISmtpSender> mockMailClient = new Mock<ISmtpSender>();
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            string expected = "D:\\1.txt";
            FileWatcher.FileWatcher fileWatcher = new FileWatcher.FileWatcher(expected, mockMailClient.Object, mockNlog.Object);          
            Assert.AreEqual(expected, fileWatcher.Directory);  
        }

        [Test]
        public void SmtpConstructorParamException()
        {
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            ISmtpSender mailClient = new SmtpSender("", "", "", "", "", "", mockNlog.Object);      
            var ex = Assert.ThrowsAsync<System.ArgumentException>(async () => await mailClient.SendEmailAsync("C:\\MailFolder\\d"));
            Assert.That(ex.Message, Is.EqualTo("Параметр 'address' не может быть пустой строкой.\r\nИмя параметра: address"));
        }

        [Test]
        public void SmtpSendMethodParamException2()
        {
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            ISmtpSender mailClient = new SmtpSender("example@gmail.com", "str", "str", "andrey321@gmail.com", "str", "str", mockNlog.Object);
            var ex = Assert.ThrowsAsync<System.ArgumentException>(async() => await mailClient.SendEmailAsync(""));
            Assert.That(ex.Message, Is.EqualTo("Параметр 'fileName' не может быть пустой строкой.\r\nИмя параметра: fileName"));

        }
        [Test]
        public void SmtpSendMethodParamException3()
        {
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            ISmtpSender mailClient = new SmtpSender("example@gmail.com", "str", "str", "andrey321@gmail.com", "str", "str", mockNlog.Object);
            var ex = Assert.ThrowsAsync<System.ArgumentNullException>(async () => await mailClient.SendEmailAsync(null));
            Assert.That(ex.Message, Is.EqualTo("Значение не может быть неопределенным.\r\nИмя параметра: fileName"));

        }

        [Test]
        public void FileWatcherMethodParamException()
        {
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            Mock<ISmtpSender> smtpSender=new Mock<ISmtpSender>();
            var fileWatcher=new FileWatcher.FileWatcher(null, smtpSender.Object, mockNlog.Object);
            var ex = Assert.Throws(typeof(ArgumentNullException), () => fileWatcher.Run());
            Assert.That(ex.Message, Is.EqualTo("Пустое значение: директории\r\nИмя параметра: Directory"));
            fileWatcher = new FileWatcher.FileWatcher("C:\\MailFolder", null, mockNlog.Object);
            ex = Assert.Throws(typeof(ArgumentNullException), () => fileWatcher.Run());
            Assert.That(ex.Message, Is.EqualTo("Пустое значение: SmtpSender\r\nИмя параметра: SmtpSender"));
        }

        [Test]
        public void IsCallingMethodInfoLogger()
        {
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            mockNlog.Object.Info("Test message");
            mockNlog.Verify(x=>x.Info(It.IsAny<string>()),Times.Once);
        }

        [Test]
        public void IsCallingMethodInfoErrorLogger1()
        {
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            string password = "123456";
            string nameFrom = "Andrey";
            string emailFrom = "test@example.com";
            string emailTo = "andrey321@gmail.com";
            string header = "Hi!";
            string message = "This was a yours message...";
            var kernel = new Ninject.StandardKernel();
            kernel.Bind<ISmtpSender>().To<SmtpSender>().Named("SmtpClient")
                .WithConstructorArgument("emailFrom", emailFrom)
                .WithConstructorArgument("password", password)
                .WithConstructorArgument("nameFrom", nameFrom)
                .WithConstructorArgument("emailTo", emailTo)
                .WithConstructorArgument("header", header)
                .WithConstructorArgument("message", message)
                .WithConstructorArgument("logger", mockNlog.Object);
            kernel.Get<ISmtpSender>().SendEmailAsync("");
            mockNlog.Verify(x => x.Info(It.IsAny<string>()), Times.Exactly(3));
            mockNlog.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(1));
            kernel.Get<ISmtpSender>().SendEmailAsync("C:\\MailFolder\\d.txt");
            mockNlog.Verify(x => x.Info(It.IsAny<string>()), Times.Exactly(7));
            mockNlog.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(1));
        }



        [Test]
        public void IsCallingMethodInfoErrorLogger2()
        {
            Mock<ISmtpSender> mockSmtpSender = new Mock<ISmtpSender>();
            Mock<ILogger> mockNlog = new Mock<ILogger>();
            string password = "123456";
            string nameFrom = "Andrey";
            string emailFrom = "test@example.com";
            string emailTo = "andrey321@gmail.com";
            string header = "Hi!";
            string message = "This was a yours message...";
            string directory = "C:\\MailFolder";
            var kernel = new Ninject.StandardKernel();
            var s = mockSmtpSender;
            kernel.Bind<IFileWatcher>().To<FileWatcher.FileWatcher>().Named("FileWatcher").WithConstructorArgument("directory", directory)
                .WithConstructorArgument("smtpSender", s.Object).WithConstructorArgument("logger", LogManager.GetCurrentClassLogger());
            new TaskFactory().StartNew(kernel.Get<IFileWatcher>().Run);                        
            Thread.Sleep(1000);
            if (File.Exists((directory + "\\r.txt")))
                File.Delete((directory + "\\r.txt"));
            using (File.Create(directory + "\\r.txt"))
            Thread.Sleep(1000);
            s.Verify(x => x.SendEmailAsync(It.IsAny<string>()), Times.Exactly(1));     
        }
    }
}
