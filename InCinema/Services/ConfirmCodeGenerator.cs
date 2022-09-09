using InCinema.Models.Countries;

namespace InCinema.Services;

public class ConfirmCodeGenerator : IConfirmCodeGenerator
{
    private const int MinCode = 1000;
    private const int MaxCode = 9999;
    
    public string GenerateEmailConfirmCode()
    {
        var random = new Random();
        string code = random.Next(MinCode, MaxCode).ToString();
        return code;
    }
}