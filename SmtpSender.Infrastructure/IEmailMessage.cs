using System.ComponentModel;

namespace SmtpSender.Infrastructure
{
    public interface IEmailMessage
    {     
        [Description("Текст сообщения.")]
        string Message { get;}

        [Description("Заголовок сообщения.")]
        string Header { get;  }

        [Description("Email с которого быдет происходить оправка.")]
        string EmailFrom { get; }

        [Description("Пароль для почты с которой будет происходить отправка.")]
        string Password { get;  }

        [Description("Имя отправителя.")]
        string NameFrom { get; }

        [Description("Email кому будет отправленно.")]
        string EmailTo { get; }

    }
}
