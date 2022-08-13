namespace InCinema.Models.Movies;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Budget { get; set; }
    public TimeSpan Time { get; set; }
}