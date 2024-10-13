using System.Linq.Expressions;

namespace PersonalShop.Data.Contracts;

public interface IRepository<T>
{
    public IQueryable<T> GetQueryable();

    public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null!,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
        string includeProperties = "");

    public Task Add(T entity);
    public void Delete(T entity);
    public void Update(T entity);
    Task<int> CountAsync();
    public IEnumerable<T> GetAll();
    public Task<IEnumerable<T>> GetAllAsync();
    public T? GetById(Type id);
    public Task<T?> GetByIdAsync(Type id, bool track = true);
    Task AddAsync(T entity);
    void RemoveRange(IEnumerable<T> entities);
    IQueryable<T> GetManyQueryable(Expression<Func<T, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
    void Detach(T entity);
}
