using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.Infrastructure;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EntregaTudo.Core.Domain.Business.Delivery;

/// <summary>
/// Classe dos itens do pedido, pode ser mais de um produto a ser entregue no mesmo pedido;
/// </summary>
public class Order : MongoEntity
{
    [BsonElement("items")]
    public List<OrderItem> Items { get; set; } = new();

    [BsonElement("originDelivery")]
    public Address OriginDelivery { get; set; }

    [BsonElement("destinationDelivery")]
    public Address DestinationDelivery { get; set; }

    [BsonElement("deliveryStatus")]
    public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.Pending;

    [BsonElement("scheduledTime")]
    public DateTime ScheduledTime { get; set; }

    [BsonElement("deliveryCost")]
    public decimal DeliveryCost { get; set; }

    [BsonElement("deliveryNote")]
    public string DeliveryNote { get; set; }

    [BsonElement("deliveryCode")]
    public string DeliveryCode { get; set; }

    [BsonElement("customerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CustomerId { get; set; }

    [BsonElement("deliveryPersonId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string DeliveryPersonId { get; set; }

    public decimal CalculateDeliveryCost(double distanceInKm, decimal distanceFactor)
    {
        var totalWeight = Items.Sum(p => p.Weight);
        var deliveryCost = (decimal)totalWeight * distanceFactor * (decimal)distanceInKm;
        return deliveryCost;
    }

    public bool ConfirmDelivery(string providedDeliveryCode)
    {
        if (providedDeliveryCode != DeliveryCode) return false;
        DeliveryStatus = DeliveryStatus.Ok;
        return true;
    }
}