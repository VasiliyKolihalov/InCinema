using AutoMapper;
using InCinema.Exceptions;
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
        return _mapper.Map<UserView>(user);
    }
    
    public UserPreview Update(UserUpdate userUpdate, int userId)
    {
        if (userUpdate.Id != userId)
            throw new NotFoundException("User not found");
        
        User updateUser = _mapper.Map<User>(userUpdate);
        _applicationContext.Users.Update(updateUser);

        return _mapper.Map<UserPreview>(updateUser);
    }

    public UserPreview Delete(int userId)
    {
        User user = _applicationContext.Users.GetById(userId);
        
        _applicationContext.Users.Delete(userId);

        return _mapper.Map<UserPreview>(user);
    }
}