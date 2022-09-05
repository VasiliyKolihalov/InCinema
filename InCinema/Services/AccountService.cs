using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Roles;
using InCinema.Models.Users;
using InCinema.Repositories;
using BCryptNet = BCrypt.Net.BCrypt;

namespace InCinema.Services;

public class AccountService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public AccountService(IApplicationContext applicationContext, IMapper mapper, IJwtService jwtService)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public string Register(UserCreate userCreate)
    {
        User? user = _applicationContext.Users.GetByEmail(userCreate.Email);
        if (user != null)
            throw new BadRequestException("User with this email already exist");

        User newUser = _mapper.Map<User>(userCreate);
        newUser.PasswordHash = BCryptNet.HashPassword(userCreate.Password);

        _applicationContext.Users.Add(newUser);

        return _jwtService.GenerateJwt(newUser);
    }

    public string Login(UserLogin userLogin)
    {
        User? user = _applicationContext.Users.GetByEmail(userLogin.Email);
        if (user == null)
            throw new BadRequestException("User with this email does not exist");

        if (!BCryptNet.Verify(userLogin.Password, user.PasswordHash))
            throw new BadRequestException("Incorrect login or password");

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(user.Id);

        return _jwtService.GenerateJwt(user, roles);
    }
}