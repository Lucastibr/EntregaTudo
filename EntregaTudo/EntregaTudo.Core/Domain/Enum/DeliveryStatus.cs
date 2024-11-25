using System.ComponentModel.DataAnnotations;

namespace EntregaTudo.Core.Domain.Enum;

public enum DeliveryStatus
{
    [Display(Name = "Entregue")]
    Ok,
    [Display(Name = "Aguardando Envio")]
    Pending,
    [Display(Name = "Em Trânsito")]
    Sended,
}