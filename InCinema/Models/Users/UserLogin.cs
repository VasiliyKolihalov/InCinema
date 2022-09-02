using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Users;

public class UserLogin
{
    [EmailAddress] [MaxLength(100)] public string Email { get; set; }
    [Required] [MaxLength(8)] public string Password { get; set; }
}