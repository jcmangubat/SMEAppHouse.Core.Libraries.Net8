using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.Repository
{
    public class RepositoryForGuidKeyedEntity<TEntity>(DbContext dbContext)
        : RepositoryForKeyedEntity<TEntity, Guid>(dbContext)
        where TEntity : class, IKeyedEntity<Guid>
    {
    }
}