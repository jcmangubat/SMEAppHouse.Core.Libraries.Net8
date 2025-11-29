using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.DbContextAbstractions
{
    /// <summary>
    /// Factory class for creating DbContext instances with configuration from IAppConfig.
    /// </summary>
    /// <typeparam name="T">The type of DbContext to create.</typeparam>
    public class DbContextFactory<T> : IDbContextFactory<T>
        where T : DbContext, new()
    {
        private readonly IAppConfig _appConfig;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory{T}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="appConfig">The application configuration containing EF behavior attributes.</param>
        /// <exception cref="ArgumentNullException">Thrown when configuration or appConfig is null.</exception>
        public DbContextFactory(IConfiguration configuration, IAppConfig appConfig)
        {
            _appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Creates a new DbContext instance using the connection string from configuration.
        /// </summary>
        /// <returns>A new instance of the DbContext.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the connection string is not found in configuration.</exception>
        public T CreateDbContext()
        {
            var connectionString = _configuration.GetConnectionString("AppDbConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'AppDbConnection' not found in configuration.");

            return CreateDbContext(connectionString);
        }

        /// <summary>
        /// Creates a new DbContext instance using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>A new instance of the DbContext.</returns>
        /// <exception cref="ArgumentNullException">Thrown when connectionString is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when AppEFBehaviorAttributes is not configured.</exception>
        public T CreateDbContext(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (_appConfig.AppEFBehaviorAttributes == null)
                throw new InvalidOperationException("AppEFBehaviorAttributes is not configured.");

            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(connectionString, x =>
            {
                var migrationInfo = _appConfig.AppEFBehaviorAttributes;
                if (!string.IsNullOrWhiteSpace(migrationInfo.MigrationTblName))
                {
                    x.MigrationsHistoryTable(migrationInfo.MigrationTblName, migrationInfo.DbSchema ?? "dbo");
                }
            });
            var dbCntxOpt = optionsBuilder.Options;
            
            // Use Activator to create instance of T instead of DbContext
            return (T)Activator.CreateInstance(typeof(T), dbCntxOpt)!;
        }
    }
}
