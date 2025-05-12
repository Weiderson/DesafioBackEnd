using DesafioBackendAPI.Application.Enums;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using SwaggerIgnoreAttribute = Swashbuckle.AspNetCore.Annotations.SwaggerIgnoreAttribute;

namespace DesafioBackendAPI.Application.DTOs
{
    public class ContaDTO
    {
        //[SwaggerSchema(ReadOnly = true)]
        //[JsonIgnore]
        //[SwaggerIgnore]
        public int? Id { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [SwaggerIgnore]
        public string? NomeCompleto { get; set; }

        public string Cnpj { get; set; }

        public string Numero { get; set; }

        public string Agencia { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Relacionamento Relacionamento { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Situacao Situacao { get; set; } = new Situacao();

        [SwaggerIgnore]
        public DateTime DtInclusao { get; set; }

        [SwaggerIgnore]
        public DateTime? DtExclusao { get; set; }

        [SwaggerIgnore]
        public DateTime? DtAlteracao { get; set; }

        [JsonIgnore]
        public IFormFile Arquivo { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [SwaggerIgnore]
        public string? LocalArquivo { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [SwaggerIgnore]
        [Precision(18, 2)]
        public decimal Saldo { get; set; }
    }
}