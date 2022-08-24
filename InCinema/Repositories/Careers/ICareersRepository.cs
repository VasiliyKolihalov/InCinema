using InCinema.Models.Careers;

namespace InCinema.Repositories.Careers;

public interface ICareersRepository : IRepository<Career, int>
{
    public Career? GetByName(string careerName);
    
    public IEnumerable<Career> GetByMoviePerson(int moviePersonId);
    public void AddToMoviePerson(int careerId, int moviePersonId);
    public void DeleteFromMoviePerson(int careerId, int moviePersonId);
}