using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Dal.Context;
using EntregaTudo.Dal.Repository.Base;

namespace EntregaTudo.Dal.Repository;

/// <summary>
/// Repositório de Cliente
/// </summary>
public class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
    public PersonRepository(EntregaTudoDbContext db) : base(db)
    {
    }
}