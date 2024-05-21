using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace EntregaTudo.Core.Domain.User;

public class DeliveryPerson : Person
{
    public DeliveryPerson()
    {
        PersonType = PersonType.DeliveryPerson;
    }

    /// <summary>
    /// Veículos (apenas se a pessoa for um entregador)
    /// </summary>
    [BsonElement("vehicle")]
    public Vehicle? Vehicle { get; set; }

    /// <summary>
    /// Lista de Entregas realizadas pelo entregador
    /// </summary>
    [BsonElement("deliveries")]
    public List<Order> Deliveries { get; set; } = new();
}