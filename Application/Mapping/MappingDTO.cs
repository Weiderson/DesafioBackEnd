using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Application.Mapping
{
    public class MappingDTO : Profile
    {
        public MappingDTO()
        {
            CreateMap<Conta, ContaDTO>()
                .ForMember(conta => conta.Relacionamento, m => m.MapFrom(contadto => contadto.Relacionamento.ToString()))
                .ForMember(conta => conta.Situacao, m => m.MapFrom(contadto => contadto.Situacao.ToString()))
                .ForMember(conta => conta.LocalArquivo, m => m.MapFrom(contadto => GetFileName(contadto.LocalArquivo)))
                .ReverseMap();

            CreateMap<Transacao, TransacaoDTO>()
                .ForMember(transacao => transacao.Tipo, m => m.MapFrom(transacaodto => transacaodto.Tipo.ToString()))
                .ReverseMap();

            CreateMap<Usuario, UsuarioDTO>()
                .ReverseMap();

            //CreateMap<Usuario, ContaDTO>()
            //.ForMember(usuario => usuario.Id, m => m.MapFrom(contaDTO => contaDTO.Id))
            //.ForMember(usuario => usuario.NomeCompleto, m => m.MapFrom(contaDTO => contaDTO.Nome))
            //.ForMember(usuario => usuario.DtInclusao, m => m.MapFrom(contaDTO => contaDTO.DtInclusao))
            //.ForMember(usuario => usuario.DtExclusao, m => m.MapFrom(contaDTO => contaDTO.DtExclusao))
            //.ForMember(usuario => usuario.DtAlteracao, m => m.MapFrom(contaDTO => contaDTO.DtAlteracao))
            //.ForAllMembers(opt => opt.Condition((contaDTO, usuario, srcMember) => srcMember != null));
        }

        private static string GetFileName(string? localArquivo)
        {
            if (string.IsNullOrEmpty(localArquivo))
            {
                return string.Empty;
            }
            return localArquivo.Split("\\").Last();
        }
    }
}