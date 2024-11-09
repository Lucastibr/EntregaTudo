using System.Text.Json.Serialization;

namespace EntregaTudo.Api.Helpers;

public class SearchAddress
{
    /// <summary>
    /// Cep
    /// </summary>
    [JsonPropertyName("cep")]
    public string PostalCode { get; set; }

    /// <summary>
    /// Setor
    /// </summary>
    [JsonPropertyName("district")]
    public string District { get; set; }

    /// <summary>
    /// Estado
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; }

    /// <summary>
    /// Rua
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; }

    /// <summary>
    /// Latitude
    /// </summary>
    [JsonPropertyName("lat")]
    public string Lat {get; set;}
    
    /// <summary>
    /// Longitude
    /// </summary>
    [JsonPropertyName("lng")]
    public string Lng {get; set;}
}