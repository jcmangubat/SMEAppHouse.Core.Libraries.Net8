using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.Repo.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo
{
    public class EntityRepositoryForGenericEntities<TEntity>(DbContext dbContext)
        : RepositoryGeneric<TEntity, Guid>(dbContext)
        where TEntity : class
    {
    }
}