using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Repository;
using EntregaTudo.Dal.Context;
using EntregaTudo.Dal.Repository.Base;

namespace EntregaTudo.Dal.Repository;

public class DeliveryRepository : RepositoryBase<Delivery>, IDeliveryRepository
{
    public DeliveryRepository(EntregaTudoDbContext db) : base(db)
    {
    }
}