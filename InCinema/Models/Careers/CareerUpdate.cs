using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Careers;

public class CareerUpdate
{
    [Required] public int Id { get; set; }
    [Required] [MaxLength(50)] public string Name { get; set; }
    [Required] [MaxLength(100)] public string Description { get; set; }
}