namespace server;

public static class ServerConfiguration
{
    public static Config _config { get; }

    static ServerConfiguration()
    {
        try
        {
            using (var file = File.OpenRead(@"appsettings.json"))
            {
                _config = System.Text.Json.JsonSerializer.Deserialize<Config>(file);
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("Config not found");
            throw;
        }
    }
}

public class Config
{
    public string Address { get; set; }
    public int Port { get; set; }
    public string StaticDirectory { get; set; }
    public string Mail { get; set; }
    public string MailPassword { get; set; }
}

class Mail
{
    public Mail(string address, string pass)
    {
        Address = address;
        Password = pass;
    }
    public string Address;
    public string Password;
}