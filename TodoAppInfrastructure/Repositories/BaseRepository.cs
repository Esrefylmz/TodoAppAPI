using Microsoft.EntityFrameworkCore;
using TodoAppInfrastructure.Data;

namespace TodoAppInfrastructure.Repositories;

public abstract class BaseRepository<T>
    where T : class
{
    protected readonly TodoContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(TodoContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
        _dbContext.SaveChanges();
    }

    protected void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    protected void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}