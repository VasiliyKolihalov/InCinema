using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Users;

public class ChangeUserEmail
{
    [MaxLength(100)] public string OldEmail { get; set; }
    [EmailAddress] [MaxLength(100)] public string NewEmail { get; set; }
    [Required] public string Password { get; set; }
}