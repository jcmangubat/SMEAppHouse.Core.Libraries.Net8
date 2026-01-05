using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.EF.Tests.Helpers;

namespace SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers;

/// <summary>
/// Test DbContext for unit testing
/// </summary>
public class TestDbContext : AppDbContextExtended<TestDbContext>
{
    /// <summary>
    /// Parameterless constructor required by AppDbContextExtended constraint.
    /// Uses in-memory database by default for testing.
    /// </summary>
    public TestDbContext() : base()
    {
        // Initialize with in-memory database for testing
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        DbContextOptions = options;
    }

    /// <summary>
    /// Constructor with options for explicit configuration
    /// </summary>
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; } = null!;
    public DbSet<TestAuditableEntity> TestAuditableEntities { get; set; } = null!;
    public DbSet<TestEntityWithGuid> TestEntitiesWithGuid { get; set; } = null!;
}

/// <summary>
/// Test entity for unit testing
/// </summary>
public class TestEntity : KeyedEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Test auditable entity for unit testing
/// </summary>
public class TestAuditableEntity : KeyedEntity<int>, IAuditableEntity
{
    public string Description { get; set; } = string.Empty;
    public bool? IsArchived { get; set; }
    public DateTime? DateArchived { get; set; }
    public string? ReasonArchived { get; set; }
}

/// <summary>
/// Test entity with validation attributes for unit testing
/// </summary>
public class TestEntityWithValidation : KeyedEntity<int>
{
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Test entity with Guid primary key for unit testing
/// </summary>
public class TestEntityWithGuid : KeyedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
}

