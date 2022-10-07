using System.ComponentModel.DataAnnotations;

namespace InCinema.Attributes;

public class PasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string password)
            throw new ValidationException("PasswordAttribute meant for strings");

        var isValid = true;
        
        if (password.Length < 8 || password.Length > 16)
        {
            ErrorMessage = "Password length must be between 8 and 16 characters";
            isValid = false;
        }

        if (!password.Any(char.IsUpper))
        {
            ErrorMessage = "Password must contain at least 1 capital letter";
            isValid = false;
        }

        if (!password.Any(char.IsLower))
        {
            ErrorMessage = "Password must contain at least 1 uppercase letter";
            isValid = false;
        }

        if (password.Contains(' '))
        {
            ErrorMessage = "Password must not include spaces";
            isValid = false;
        }
        
        string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        if (!specialCharacters.Any(password.Contains))
        {
            ErrorMessage = "Password must contain special characters";
            isValid = false;
        }

        return isValid;
    }
}