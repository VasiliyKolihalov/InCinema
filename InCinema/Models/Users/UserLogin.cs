using System.ComponentModel.DataAnnotations;
using InCinema.Attributes;

namespace InCinema.Models.Users;

public class UserLogin
{
    [EmailAddress] [MaxLength(100)] public string Email { get; set; }
    [Required] public string Password { get; set; }
}