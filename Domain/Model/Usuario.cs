using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioBackendAPI.Domain.Model
{
    [Table("Usuarios")]
    public class Usuario
    {
        public int? Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public byte[]? SenhaHash { get; set; }

        public byte[]? SenhaSalt { get; set; }

        public DateTime DtInclusao { get; set; }

        public DateTime? DtExclusao { get; set; }

        public DateTime? DtAlteracao { get; set; }
    }
}
