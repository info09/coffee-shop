using IDP.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace IDP.Common.Domains;

public class RepositoryBase<T, K> : IRepositoryBase<T, K> where T : EntityBase<K>
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryBase(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    #region Query

    public IQueryable<T> FindAll(bool trackChanges = false) =>
        !trackChanges ? _context.Set<T>().AsNoTracking() :
            _context.Set<T>();

    public IQueryable<T> FindAll(bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges = false)
    => !trackChanges
        ? _context.Set<T>().Where(expression).AsNoTracking()
        : _context.Set<T>().Where(expression);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public Task<T> GetByIdAsync(K id)
        => FindByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();

    public Task<T> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties)
        => FindByCondition(x => x.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();

    #endregion

    #region Action

    public async Task<K> CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Unchanged) return;

        T exist = await _context.Set<T>().FindAsync(entity.Id);
        _context.Entry(exist).CurrentValues.SetValues(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateListAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteListAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        await SaveChangesAsync();
    }

    #endregion

    public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync()
        => _context.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
        => _context.Database.RollbackTransactionAsync();
}