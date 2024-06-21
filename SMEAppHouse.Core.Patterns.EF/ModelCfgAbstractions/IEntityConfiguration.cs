using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.ModelCfgAbstractions
{
    public interface IEntityConfiguration<TEntity, TPk> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
        where TPk : struct
    {
        string Schema { get; }
        Expression<Func<TEntity, object>>[] FieldsToIgnore { get; set; }

        void OnModelCreating(EntityTypeBuilder<TEntity> entityBuilder);
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}