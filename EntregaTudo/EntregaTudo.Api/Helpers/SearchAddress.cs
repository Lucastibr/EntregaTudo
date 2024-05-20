using System.Text.Json.Serialization;

namespace EntregaTudo.Api.Helpers;

public class SearchAddress
{
    [JsonPropertyName("district")]
    public string District { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("lat")]
    public string Lat {get; set;}
    
    [JsonPropertyName("lng")]
    public string Lng {get; set;}
}