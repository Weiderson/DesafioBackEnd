using DesafioBackendAPI.Application.Enums;
using System.Text.Json.Serialization;

namespace DesafioBackendAPI.Application.DTOs
{
    public class ExtratoDTO
    {
        public int ContaId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExtratoPeriodo ExtratoPeriodo { get; set; }

    }
}