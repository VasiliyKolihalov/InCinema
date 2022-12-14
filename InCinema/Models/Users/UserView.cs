using InCinema.Models.Roles;

namespace InCinema.Models.Users;

public class UserView
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public IEnumerable<RoleView> Roles { get; set; }
}