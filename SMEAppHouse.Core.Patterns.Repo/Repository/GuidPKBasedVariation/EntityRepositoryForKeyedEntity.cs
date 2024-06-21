using System;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.Repository.GuidPKBasedVariation
{
    public class EntityRepositoryForKeyedEntity<TEntity>(DbContext dbContext) 
        : RepositoryForKeyedEntity<TEntity, Guid>(dbContext)
        where TEntity : class, IEntityKeyed<Guid>
    {
    }
}