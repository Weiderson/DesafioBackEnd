using System.Text.RegularExpressions;

namespace DesafioBackendAPI.Application.Services
{
    public static class FormataString
    {
        public static bool SomenteNumeros(string valor)
        {
            return Regex.IsMatch(valor, @"^\d+$");
        }
    }
}