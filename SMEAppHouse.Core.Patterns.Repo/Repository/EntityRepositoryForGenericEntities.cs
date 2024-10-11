using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.Repository
{
    public class EntityRepositoryForGenericEntities<TEntity>(DbContext dbContext)
        : RepositoryGeneric<TEntity, Guid>(dbContext)
        where TEntity : class
    {
    }
}