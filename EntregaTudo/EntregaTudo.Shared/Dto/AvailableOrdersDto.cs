namespace EntregaTudo.Shared.Dto;

/// <summary>
/// Dto das entregas disponíveis
/// </summary>
public class AvailableOrdersDto 
{
    public decimal? OrderPrice { get; set; }

    public AddressDto Address { get; set; }

    public string DeliveryCode { get; set; }

    public string Id { get; set; }

    public string? PhoneNumber {get; set;}

    public string? LicensePlate {get; set;}

    public string? DeliveryPersonName {get; set;}
    public string? CustomerName {get; set;}
}