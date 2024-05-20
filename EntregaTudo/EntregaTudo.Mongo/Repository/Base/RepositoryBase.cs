using System.Linq.Expressions;
using MongoDB.Bson;
using Codout.Framework.DAL.Entity;
using Codout.Framework.Mongo;

namespace EntregaTudo.Mongo.Repository.Base;

public class RepositoryBase<T> : MongoCollection<T> where T : class, IEntity
{
    public RepositoryBase(MongoContext mongoContext) : base(mongoContext)
    {
    }

    public IEnumerable<T> Find(int? pageSize, int? pageNumber, out int count, Expression<Func<T, bool>>? expression = null)
    {
        var query = All();

        if (expression != null)
            query = query.Where(expression);

        count = query.Count();

        var take = pageSize is <= 0 or null ? 20 : pageSize.Value;
        var skip = ((pageNumber is <= 0 or null ? 1 : pageNumber.Value) - 1) * take;

        return query.Skip(skip).Take(take);
    }

    public T Get(string key) => !string.IsNullOrWhiteSpace(key) ? base.Get(ObjectId.Parse(key)) : null;

    public T Load(string key) => !string.IsNullOrWhiteSpace(key) ? base.Get(ObjectId.Parse(key)) : null;

    public async Task<T> GetAsync(string key) => !string.IsNullOrWhiteSpace(key) ? await base.GetAsync(ObjectId.Parse(key)) : null;

    public async Task<T> LoadAsync(string key) => !string.IsNullOrWhiteSpace(key) ? await base.LoadAsync(ObjectId.Parse(key)) : null;
}