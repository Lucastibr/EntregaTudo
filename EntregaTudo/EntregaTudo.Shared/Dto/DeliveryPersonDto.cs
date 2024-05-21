using EntregaTudo.Shared.Dto.Base;

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

    public List<OrderDto> Orders { get; set; } = new();
}