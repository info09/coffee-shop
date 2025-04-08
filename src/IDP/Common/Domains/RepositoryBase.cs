﻿using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace IDP.Common.Domains;

public class RepositoryBase<T, K> : IRepositoryBase<T, K> where T : EntityBase<K>
{
    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<K> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public Task EndTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAsync(K id)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties)
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateListAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }
}
