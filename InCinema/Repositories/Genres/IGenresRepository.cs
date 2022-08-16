using InCinema.Models.Genres;

namespace InCinema.Repositories.Genres;

public interface IGenresRepository : IRepository<Genre, int>
{
    public Genre? GetByName(string genreName);
    
    public IEnumerable<Genre> GetByMovie(int movieId);
    public void AddToMovie(int genreId, int movieId);
    public void DeleteFromMovies(int genreId, int movieId);
}