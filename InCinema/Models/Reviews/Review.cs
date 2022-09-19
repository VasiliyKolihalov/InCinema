using InCinema.Models.Users;

namespace InCinema.Models.Reviews;

public class Review
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int MovieScore { get; set; }
    public string? Text { get; set; }
    public DateTime DateTime { get; set; }
    public User Author { get; set; }
}