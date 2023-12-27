using System.Net;
using System.Net.Mail;
using server;

namespace DefaultNamespace;

public class EmailsSenderService : IEmailSenderServis
{
	private readonly Config _config = ServerConfiguration._config;
	
	public void SendMail(string message, string mailTo)
	{
		using SmtpClient client = new SmtpClient("smtp.yandex.ru");
		client.UseDefaultCredentials = false;
		client.Credentials = new NetworkCredential(_config.Mail, _config.MailPassword);
		client.EnableSsl = true;
		client.Port = 25;

		MailMessage mailMessage = new MailMessage();
		mailMessage.From = new MailAddress("marserkarimov@stud.kpfu.ru");
		mailMessage.To.Add(mailTo);
		mailMessage.Subject = "Ты попался!";
		mailMessage.Body = message;

		client.Send(mailMessage);
	}
}