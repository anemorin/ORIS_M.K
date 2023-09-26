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
    public string staticDirectory { get; set; }
}