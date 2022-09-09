namespace InCinema.Models.Configurations;

public class EmailOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool IsNeedSsl { get; set; }
}