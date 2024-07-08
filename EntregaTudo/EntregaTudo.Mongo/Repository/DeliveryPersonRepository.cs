using Codout.Framework.Mongo;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository.Base;

namespace EntregaTudo.Mongo.Repository;

public class DeliveryPersonRepository(MongoContext mongoContext)
    : RepositoryBase<DeliveryPerson>(mongoContext), IDeliveryPersonRepository
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public DeliveryPerson? GetDeliveryPersonByEmail(string email)
    {
        return Find(p => p.Email == email).FirstOrDefault();
    }
}