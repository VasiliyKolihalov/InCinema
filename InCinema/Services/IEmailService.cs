namespace InCinema.Services;

public interface IEmailService
{
    public void Send(string email, string subject, string message);
}