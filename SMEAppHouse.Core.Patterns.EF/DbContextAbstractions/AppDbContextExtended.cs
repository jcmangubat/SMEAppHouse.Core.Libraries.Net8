using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.EF.DbContextAbstractions
{
    /// <summary>
    /// Extended DbContext implementation that provides additional constructors and static helpers
    /// for creating DbContext instances with custom options, including migration table and schema configuration.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext to extend.</typeparam>
    public class AppDbContextExtended<TDbContext> : DbContext
        where TDbContext : DbContext, new()
    {
        /// <summary>
        /// Gets or sets the DbContext options for this instance.
        /// </summary>
        public DbContextOptions<TDbContext> DbContextOptions { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContextExtended{TDbContext}"/> class.
        /// </summary>
        public AppDbContextExtended() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContextExtended{TDbContext}"/> class
        /// using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <exception cref="ArgumentNullException">Thrown when connectionString is null or empty.</exception>
        public AppDbContextExtended(string connectionString) 
            : this(GetOptions(connectionString))
        {
            // Validation is performed in GetOptions method
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContextExtended{TDbContext}"/> class
        /// using the specified options.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
        public AppDbContextExtended(DbContextOptions<TDbContext> options)
            : base(options)
        {
            DbContextOptions = options ?? throw new ArgumentNullException(nameof(options));
        }

        #endregion

        #region Static Factory Methods

        /// <summary>
        /// Creates a new instance of the DbContext with the specified connection string and migration configuration.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="migrationTblName">The name of the migrations history table.</param>
        /// <param name="dbSchema">The schema for the migrations history table.</param>
        /// <returns>A new instance of the DbContext.</returns>
        /// <exception cref="ArgumentNullException">Thrown when connectionString, migrationTblName, or dbSchema is null or empty.</exception>
        /// <remarks>
        /// Reference: https://blog.oneunicorn.com/2016/10/24/ef-core-1-1-creating-dbcontext-instances/
        /// </remarks>
        public static TDbContext CreateDbContext(string connectionString, string migrationTblName, string dbSchema)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(migrationTblName))
                throw new ArgumentNullException(nameof(migrationTblName));
            if (string.IsNullOrWhiteSpace(dbSchema))
                throw new ArgumentNullException(nameof(dbSchema));

            var dbCntxOpt = GetOptions(connectionString, migrationTblName, dbSchema);
            return (TDbContext)(DbContext)new AppDbContextExtended<TDbContext>(dbCntxOpt);
        }

        /// <summary>
        /// Creates a new instance of the DbContext with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>A new instance of the DbContext.</returns>
        /// <exception cref="ArgumentNullException">Thrown when connectionString is null or empty.</exception>
        public static TDbContext CreateDbContext(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            var dbCntxOpt = GetOptions(connectionString);
            return (TDbContext)(DbContext)new AppDbContextExtended<TDbContext>(dbCntxOpt);
        }

        /// <summary>
        /// Gets the DbContext options for the specified connection string.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>The configured DbContext options.</returns>
        /// <exception cref="ArgumentNullException">Thrown when connectionString is null or empty.</exception>
        public static DbContextOptions<TDbContext> GetOptions(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            return new DbContextOptionsBuilder<TDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;
        }

        /// <summary>
        /// Gets the DbContext options for the specified connection string with custom migration table configuration.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="migrationTblName">The name of the migrations history table.</param>
        /// <param name="dbSchema">The schema for the migrations history table.</param>
        /// <returns>The configured DbContext options.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null or empty.</exception>
        public static DbContextOptions<TDbContext> GetOptions(string connectionString, string migrationTblName, string dbSchema)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(migrationTblName))
                throw new ArgumentNullException(nameof(migrationTblName));
            if (string.IsNullOrWhiteSpace(dbSchema))
                throw new ArgumentNullException(nameof(dbSchema));

            return new DbContextOptionsBuilder<TDbContext>()
                    .UseSqlServer(connectionString, builder =>
                    {
                        builder.MigrationsHistoryTable(migrationTblName, dbSchema);
                    })
                    .Options;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Validates all entities in the change tracker that are modified or added.
        /// </summary>
        /// <exception cref="ValidationException">Thrown when one or more entities fail validation.</exception>
        private void ValidateEntities()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(entry.Entity);
                
                if (!Validator.TryValidateObject(entry.Entity, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                    throw new ValidationException($"Validation failed for entity {entry.Entity.GetType().Name}: {errorMessages}");
                }
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// Validates all entities before saving.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="ValidationException">Thrown when one or more entities fail validation.</exception>
        public override int SaveChanges()
        {
            ValidateEntities();
            return base.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// Validates all entities before saving.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        /// <exception cref="ValidationException">Thrown when one or more entities fail validation.</exception>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ValidateEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        #endregion
    }
}
