using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.Infrastructure;

namespace EntregaTudo.Core.Domain.Business.Delivery;

/// <summary>
/// Classe dos itens do pedido, pode ser mais de um produto a ser entregue no mesmo pedido;
/// </summary>
public class Delivery : Entity
{
    public List<ItemDelivery> Items { get; set; }
    public Address Address { get; set; }
    public DeliveryStatus DeliveryStatus { get; set; }
    public DateTime ScheduledTime { get; set; } // Data e hora agendada para a entrega
    public decimal DeliveryCost { get; set; } // Custo da entrega
    public string DeliveryNote { get; set; } // Observações adicionais para a entrega
    public string DeliveryCode { get; set; }

    // Método para calcular o preço da entrega
    public decimal CalculateDeliveryCost(double distanceInKm, decimal distanceFactor)
    {
        var totalWeight = Items.Sum(p => p.Weight); 
        var deliveryCost = (decimal)totalWeight * distanceFactor * (decimal)distanceInKm;
        return deliveryCost;
    }

    // Método para calcular o tempo estimado de entrega com base na distância
    public TimeSpan CalculateEstimatedDeliveryTime(double distanceInKm, double averageSpeed)
    {
        var estimatedTimeInHours = distanceInKm / averageSpeed;
        return TimeSpan.FromHours(estimatedTimeInHours);
    }

    // Método para adicionar itens à entrega
    public void AddItem(ItemDelivery item)
    {
        Items.Add(item);
    }

    // Método para verificar e marcar a entrega como "OK"
    public bool ConfirmDelivery(string providedDeliveryCode)
    {
        if (providedDeliveryCode != DeliveryCode) return false;
        DeliveryStatus = DeliveryStatus.Ok;
        return true;
    }
}