using System.Text.Json;

namespace EntregaTudo.Api.Helpers;

public static class CepHelper
{
    public static async Task<SearchAddress?> SearchAddress(string cep)
    {
        var http = new HttpClient();

        var uri = new Uri($"https://cep.awesomeapi.com.br/json/{cep}");

        var result = await http.GetAsync(uri);

        var content = await result.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<SearchAddress>(content);

    }

    /// <summary>
    /// Método para calcular a distância em KM da origem para o destino
    /// </summary>
    /// <param name="longitude"></param>
    /// <param name="latitude"></param>
    /// <param name="otherLongitude"></param>
    /// <param name="otherLatitude"></param>
    /// <returns></returns>
    public static double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
    {
        // Converter graus para radianos
        var lat1Rad = latitude * (Math.PI / 180.0);
        var lon1Rad = longitude * (Math.PI / 180.0);
        var lat2Rad = otherLatitude * (Math.PI / 180.0);
        var lon2Rad = otherLongitude * (Math.PI / 180.0);

        // Diferença entre as longitudes e latitudes
        var dLat = lat2Rad - lat1Rad;
        var dLon = lon2Rad - lon1Rad;

        // Fórmula de Haversine
        var a = Math.Pow(Math.Sin(dLat / 2.0), 2.0) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Pow(Math.Sin(dLon / 2.0), 2.0);
        var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

        // Raio da Terra em quilômetros
        var radiusEarthKm = 6371.0;

        // Distância em quilômetros
        return radiusEarthKm * c;
    }
}