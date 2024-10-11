using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbEntityCfg<TEntity> where TEntity : class
    {
        public abstract void Map(EntityTypeBuilder<TEntity> entityBuilder);
        //public abstract void IgnoreConvention(Expression<Func<TEntity, object>> property);
    }
}