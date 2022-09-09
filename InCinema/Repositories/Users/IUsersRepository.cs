using InCinema.Models.Users;

namespace InCinema.Repositories.Users;

public interface IUsersRepository : IRepository<User, int>
{
    public User? GetByEmail(string email);
    public void ChangePasswordHash(User user);
    public void ChangeEmail(User user);

    public string? GetEmailConfirmCode(int userId);
    public void ConfirmEmail(int userId);
    public void AddEmailConfirmCode(int userId, string code);
    public void DeleteEmailConfirmCode(int userId);
}