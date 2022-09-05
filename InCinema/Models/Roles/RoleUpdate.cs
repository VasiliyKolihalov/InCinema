using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Roles;

public class RoleUpdate
{
    [Required] public int Id { get; set; }
    [Required] [MaxLength(100)] public string Name { get; set; }
}