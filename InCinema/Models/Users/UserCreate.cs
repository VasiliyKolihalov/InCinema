using System.ComponentModel.DataAnnotations;
using InCinema.Attributes;

namespace InCinema.Models.Users;

public class UserCreate
{
    [Required] [MaxLength(50)] public string FirstName { get; set; }
    [Required] [MaxLength(50)] public string LastName { get; set; }
    [EmailAddress] [MaxLength(100)] public string Email { get; set; }
    [Required] [Password] public string Password { get; set; }
}