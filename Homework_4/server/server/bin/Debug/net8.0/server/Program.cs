using System;
using server;

Server server = new Server();

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