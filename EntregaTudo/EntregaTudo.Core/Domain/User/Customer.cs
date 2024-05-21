using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace EntregaTudo.Core.Domain.User;

public class Customer : Person
{
    public Customer()
    {
        PersonType = PersonType.User;
    }

    /// <summary>
    /// Lista de Pedidos (Entregas) realizados pelo cliente
    /// </summary>
    [BsonElement("deliveries")]
    public List<Order> Deliveries { get; set; } = new();
}