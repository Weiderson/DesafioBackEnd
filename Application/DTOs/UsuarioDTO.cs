using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace DesafioBackendAPI.Application.DTOs
{
    public class UsuarioDTO
    {

        [SwaggerIgnore]
        public int? Id { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string Nome { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [SwaggerRequestBody]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [MaxLength(20, ErrorMessage = "A senha deve ter no máximo 20 caracteres.")]
        public string Senha { get; set; }

        [SwaggerIgnore]
        [DataType(DataType.DateTime)]
        public DateTime DtInclusao { get; set; }

        [SwaggerIgnore]
        [DataType(DataType.DateTime)]
        public DateTime? DtExclusao { get; set; }

        [SwaggerIgnore]
        [DataType(DataType.DateTime)]
        public DateTime? DtAlteracao { get; set; }

    }
}
