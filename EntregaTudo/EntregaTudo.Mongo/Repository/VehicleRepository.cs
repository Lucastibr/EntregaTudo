using Codout.Framework.Mongo;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository.Base;

namespace EntregaTudo.Mongo.Repository;

public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
{
    public VehicleRepository(MongoContext mongoContext) : base(mongoContext)
    {
    }
}