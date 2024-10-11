using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork
{
    public interface IGenericUnitOfWork<TPk> : IDisposable
        where TPk : struct
    {
        IRepositoryForKeyedEntity<TEntity, TPk> GetRepository<TEntity>()
            where TEntity : class, IKeyedEntity<TPk>;

        int SaveChanges();
    }

    public interface IGenericUnitOfWork<out TContext, TPk> : IGenericUnitOfWork <TPk>
        where TContext : DbContext
        where TPk : struct
    {
        TContext DbContext { get; }
    }
}
