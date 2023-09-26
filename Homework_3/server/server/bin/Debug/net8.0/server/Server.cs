using System;
using System.IO;
using System.Net;
using System.Text;

public class Server
{
    private HttpListener listener;
    private string baseDirectory;

    public Server(string baseUrl, string staticDirectory)
    {
        listener = new HttpListener();
        listener.Prefixes.Add(baseUrl);
        baseDirectory = staticDirectory;
        
        if (!Directory.Exists(baseDirectory))
        {
            Directory.CreateDirectory(baseDirectory);
        }
    }
    
    public void Start()
    {
        listener.Start();
        Console.WriteLine("Сервер запущен. Ожидание запросов...");

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Console.WriteLine($"Получен запрос: {request.Url}");

            string filePath;
            
            if (!request.Url.LocalPath.Contains('.'))
            {
                filePath = Path.Combine(baseDirectory, request.Url.LocalPath.TrimStart('/'), "index.html");
            }
            else
            {
                filePath = Path.Combine(baseDirectory, request.Url.LocalPath.TrimStart('/'));
            }
            
            if (File.Exists(filePath))
            {
                var pageContents = File.ReadAllBytes(filePath);
                response.ContentLength64 = pageContents.Length;
                response.ContentType = GetContentType(filePath);
                response.OutputStream.Write(pageContents, 0, pageContents.Length);
                response.Close();
            }
            else
            {
                string Str = "HTTP/1.1 " + "404 Error";
                byte[] Buffer = Encoding.ASCII.GetBytes(Str);
                response.ContentLength64 = Buffer.Length;
                response.OutputStream.Write(Buffer,0, Buffer.Length);
                response.Close();
            }
        }
    }

    private string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();

        switch (extension)
        {
            case ".htm":
            case ".html":
                return "text/html";
            
            case ".css":
                return "text/css";
            
            case ".js":
                return "text/javascript";
            
            case ".jpg":
                return "image/jpeg";
            
            case ".jpeg":
            case ".png":
            case ".gif":
                return "image/" + extension.Substring(1);
            
            case ".svg":
                return "image/svg+xml";
            
            default:
                if (extension.Length > 1)
                    return "application/" + extension.Substring(1);
                else
                    return "application/unknown";
        }
    }
    
    public void Stop()
    {
        listener.Stop();
        listener.Close();
    }
}
