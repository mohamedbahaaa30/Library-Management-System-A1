using Library_Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Management_System.Services.Implemetation
{
    public class TokenService : ITokenService
    {
        public readonly IConfiguration _config;
        public TokenService (IConfiguration configuration)
        {
            this._config = configuration;
        }

        public async Task<string> CreateToken(ApplicationUser user)
        {
            //claims 
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            //key 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //token 
            var token = new JwtSecurityToken
            (
                claims: Claims,
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]!))
            );

            return new JwtSecurityTokenHandler().WriteToken(token);  
        }
    }
}
