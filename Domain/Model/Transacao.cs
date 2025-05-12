using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioBackendAPI.Domain.Model
{
    [Table("Transacoes")]
    public class Transacao
    {
        [Key]
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public string Tipo { get; set; }

        [Precision(18, 2)]
        public decimal Valor { get; set; }

        public DateTime DtInclusao { get; set; }

        [SwaggerIgnore]
        public DateTime? DtExclusao { get; set; }

        public int ContaId { get; set; }

        public Conta Conta { get; set; }

        public int? ContaId_Destino { get; set; }

    }
}