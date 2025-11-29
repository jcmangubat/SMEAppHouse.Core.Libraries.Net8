using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions
{
    /// <summary>
    /// Abstract base class for configuring entity mappings using the Fluent API.
    /// Inherit from this class to define entity-specific configuration.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to configure.</typeparam>
    public abstract class DbEntityCfg<TEntity> where TEntity : class
    {
        /// <summary>
        /// Configures the entity using the Fluent API.
        /// </summary>
        /// <param name="entityBuilder">The builder to use for configuring the entity.</param>
        /// <exception cref="ArgumentNullException">Thrown when entityBuilder is null.</exception>
        public abstract void Map(EntityTypeBuilder<TEntity> entityBuilder);
    }
}