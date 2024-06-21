using System;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.Repository.GuidPKBasedVariation
{
    public class EntityRepositoryGeneric<TEntity>(DbContext dbContext) 
        : RepositoryGeneric<TEntity, Guid>(dbContext)
        where TEntity : class
    {
    }
}