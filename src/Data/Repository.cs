using Microsoft.EntityFrameworkCore;
using PersonalShop.Data.Contracts;
using System.Linq.Expressions;

namespace PersonalShop.Data;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _dbSet;
    }

    public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null!,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        foreach (var include in includeProperties.Split(new char[] { ',' },
            StringSplitOptions.RemoveEmptyEntries))
        {
            query.Include(include);
        }

        if (orderBy is not null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public Task Add(TEntity entity)
    {
        return Task.FromResult(_dbContext.Add(entity));
    }
    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }
    public IEnumerable<TEntity> GetAll()
    {
        return _dbSet.ToList();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    public TEntity? GetById(Type id)
    {
        return _dbSet.Find(id);
    }
    public async Task<TEntity?> GetByIdAsync(Type id, bool track = true)
    {
        var data = await _dbSet.FindAsync(id);

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
    public IQueryable<TEntity> GetManyQueryable(Expression<Func<TEntity, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
    public void Detach(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbContext.Entry(entity).State = EntityState.Detached;
    }
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }
    public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
}
