using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Roles;

public class RoleCreate
{
    [Required] [MaxLength(100)] public string Name { get; set; }
}