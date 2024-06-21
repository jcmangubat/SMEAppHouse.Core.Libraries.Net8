using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.Repository.IntPKBasedVariation
{
    public class EntityRepository<TEntity>(DbContext dbContext) 
        : RepositoryForKeyedEntity<TEntity, int>(dbContext)
        where TEntity : class, IEntityKeyed<int>
    {
    }
}