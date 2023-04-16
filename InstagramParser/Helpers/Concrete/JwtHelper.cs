using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InstagramParser.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InstagramParser.Helpers.Concrete;

public class JwtHelper : IJwtHelper
{
    private readonly JwtConfiguration _jwtConfiguration;

    public JwtHelper(IOptions<JwtConfiguration> jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public string GenerateToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguration.SecurityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            }),
            Audience = _jwtConfiguration.Audience,
            Issuer = _jwtConfiguration.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}