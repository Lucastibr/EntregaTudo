namespace EntregaTudo.Shared.Dto;

public class AddressDto
{
    /// Endereço Rua
    /// </summary>
    public string StreetAddress { get; set; }

    /// <summary>
    /// Número do Endereço
    /// </summary>
    public string NumberAddress { get; set; }

    /// <summary>
    /// Complemento do Endereço
    /// </summary>
    public string AddressComplement { get; set; }

    /// <summary>
    /// Complemento do Endereço
    /// </summary>
    public string Neighborhood { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Código Postal (CEP)
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    /// País
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Latitude
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude
    /// </summary>
    public double Longitude { get; set; }
}