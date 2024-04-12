using System.ComponentModel.DataAnnotations;
using EntregaTudo.Shared.Dto.Base;
using EntregaTudo.Shared.Enums;

namespace EntregaTudo.Shared.Dto;

public class PersonDto : DtoBase
{
    /// <summary>
    /// Primeiro Nome
    /// </summary>
    [Required(ErrorMessage ="Informe o nome")]
    [MaxLength(80, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
    public string? FirstName { get; set; }

    /// <summary>
    /// SobreNome
    /// </summary>
    [Required(ErrorMessage ="Informe o sobrenome")]
    [MaxLength(80, ErrorMessage = "O sobrenome deve ter no máximo 200 caracteres")]
    public string? LastName { get; set; }

    /// <summary>
    /// Número do documento 
    /// </summary>
    [Required(ErrorMessage ="Informe o Número do Documento")]
    [MaxLength(80, ErrorMessage = "O número do documento deve ter no máximo 14 caracteres")]
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// E-mail
    /// </summary>
    [Required(ErrorMessage ="Informe o e-mail")]
    public string? Email { get; set; }

    /// <summary>
    /// Telefone
    /// </summary>
    [Required(ErrorMessage ="Informe o telefone")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Se a pessoa é que vai enviar ou vai entregar o pedido
    /// </summary>
    [Required(ErrorMessage ="Informe o tipo de Pessoa")]
    public PersonType? PersonType { get; set; }
}