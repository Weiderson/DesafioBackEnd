using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioBackendAPI.Domain.Model
{
    [Table("Contas")]
    public class Conta
    {
        [Key]
        public int? Id { get; set; }

        public string Cnpj { get; set; }

        public string Numero { get; set; }

        public string Agencia { get; set; }

        public string Relacionamento { get; set; }

        public string Situacao { get; set; }

        public DateTime DtInclusao { get; set; }

        public DateTime? DtExclusao { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public string LocalArquivo { get; set; }

        public decimal Saldo { get; set; }

        [NotMapped]
        public string NomeCompleto { get; set; }

        public ICollection<Transacao> Transacoes { get; } = new List<Transacao>();
    }
}