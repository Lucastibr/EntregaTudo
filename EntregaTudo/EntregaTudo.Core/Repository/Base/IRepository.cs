using System.Linq.Expressions;
using Codout.Framework.DAL.Entity;
using Codout.Framework.DAL.Repository;

namespace EntregaTudo.Core.Repository.Base;

public interface IRepositoryBase<T> : IRepository<T> where T : class, IEntity
{
    public T Get(string key);

    public T Load(string key);

    public Task<T> GetAsync(string key);

    public Task<T> LoadAsync(string key);
}