using Codout.Framework.Mongo;
using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository.Base;

namespace EntregaTudo.Mongo.Repository;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(MongoContext mongoContext) : base(mongoContext)
    {
    }
}