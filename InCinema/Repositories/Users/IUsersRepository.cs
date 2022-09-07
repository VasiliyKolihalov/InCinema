using InCinema.Models.Users;

namespace InCinema.Repositories.Users;

public interface IUsersRepository : IRepository<User, int>
{
    public User? GetByEmail(string email);
    public void ChangePasswordHash(User user);
}