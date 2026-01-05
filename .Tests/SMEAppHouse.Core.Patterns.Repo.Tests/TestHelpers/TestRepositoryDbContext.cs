using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

namespace SMEAppHouse.Core.Patterns.Repo.Tests.TestHelpers;

public class TestRepositoryDbContext : AppDbContextExtended<TestRepositoryDbContext>
{
    public TestRepositoryDbContext() : base()
    {
        var options = new DbContextOptionsBuilder<TestRepositoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        DbContextOptions = options;
    }

    public TestRepositoryDbContext(DbContextOptions<TestRepositoryDbContext> options) : base(options)
    {
    }

    public DbSet<TestUser> Users { get; set; } = null!;
    public DbSet<TestOrder> Orders { get; set; } = null!;
}

public class TestUser : KeyedEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public new bool IsActive { get; set; } = true;
    public List<TestOrder> Orders { get; set; } = new();
}

public class TestOrder : KeyedEntity<int>
{
    public int UserId { get; set; }
    public TestUser User { get; set; } = null!;
    public string ProductName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime OrderDate { get; set; }
}

