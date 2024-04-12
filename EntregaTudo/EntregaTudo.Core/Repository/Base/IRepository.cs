using EntregaTudo.Core.Domain.Base;
using System.Linq.Expressions;

namespace EntregaTudo.Core.Repository.Base;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    Task Save(TEntity entity);
    Task<TEntity> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    Task Update(TEntity entity);
    Task Delete(Guid id);
    Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
    Task<int> SaveChanges();
}