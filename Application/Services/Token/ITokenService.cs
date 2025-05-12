using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Application.Services.Token
{
    public interface ITokenService
    {
        string CriarToken(Usuario usuario);
    }
}
