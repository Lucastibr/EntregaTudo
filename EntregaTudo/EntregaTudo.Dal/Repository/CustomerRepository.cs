using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Dal.Context;
using EntregaTudo.Dal.Repository.Base;

namespace EntregaTudo.Dal.Repository;

/// <summary>
/// Repositório de Cliente
/// </summary>
public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(EntregaTudoDbContext db) : base(db)
    {

    }
}