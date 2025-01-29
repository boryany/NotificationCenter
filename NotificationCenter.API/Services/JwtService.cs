using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly byte[] _key;
    private readonly string _secretKey;

    public JwtService(IConfiguration configuration)
    {
        if (configuration != null) 
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            _secretKey = configuration["JwtSettings:SecretKey"];
#pragma warning restore CS8601 // Possible null reference assignment.
            if (_secretKey != null)_key = Encoding.ASCII.GetBytes(_secretKey);
            else throw new InvalidOperationException("JWT Secret key is not configured.");
        }
        else throw new InvalidOperationException("JWT Secret key is not configured.");
    }

    public string GenerateToken(string clientId, string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("clientId", clientId),
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
