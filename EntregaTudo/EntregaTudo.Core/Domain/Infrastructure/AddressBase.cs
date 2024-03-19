namespace EntregaTudo.Core.Domain.Infrastructure;

/// <summary>
/// Classe base para que tanto o cliente, quanto o entregador herde o endereço
/// </summary>
public abstract class AddressBase
{
    public string Name { get; set; } 
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}