using EntregaTudo.Core.Domain.Base;

namespace EntregaTudo.Core.Domain.Business.Delivery;

/// <summary>
/// Classe do Produto a ser enviado
/// </summary>
public class OrderItem : MongoEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Weight { get; set; }
}