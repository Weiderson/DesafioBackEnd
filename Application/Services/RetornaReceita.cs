using DesafioBackendAPI.Application.ViewModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DesafioBackendAPI.Application.Services
{
    /// <summary>
    /// Serviço responsável por retornar o nome associado a um CNPJ consultando a ReceitaWS.
    /// </summary>
    public class RetornaReceita
    {
        /// <summary>
        /// Retorna o nome associado ao CNPJ fornecido.
        /// </summary>
        /// <param name="cnpj">O CNPJ a ser consultado.</param>
        /// <returns>O nome associado ao CNPJ ou null se não encontrado.</returns>
        public static async Task<string?> RetornaNome(string cnpj)
        {
            Uri uri = new Uri("https://receitaws.com.br/v1/");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = uri;

                HttpResponseMessage response = await client.GetAsync("cnpj/" + cnpj).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        var receitaCnpj = JsonConvert.DeserializeObject<ReceitaCnpjVM>(content);
                        return receitaCnpj?.Nome;
                    }
                }
                else if ((int)response.StatusCode == 429) // Código de status HTTP 429: Too Many Requests
                {
                    return "TooManyRequests";
                }

                return null;
            }
        }
    }
}