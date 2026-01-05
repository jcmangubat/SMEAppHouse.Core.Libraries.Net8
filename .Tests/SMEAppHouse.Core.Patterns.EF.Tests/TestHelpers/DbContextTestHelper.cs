using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers;

/// <summary>
/// Helper class for creating test DbContext instances
/// </summary>
public static class DbContextTestHelper
{
    /// <summary>
    /// Creates an in-memory test DbContext
    /// </summary>
    public static TestDbContext CreateInMemoryContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new TestDbContext(options);
    }

    /// <summary>
    /// Creates a test DbContext with SQL Server connection string (for integration tests)
    /// </summary>
    public static TestDbContext CreateSqlServerContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new TestDbContext(options);
    }
}

