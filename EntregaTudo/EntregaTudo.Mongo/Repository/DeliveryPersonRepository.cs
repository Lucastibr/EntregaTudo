using Codout.Framework.Mongo;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository.Base;

namespace EntregaTudo.Mongo.Repository;

public class DeliveryPersonRepository : RepositoryBase<DeliveryPerson>, IDeliveryPersonRepository
{
    public DeliveryPersonRepository(MongoContext mongoContext) : base(mongoContext)
    {
    }
}