using Microsoft.EntityFrameworkCore;

namespace IDP.Common.Domains;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
}

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
}