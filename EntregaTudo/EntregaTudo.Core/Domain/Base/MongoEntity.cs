using Codout.Framework.DAL.Entity;
using System.Reflection;
using Codout.Framework.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace EntregaTudo.Core.Domain.Base;

[Serializable]
public abstract class MongoEntity : ValidatableObject, IEntity<ObjectId>
{
    private const int HashMultiplier = 31;

    private int? _cachedHashcode;

    [BsonElement("_id")]
    [JsonProperty("_id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual ObjectId Id { get; set; }

    public virtual bool IsTransient()
    {
        return Id == ObjectId.Empty; //string.IsNullOrWhiteSpace(Id);
    }

    public override bool Equals(object obj)
    {
        var compareTo = obj as MongoEntity;

        if (ReferenceEquals(this, compareTo))
        {
            return true;
        }

        if (compareTo == null || GetType() != compareTo.GetTypeUnproxied())
        {
            return false;
        }

        if (HasSameNonDefaultIdAs(compareTo))
        {
            return true;
        }

        return IsTransient() && compareTo.IsTransient() && HasSameObjectSignatureAs(compareTo);
    }

    public override int GetHashCode()
    {
        if (_cachedHashcode.HasValue)
        {
            return _cachedHashcode.Value;
        }

        if (IsTransient())
        {
            _cachedHashcode = base.GetHashCode();
        }
        else
        {
            unchecked
            {
                var hashCode = GetType().GetHashCode();
                _cachedHashcode = (hashCode * HashMultiplier) ^ Id.GetHashCode();
            }
        }

        return _cachedHashcode.Value;
    }

    private bool HasSameNonDefaultIdAs(MongoEntity compareTo)
    {
        return !IsTransient() && !compareTo.IsTransient() && Id.Equals(compareTo.Id);
    }

    protected override IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties()
    {
        throw new NotImplementedException();
    }
}