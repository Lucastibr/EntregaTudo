using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Repository;
using EntregaTudo.Dal.Context;
using EntregaTudo.Dal.Repository.Base;

namespace EntregaTudo.Dal.Repository;

public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
{
    public VehicleRepository(EntregaTudoDbContext db) : base(db)
    {
    }
}