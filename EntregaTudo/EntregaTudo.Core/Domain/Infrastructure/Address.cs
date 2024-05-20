using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Enum;

namespace EntregaTudo.Core.Domain.Infrastructure;

/// <summary>
/// Classe de endereço
/// </summary>
public class Address : MongoEntity
{
    /// <summary>
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
    /// Estado
    /// </summary>
    public State State { get; set; }

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
    public double Latitude {get; set;}

    /// <summary>
    /// Longitude
    /// </summary>
    public double Longitude {get; set;}
}