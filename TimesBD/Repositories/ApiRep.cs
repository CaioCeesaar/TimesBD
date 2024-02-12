using Newtonsoft.Json;
using TimesBD.Entities;

namespace TimesBD.Repositories
{
    public class ApiRep
    {
        private readonly HttpClient _httpClient = new();

        public async Task<Endereco?> ConsultarCep(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var endereco = JsonConvert.DeserializeObject<Endereco>(content);
                return endereco;
            }
            return null;
        }
    }
}
