using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Infrastructure;

namespace EntregaTudo.Core.Domain.Customer;

/// <summary>
/// Classe do Cliente
/// </summary>
public class Customer : Entity
{
    public string? Name { get; set; }

    public CustomerAddress CustomerAddress { get; set; }

    public string? DocumentNumber {get; set;}
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

}