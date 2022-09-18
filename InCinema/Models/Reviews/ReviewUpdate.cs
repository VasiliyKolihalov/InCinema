using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace InCinema.Models.Reviews;

public class ReviewUpdate
{
    [Required] public int Id { get; set; }
    [Required] [Min(0)] [Max(10)] public int MovieScore { get; set; }
    public string? Text { get; set; }
}