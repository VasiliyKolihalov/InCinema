using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Users;

public class UserCreate
{
    [Required] [MaxLength(50)] public string FirstName { get; set; }
    [Required] [MaxLength(50)] public string LastName { get; set; }
    [EmailAddress] [MaxLength(100)] public string Email { get; set; }
    [Required] [MaxLength(8)] public string Password { get; set; }
}