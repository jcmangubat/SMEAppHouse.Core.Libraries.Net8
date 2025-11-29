using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.EF.DbContextAbstractions
{
    /// <summary>
    /// Base DbContext implementation with automatic entity validation on save.
    /// </summary>
    public class AppDbContextExt : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContextExt"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
        public AppDbContextExt(DbContextOptions options)
            : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
        }

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
    }
}