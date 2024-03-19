namespace EntregaTudo.Core.Domain.Base;

public abstract class Entity
{
    public Guid? Id { get; set; } = Guid.NewGuid();
}