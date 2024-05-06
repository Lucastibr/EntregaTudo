namespace EntregaTudo.Shared.Dto;

public class DeliveryDto
{
    public AddressDto AddressOrigin { get; set; }
    public AddressDto AddressDestiny { get; set; }

    public List<ItemDeliveryDto> Items = new();
}