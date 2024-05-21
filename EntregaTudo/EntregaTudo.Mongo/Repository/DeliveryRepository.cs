using Codout.Framework.Mongo;
using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository.Base;

namespace EntregaTudo.Mongo.Repository;

public class DeliveryRepository : RepositoryBase<Order>, IDeliveryRepository
{
    public DeliveryRepository(MongoContext mongoContext) : base(mongoContext)
    {
    }
}