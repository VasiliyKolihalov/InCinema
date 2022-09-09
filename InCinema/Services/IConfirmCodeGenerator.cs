namespace InCinema.Services;

public interface IConfirmCodeGenerator
{
    public string GenerateEmailConfirmCode();
}