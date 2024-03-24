using EntregaTudo.Core.Domain.Base;

namespace EntregaTudo.Core.Domain.Business.Delivery;

/// <summary>
/// Classe dos itens do pedido, pode ser mais de um produto a ser entregue no mesmo pedido;
/// </summary>
public class Delivery : Entity
{
    public List<ItemDelivery> Items { get; set; }
    public string DeliveryAddress { get; set; }

    //Criar um ENUM para setar o status do pedido;
    public string Status { get; set; }

    // Método para calcular o preço da entrega, temos que levar outras consideracoes, como trafego, por exemplo.
    public decimal CalculateDeliveryCost(double distanceInKm)
    {
        var totalWeight = Items.Sum(p => p.Weight); 
        const decimal distanceFactor = 0.5m; // Fator de custo por quilômetro, temos que levar em consideração uma configuracao a ser criada;

        var deliveryCost = (decimal)totalWeight * distanceFactor * (decimal)distanceInKm;

        return deliveryCost;
    }
}