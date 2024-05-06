using System.Text.Json;

namespace EntregaTudo.Api.Helpers;

public class CepHelper
{
    public async Task<SearchAddress?> SearchAddress(string cep)
    {
        var http = new HttpClient();

        var uri = new Uri($"https://cep.awesomeapi.com.br/json/{cep}");

        var result = await http.GetAsync(uri);

        var content = await result.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<SearchAddress>(content);

    }
}