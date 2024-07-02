using EntregaTudo.Core.Domain.Enum;

namespace EntregaTudo.Core.Domain.User;

public class Customer : Person
{
    public Customer()
    {
        PersonType = PersonType.User;
    }
}