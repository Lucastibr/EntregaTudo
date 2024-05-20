using EntregaTudo.Dal.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Repository.Base;

namespace EntregaTudo.Dal.Repository.Base;


public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly EntregaTudoDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryBase(EntregaTudoDbContext db)
    {
        Db = db;
        DbSet = Db.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task Save(TEntity entity)
    {
        //Db.Entry(entity).State = EntityState.Detached;
        //Db.Set<TEntity>().Add(entity);
        //await Db.SaveChangesAsync();

        DbSet.Add(entity);
        await SaveChanges();
    }

    public virtual async Task Update(TEntity entity)
    {
        //Db.Entry(entity).State = EntityState.Detached;
        //Db.Set<TEntity>().Update(entity);
        //await Db.SaveChangesAsync();
        //Db.ChangeTracker.Clear();

        DbSet.Update(entity);
        await SaveChanges();
    }

    public virtual async Task Delete(Guid id)
    {
        DbSet.Remove(await DbSet.FindAsync(id));
        await SaveChanges();
    }

    public async Task<int> SaveChanges()
    {
        return await Db.SaveChangesAsync();
    }

    public async void Dispose()
    {
        Db?.Dispose(); // ? Significa se ele existir, faça o dispose, se não existir, não faça.
    }
}