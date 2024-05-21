using EntregaTudo.Core.Domain.Base;
using MongoDB.Bson;

namespace EntregaTudo.Core.Domain.Business.Financial;

public class Order : MongoEntity
{
    /// <summary>
    /// Delivery
    /// </summary>
    public ObjectId DeliveryId { get; set; }

    /// <summary>
    /// Data da Criação
    /// </summary>
    public DateTime? CreatedAt { get; set; }
}