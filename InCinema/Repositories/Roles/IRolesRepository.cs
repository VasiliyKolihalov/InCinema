using InCinema.Models.Roles;

namespace InCinema.Repositories.Roles;

public interface IRolesRepository : IRepository<Role, int>
{
    public Role? GetByName(string roleName);
    public IEnumerable<Role> GetByUserId(int userId);
    public void AddToUser(int roleId, int userId);
    public void DeleteFromUser(int roleId, int userId);
}