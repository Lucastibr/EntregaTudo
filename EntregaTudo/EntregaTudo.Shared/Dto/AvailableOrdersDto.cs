using EntregaTudo.Shared.Dto.Base;

namespace EntregaTudo.Shared.Dto;

/// <summary>
/// Dto das entregas disponíveis
/// </summary>
public class AvailableOrdersDto : DtoBase
{
    public decimal? OrderPrice { get; set; }

    public AddressDto Address { get; set; }

    public string DeliveryCode { get; set; }
}