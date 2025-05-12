using DesafioBackendAPI.Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DesafioBackendAPI.Application.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _Key;

        public TokenService(IConfiguration configuration)
        {
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        }


        public string CriarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            };

            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha256);

            var description = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = creds
            };

            var handker = new JwtSecurityTokenHandler();

            var token = handker.CreateToken(description);

            return handker.WriteToken(token);
        }
    }
}
