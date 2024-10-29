namespace EntregaTudo.Shared.Dto;

public class OrderDto
{
    public AddressDto AddressOrigin { get; set; }
    public AddressDto AddressDestiny { get; set; }

    public List<OrderItemDto?> Items {get; set;} = [];

    public decimal? DeliveryCost { get; set; }
}