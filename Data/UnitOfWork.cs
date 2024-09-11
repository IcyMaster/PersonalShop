using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PersonalShop.Data.Contracts;
using System;
using System.Data;

namespace PersonalShop.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction _transaction;
    private bool _disposed = false;

    public UnitOfWork(ApplicationDbContext dbContext, Dictionary<Type, object> repositories)
    {
        _dbContext = dbContext;
        _repositories = repositories;
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(_dbContext);

        _repositories.Add(typeof(TEntity), repository);

        return repository;
    }
    public int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return _dbContext.SaveChanges(acceptAllChangesOnSuccess);
    }
    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }
    public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task CommitTransactionAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            _transaction.Dispose();
        }
    }
    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
        _transaction.Dispose();
    }
    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
