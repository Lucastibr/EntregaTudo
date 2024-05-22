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
}