using MongoDB.Bson;

namespace EntregaTudo.Shared.Dto.Base;

public abstract class DtoBase 
{
    public ObjectId? Id {get; set;}
}