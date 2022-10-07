using AutoMapper;
using InCinema.Constants;
using InCinema.Exceptions;
using InCinema.Models.Roles;
using InCinema.Models.Users;
using InCinema.Repositories;

namespace InCinema.Services;

public class UsersService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public UsersService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<UserPreview> GetAll()
    {
        IEnumerable<User> users = _applicationContext.Users.GetAll();
        return _mapper.Map<IEnumerable<UserPreview>>(users);
    }

    public UserView GetById(int userId)
    {
        User user = _applicationContext.Users.GetById(userId);
        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);

        var userView = _mapper.Map<UserView>(user);
        userView.Roles = _mapper.Map<IEnumerable<RoleView>>(roles);

        return userView;
    }

    public UserPreview Update(UserUpdate userUpdateModel, int userId)
    {
        _applicationContext.Users.GetById(userUpdateModel.Id);
        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);

        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && userUpdateModel.Id != userId)
            throw new ForbiddenException("User does not have enough rights for this action");

        var updateUser = _mapper.Map<User>(userUpdateModel);
        _applicationContext.Users.Update(updateUser);

        return _mapper.Map<UserPreview>(updateUser);
    }

    public UserPreview Delete(int userToDeleteId, int currentRequestUserId)
    {
        User user = _applicationContext.Users.GetById(userToDeleteId);
        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(currentRequestUserId);

        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && userToDeleteId != currentRequestUserId)
            throw new ForbiddenException("User does not have enough rights for this action");

        _applicationContext.Users.Delete(userToDeleteId);

        return _mapper.Map<UserPreview>(user);
    }

    public UserPreview AddRole(int userId, int roleId)
    {
        User user = _applicationContext.Users.GetById(userId);
        _applicationContext.Roles.GetById(roleId);

        IEnumerable<Role> userRoles = _applicationContext.Roles.GetByUserId(userId);
        if (userRoles.Any(x => x.Id == roleId))
            throw new BadRequestException("User already have this role");

        _applicationContext.Roles.AddToUser(roleId, userId);

        return _mapper.Map<UserPreview>(user);
    }

    public UserPreview DeleteRole(int userId, int roleId)
    {
        User user = _applicationContext.Users.GetById(userId);
        _applicationContext.Roles.GetById(roleId);

        IEnumerable<Role> userRoles = _applicationContext.Roles.GetByUserId(userId);
        if (userRoles.All(x => x.Id != roleId))
            throw new BadRequestException("User does not have this role");

        _applicationContext.Roles.DeleteFromUser(roleId, userId);

        return _mapper.Map<UserPreview>(user);
    }
}