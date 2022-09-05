using InCinema.Models.Roles;
using InCinema.Models.Users;

namespace InCinema.Services;

public interface IJwtService
{
    public string GenerateJwt(User user, IEnumerable<Role>? roles = null);
}