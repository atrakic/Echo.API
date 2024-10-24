using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EchoApi.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    const string TOKEN_EXPIRATION_IN_MINUTES = "60";

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username)
    {
        var jwtKey = _configuration?["AppSettings:Jwt:Key"];
        var securityKey = !string.IsNullOrEmpty(jwtKey) ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) : null;
        var credentials = securityKey != null ? new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256) : null;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        int tokenExpirationInMinutes = int.Parse(_configuration?["AppSettings:Jwt:TokenExpirationInMinutes"] ?? TOKEN_EXPIRATION_IN_MINUTES);

        var token = new JwtSecurityToken(
            issuer: _configuration?["AppSettings:Jwt:Issuer"],
            audience: _configuration?["AppSettings:Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(tokenExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
