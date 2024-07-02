using Codout.Framework.Mongo;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository.Base;

namespace EntregaTudo.Mongo.Repository;

public class CustomerRepository(MongoContext mongoContext) : RepositoryBase<Customer>(mongoContext), ICustomerRepository
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public Customer? GetPersonByEmail(string email)
    {
        return Find(p => p.Email == email).FirstOrDefault();
    }
}