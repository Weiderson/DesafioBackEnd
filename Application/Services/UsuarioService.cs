using System.Security.Cryptography;
using System.Text;

namespace DesafioBackendAPI.Application.Services
{
    public static class UsuarioService
    {
        public static string GerarHash(string senha)
        {
            using (var hmac = new HMACSHA256())
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));

                return Convert.ToBase64String(hash);
            }
        }

        public static string GerarSalt(string senha)
        {
            using (var hmac = new HMACSHA256())
            {
                var salt = hmac.Key;
                return Convert.ToBase64String(salt);
            }
        }
    }
}
