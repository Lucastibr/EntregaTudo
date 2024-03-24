using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.Infrastructure;

namespace EntregaTudo.Core.Domain.User.Base
{
    /// <summary>
    /// Classe base para definir os atributos dos usuários
    /// </summary>
    public abstract class Person : Entity
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
        /// Lista de Endereços
        /// </summary>
        public IList<DeliveryAddress> DeliveryAddresses { get; set; } = [];

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

        /// <summary>
        /// Se a pessoa é que vai enviar ou vai entregar o pedido
        /// </summary>
        public PersonType PersonType { get; set; }
    }
}
