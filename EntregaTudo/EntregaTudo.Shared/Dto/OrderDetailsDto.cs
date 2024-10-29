namespace EntregaTudo.Shared.Dto;

public class OrderDetailsDto
{
    public string DeliveryCode { get; set; }

    public string DeliveryStatus {get; set;}
    public string OrderId { get; set;}

    public decimal DeliveryCost { get; set; }

    public DestinationDeliveryDto DestinationDelivery { get; set; }
    public List<OrderDetailItemsDto> OrderDetailsItems { get; set; }
    public DateTime DateHourOrder { get; set; }

    public class DestinationDeliveryDto
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
        public string State { get; set; }

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

    public class OrderDetailItemsDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Weight { get; set; }
    }
}