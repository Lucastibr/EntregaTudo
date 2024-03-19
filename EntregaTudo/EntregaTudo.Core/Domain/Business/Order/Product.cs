using EntregaTudo.Core.Domain.Base;

namespace EntregaTudo.Core.Domain.Business.Order;

/// <summary>
/// Classe do Produto a ser enviado
/// </summary>
public class Product : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Weight { get; set; }
}