using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Infrastructure;

namespace DesafioBackendAPI.Application.Services
{
    public static class DataSeeder
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Contexto>();
                context.Database.EnsureCreated();

                context.Conta.Add(new Conta
                {
                    Id = 1,
                    Cnpj = "18715383000140",
                    Agencia = "0007",
                    Numero = "1012347",
                    Relacionamento = "Comum",
                    Situacao = "Ativa",
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    DtAlteracao = null,
                    Saldo = 850.00m,
                    LocalArquivo = "Armazenamento\\_334450555148041616.jpg",
                });

                context.Conta.Add(new Conta
                {
                    Id = 2,
                    Cnpj = "18715508000131",
                    Agencia = "0007",
                    Numero = "1012348",
                    Relacionamento = "Comum",
                    Situacao = "Ativa",
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    DtAlteracao = null,
                    Saldo = 700.00m,
                    LocalArquivo = "Armazenamento\\_12288081790033341.jpg",
                });

                context.Conta.Add(new Conta
                {
                    Id = 3,
                    Cnpj = "18291351000164",
                    Agencia = "0007",
                    Numero = "1012349",
                    Relacionamento = "Comum",
                    Situacao = "Inativa",
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    DtAlteracao = null,
                    Saldo = 0.00m,
                    LocalArquivo = "Armazenamento\\_12214731845245810.jpg",
                });

                context.Conta.Add(new Conta
                {
                    Id = 4,
                    Cnpj = "19876424000142",
                    Agencia = "0007",
                    Numero = "1012350",
                    Relacionamento = "Comum",
                    Situacao = "Ativa",
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    DtAlteracao = null,
                    Saldo = 500.00m,
                    LocalArquivo = "Armazenamento\\_12216961649449580.jpg",
                });

                context.Transacao.Add(new Transacao
                {
                    Id = 1,
                    Descricao = "DEPÓSITO C/C",
                    Tipo = "Deposito",
                    Valor = 1000.00m,
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    ContaId = 1,
                    ContaId_Destino = null
                });

                context.Transacao.Add(new Transacao
                {
                    Id = 2,
                    Descricao = "SAQUE C/C",
                    Tipo = "Saque",
                    Valor = 150.00m,
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    ContaId = 1,
                    ContaId_Destino = null
                });

                context.Transacao.Add(new Transacao
                {
                    Id = 3,
                    Descricao = "DEPÓSITO C/C",
                    Tipo = "Deposito",
                    Valor = 1500.00m,
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    ContaId = 2,
                    ContaId_Destino = null
                });

                context.Transacao.Add(new Transacao
                {
                    Id = 4,
                    Descricao = "SAQUE C/C",
                    Tipo = "Saque",
                    Valor = 800.00m,
                    DtInclusao = DateTime.Now,
                    DtExclusao = null,
                    ContaId = 2,
                    ContaId_Destino = null
                });

                context.Usuario.Add(new Usuario
                {
                    Id = 1,
                    Nome = "Usuario1",
                    Email = "usuario1@mail.com",
                    SenhaHash = null,
                    SenhaSalt = null,
                    DtInclusao = DateTime.Now,
                    DtAlteracao = null,
                    DtExclusao = null,
                });

                context.SaveChanges();
            }
        }
    }
}