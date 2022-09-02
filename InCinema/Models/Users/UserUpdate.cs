using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Users;

public class UserUpdate
{
    [Required] public int Id { get; set; }
    [Required] [MaxLength(50)] public string FirstName { get; set; }
    [Required] [MaxLength(50)] public string LastName { get; set; }
}