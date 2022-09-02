using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models;
using InCinema.Models.Users;
using InCinema.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using BCryptNet = BCrypt.Net.BCrypt; 

namespace InCinema.Services;

public class AccountService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;
    private readonly IOptions<JwtAuthOptions> _jwtAuthOptions;

    public AccountService(IApplicationContext applicationContext, IMapper mapper,
        IOptions<JwtAuthOptions> jwtAuthOptions)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
        _jwtAuthOptions = jwtAuthOptions;
    }

    public string Register(UserCreate userCreate)
    {
        User? user = _applicationContext.Users.GetByEmail(userCreate.Email);
        if (user != null)
            throw new BadRequestException("User with this email already exist");
        
        User newUser = _mapper.Map<User>(userCreate);
        newUser.PasswordHash = BCryptNet.HashPassword(userCreate.Password);
        
        _applicationContext.Users.Add(newUser);
        
        return GenerateJwt(newUser);
    }
    
    public string Login(UserLogin userLogin)
    {
        User? user = _applicationContext.Users.GetByEmail(userLogin.Email);
        if (user == null)
            throw new BadRequestException("User with this email does not exist");

        if (!BCryptNet.Verify(userLogin.Password, user.PasswordHash))
            throw new BadRequestException("Incorrect login or password");

        return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {
        JwtAuthOptions jwtAuthOptions = _jwtAuthOptions.Value;

        SymmetricSecurityKey securityKey = jwtAuthOptions.GetSymmetricSecurityKey();
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            jwtAuthOptions.Issuer,
            jwtAuthOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtAuthOptions.TokenMinuteLifetime),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}