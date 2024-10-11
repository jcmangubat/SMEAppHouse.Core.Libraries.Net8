using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.Repository
{
    public class RepositoryForIntKeyedEntity<TEntity>(DbContext dbContext)
        : RepositoryForKeyedEntity<TEntity, int>(dbContext)
        where TEntity : class, IKeyedEntity<int>
    {
    }
}