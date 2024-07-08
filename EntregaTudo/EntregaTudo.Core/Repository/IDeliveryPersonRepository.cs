using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository.Base;

namespace EntregaTudo.Core.Repository;

public interface IDeliveryPersonRepository : IRepositoryBase<DeliveryPerson>
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string hashedPassword);

    DeliveryPerson? GetDeliveryPersonByEmail(string email);
}