using System;
using server;


Config _config = ServerConfiguration._config;
string baseUrl = $"http://{_config.Address}:{_config.Port}/";
string staticDirectory = _config.staticDirectory;
Server server = new Server(baseUrl, staticDirectory);

try
{
    server.Start();
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при запуске сервера: {ex.Message}");
}

Console.WriteLine("Нажмите Enter для завершения...");
Console.ReadLine();

server.Stop();