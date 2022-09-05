using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InCinema.Models;
using InCinema.Models.Roles;
using InCinema.Models.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InCinema.Services;

public class JwtService : IJwtService
{
    private readonly JwtAuthOptions _jwtAuthOptions;

    public JwtService(IOptions<JwtAuthOptions> jwtAuthOptions)
    {
        _jwtAuthOptions = jwtAuthOptions.Value;
    }
    
    public string GenerateJwt(User user, IEnumerable<Role>? roles = null)
    {
        SymmetricSecurityKey securityKey = _jwtAuthOptions.GetSymmetricSecurityKey();
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        if (roles != null)
        {
            foreach (Role role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
        }
        
        var token = new JwtSecurityToken(
            _jwtAuthOptions.Issuer,
            _jwtAuthOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtAuthOptions.TokenMinuteLifetime),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}