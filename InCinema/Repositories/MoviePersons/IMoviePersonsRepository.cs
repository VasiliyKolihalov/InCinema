using InCinema.Models.MoviePersons;

namespace InCinema.Repositories.MoviePersons;

public interface IMoviePersonsRepository : IRepository<MoviePerson, int>
{
    public IEnumerable<MoviePerson> GetActorsByMovieId(int movieId);
    public void AddToMoviesActors(int moviePersonId, int movieId);
    public void DeleteFromMoviesActors(int moviePersonId, int movieId);

}