using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.EF.DbContextAbstractions
{
    /// <summary>
    /// Interface for creating DbContext instances.
    /// </summary>
    /// <typeparam name="T">The type of DbContext to create.</typeparam>
    public interface IDbContextFactory<T> where T : DbContext
    {
        /// <summary>
        /// Creates a new DbContext instance using configuration.
        /// </summary>
        /// <returns>A new instance of the DbContext.</returns>
        T CreateDbContext();

        /// <summary>
        /// Creates a new DbContext instance using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>A new instance of the DbContext.</returns>
        T CreateDbContext(string connectionString);
    }
}