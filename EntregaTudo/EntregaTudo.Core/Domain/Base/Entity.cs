using System.ComponentModel.DataAnnotations;

namespace EntregaTudo.Core.Domain.Base;

public abstract class Entity
{
    [Key]
    public Guid? Id { get; set; } = Guid.NewGuid();
}