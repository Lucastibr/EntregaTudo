using EntregaTudo.Shared.Dto.Base;
using EntregaTudo.Shared.Enums;

namespace EntregaTudo.Shared.Dto;

public class DeliveryPersonDto : DtoBase
{
    /// <summary>
    /// Primeiro Nome
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// SobreNome
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Número do documento 
    /// </summary>
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// E-mail
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Telefone
    /// </summary>
    public string? PhoneNumber { get; set; }

    public List<OrderDto>? Orders { get; set; } = [];

    public VehicleDto Vehicle { get; set; } = new();

    /// <summary>
    /// Se a pessoa é que vai enviar ou vai entregar o pedido
    /// </summary>
    public PersonType? PersonType { get; set; }

    public string? Password { get; set; }
}