using System.ComponentModel.DataAnnotations;
using InCinema.Attributes;

namespace InCinema.Models.Users;

public class ChangeUserPassword
{
    [EmailAddress] [MaxLength(100)] public string Email { get; set; }
    [Required] public string OldPassword { get; set; }
    [Required] [Password] public string NewPassword { get; set; }
}