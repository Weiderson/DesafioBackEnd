using DesafioBackendAPI.Application.DTOs;

namespace DesafioBackendAPI.Application.Services
{
    /// <summary>
    /// Classe responsável por operações relacionadas a arquivos.
    /// </summary>
    public class RetornaArquivo
    {
        /// <summary>
        /// Salva um arquivo no disco, associando-o a uma conta específica.
        /// </summary>
        /// <param name="numero">Número da conta.</param>
        /// <param name="agencia">Agência da conta.</param>
        /// <param name="contaDto">Objeto DTO contendo informações da conta e o arquivo.</param>
        public static async Task SalvarArquivoDisco(string conta_agencia_numero, string local_arquivo, ContaDTO contaDto)
        {
            var apagar_arquivos = Directory.GetFiles("Armazenamento", "conta_agencia_numero*");
            foreach (var file in apagar_arquivos)
            {
                System.IO.File.Delete(file);
            }
            using (Stream fileStream = new FileStream(local_arquivo, FileMode.Create))
            {
                await contaDto.Arquivo.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// Lê o conteúdo de um arquivo e retorna como uma string.
        /// </summary>
        /// <param name="filePath">Caminho do arquivo.</param>
        /// <returns>Conteúdo do arquivo como string.</returns>
        public static async Task<Byte[]> RecuperarArquivoDisco(string local_arquivo_completo)
        {
            return await System.IO.File.ReadAllBytesAsync(local_arquivo_completo);
        }
    }
}