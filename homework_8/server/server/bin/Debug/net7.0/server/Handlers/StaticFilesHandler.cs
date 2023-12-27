using System.Net;
using server;

namespace HttpServer.Handlers
{
    public class StaticFilesHandler : Handler
    {
        private readonly Config _config = ServerConfiguration._config;
        private string id;
        
        private string GetUrl(HttpListenerContext context)
        {
            var url = context.Request.Url?.AbsolutePath.TrimEnd('/');
            
            if (url == "")
            {
                url = "/index.html";
            }

            url = "static" + url;

            url = string.Join('/', url.Split('/').ToHashSet());
            
            if (File.Exists(url)  || url.EndsWith("/sendmail"))
            {
                return url;
            }
            else
            {
                return "static/not_found_page.html";
            }

        }

        private async void SendResponseAsync(HttpListenerResponse response, string url, HttpListenerRequest request)
        {
            byte[] buffer;
            buffer = await File.ReadAllBytesAsync($"{url}");
            
            response.ContentType = ContentTypeManager.GetContentType(url);
            response.ContentLength64 = buffer.Length;
            await using var output = response.OutputStream;

            await output.WriteAsync(buffer);
            await output.FlushAsync();
        }

        public override void HandleRequest(HttpListenerContext context)
        {
            var url = GetUrl(context);
            if (url.Split('/').LastOrDefault()!.Contains('.'))
            {
                SendResponseAsync(context.Response, url, context.Request);
            }
            else if (Successor != null)
            {
                Successor.HandleRequest(context);
            }
        }
    }
}