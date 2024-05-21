using Codout.Framework.Mongo;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository.Base;

namespace EntregaTudo.Mongo.Repository;

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(MongoContext mongoContext) : base(mongoContext)
    {
    }
}