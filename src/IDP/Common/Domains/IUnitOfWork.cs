using Microsoft.EntityFrameworkCore;

namespace IDP.Common.Domains;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
}