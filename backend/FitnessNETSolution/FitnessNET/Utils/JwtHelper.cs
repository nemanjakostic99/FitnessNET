using FitnessNET.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessNET.Utils
{
    public class JwtHelper
    {
        public static string GenerateToken(ClientProfile client, string secretKey, string issuer, string audience)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, client.ID.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, client.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
