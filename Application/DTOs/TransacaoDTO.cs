using DesafioBackendAPI.Application.Enums;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace DesafioBackendAPI.Application.DTOs
{
    public class TransacaoDTO
    {

        [SwaggerIgnore]
        public int? Id { get; set; }

        public int ContaId { get; set; }

        public int? ContaId_Destino { get; set; }

        public string Descricao { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransacaoTipo Tipo { get; set; }

        [Precision(18, 2)]
        public decimal Valor { get; set; }

        [SwaggerIgnore]
        public DateTime DtInclusao { get; set; }

        [SwaggerIgnore]
        public DateTime? DtExclusao { get; set; }
    }
}