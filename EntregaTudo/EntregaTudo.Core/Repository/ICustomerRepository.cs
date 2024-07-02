using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository.Base;

namespace EntregaTudo.Core.Repository;

public interface ICustomerRepository : IRepositoryBase<Customer>
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string hashedPassword);

    Customer? GetPersonByEmail(string email);
}