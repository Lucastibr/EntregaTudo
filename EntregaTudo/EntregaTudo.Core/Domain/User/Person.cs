using System.ComponentModel.DataAnnotations;
using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace EntregaTudo.Core.Domain.User;
public abstract class Person : MongoEntity
{
    /// <summary>
    /// Primeiro Nome
    /// </summary>
    [Required(ErrorMessage = "Informe o nome")]
    [MaxLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres")]
    [BsonElement("firstName")]
    public string? FirstName { get; set; }

    /// <summary>
    /// SobreNome
    /// </summary>
    [Required(ErrorMessage = "Informe o sobrenome")]
    [MaxLength(80, ErrorMessage = "O sobrenome deve ter no máximo 80 caracteres")]
    [BsonElement("lastName")]
    public string? LastName { get; set; }

    /// <summary>
    /// Senha
    /// </summary>
    [Required(ErrorMessage = "Informe a senha")]
    [BsonElement("password")]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Número do documento 
    /// </summary>
    [Required(ErrorMessage = "Informe o Número do Documento")]
    [MaxLength(14, ErrorMessage = "O número do documento deve ter no máximo 14 caracteres")]
    [BsonElement("documentNumber")]
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// E-mail
    /// </summary>
    [Required(ErrorMessage = "Informe o e-mail")]
    [BsonElement("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Telefone
    /// </summary>
    [Required(ErrorMessage = "Informe o telefone")]
    [BsonElement("phoneNumber")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Se a pessoa é que vai enviar ou vai entregar o pedido
    /// </summary>
    [Required(ErrorMessage = "Informe o tipo de pessoa")]
    [BsonElement("personType")]
    public PersonType PersonType { get; set; }

    /// <summary>
    /// Veículos (apenas se a pessoa for um entregador)
    /// </summary>
    [BsonElement("vehicle")]
    public Vehicle? Vehicle { get; set; }

    /// <summary>
    /// Lista de Entregas
    /// </summary>
    [BsonElement("deliveries")]
    public List<Order> Deliveries { get; set; } = new();
}
