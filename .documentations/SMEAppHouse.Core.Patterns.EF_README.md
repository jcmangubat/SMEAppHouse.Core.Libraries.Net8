# SMEAppHouse.Core.Patterns.EF

## Overview

`SMEAppHouse.Core.Patterns.EF` is a comprehensive Entity Framework Core patterns library that provides abstractions, base classes, and utilities for implementing entity structural and adapter patterns. It simplifies and standardizes the configuration and management of EF Core DbContexts, entity mappings, and data access operations in .NET 8 applications.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.Patterns.EF`

---

## Public Classes and Interfaces

### 1. DbContext Abstractions

#### AppDbContextExtended<TDbContext>

Extended DbContext implementation that provides additional constructors and static helpers for creating DbContext instances with custom options, including migration table and schema configuration.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.DbContextAbstractions`

**Key Properties:**
- `DbContextOptions<TDbContext> DbContextOptions` - Gets or sets the DbContext options for this instance.

**Key Methods:**

##### Constructors

```csharp
public AppDbContextExtended()
public AppDbContextExtended(string connectionString)
public AppDbContextExtended(DbContextOptions<TDbContext> options)
```

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;

// Create using connection string
var context = new AppDbContextExtended<MyDbContext>("Server=...;Database=...;");

// Create using options
var options = new DbContextOptionsBuilder<MyDbContext>()
    .UseSqlServer(connectionString)
    .Options;
var context = new AppDbContextExtended<MyDbContext>(options);
```

##### Static Factory Methods

```csharp
public static TDbContext CreateDbContext(string connectionString)
public static TDbContext CreateDbContext(string connectionString, string migrationTblName, string dbSchema)
public static DbContextOptions<TDbContext> GetOptions(string connectionString)
public static DbContextOptions<TDbContext> GetOptions(string connectionString, string migrationTblName, string dbSchema)
```

**Example:**
```csharp
// Create with default migration table
var context = AppDbContextExtended<MyDbContext>.CreateDbContext(connectionString);

// Create with custom migration table and schema
var context = AppDbContextExtended<MyDbContext>.CreateDbContext(
    connectionString, 
    "MyMigrationsHistory", 
    "dbo"
);

// Get options for custom configuration
var options = AppDbContextExtended<MyDbContext>.GetOptions(connectionString);
```

##### Save Operations

```csharp
public override int SaveChanges()
public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
```

Both methods automatically validate all entities before saving using Data Annotations.

**Example:**
```csharp
var user = new User { Name = "John" };
context.Users.Add(user);

// Validates before saving
int saved = await context.SaveChangesAsync();
```

---

#### AppDbContextExt

Base DbContext implementation with automatic entity validation on save.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.DbContextAbstractions`

**Key Methods:**

```csharp
public override int SaveChanges()
public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
```

**Example:**
```csharp
public class MyDbContext : AppDbContextExt
{
    public MyDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}

// Usage
var options = new DbContextOptionsBuilder<MyDbContext>()
    .UseSqlServer(connectionString)
    .Options;
var context = new MyDbContext(options);
```

---

#### DbContextFactory<T>

Factory class for creating DbContext instances with configuration from IAppConfig.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.DbContextAbstractions`

**Key Methods:**

```csharp
public T CreateDbContext()
public T CreateDbContext(string connectionString)
```

**Example:**
```csharp
using Microsoft.Extensions.Configuration;
using SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces;

// Register in DI
services.AddScoped<IDbContextFactory<MyDbContext>>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var appConfig = sp.GetRequiredService<IAppConfig>();
    return new DbContextFactory<MyDbContext>(config, appConfig);
});

// Usage
var factory = serviceProvider.GetRequiredService<IDbContextFactory<MyDbContext>>();
var context = factory.CreateDbContext();
// or
var context = factory.CreateDbContext(customConnectionString);
```

---

#### IDbContextFactory<T>

Interface for creating DbContext instances.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.DbContextAbstractions`

**Methods:**
- `T CreateDbContext()` - Creates a new DbContext instance using configuration.
- `T CreateDbContext(string connectionString)` - Creates a new DbContext instance using the specified connection string.

---

### 2. Entity Configuration

#### DbEntityCfg<TEntity>

Abstract base class for configuring entity mappings using the Fluent API.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions`

**Key Methods:**

```csharp
public abstract void Map(EntityTypeBuilder<TEntity> entityBuilder)
```

**Example:**
```csharp
public class UserConfiguration : DbEntityCfg<User>
{
    public override void Map(EntityTypeBuilder<User> entityBuilder)
    {
        entityBuilder
            .ToTable("Users")
            .HasKey(u => u.Id);
        
        entityBuilder
            .Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        entityBuilder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
    }
}

// In OnModelCreating
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfiguration(new UserConfiguration());
}
```

---

#### EntityConfiguration<TEntity, TPk>

Abstract base class for configuring keyed entities with automatic convention setup.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions`

**Key Properties:**
- `string Schema` - Database schema name
- `Expression<Func<TEntity, object>>[] FieldsToIgnore` - Fields to ignore during configuration

**Key Methods:**

```csharp
public virtual void OnModelCreating(EntityTypeBuilder<TEntity> entityBuilder)
public virtual void OnModelCreating(ModelBuilder modelBuilder)
public void Configure(EntityTypeBuilder<TEntity> entityBuilder)
```

**Example:**
```csharp
public class ProductConfiguration : EntityConfiguration<Product, int>
{
    public ProductConfiguration() : base(schema: "catalog", pluralizeTblName: true)
    {
    }

    public override void OnModelCreating(EntityTypeBuilder<Product> entityBuilder)
    {
        base.OnModelCreating(entityBuilder);
        
        entityBuilder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        entityBuilder
            .Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}
```

---

#### EntityConfigurationAuditable<TEntity, TPk>

Abstract base class for configuring auditable entities with archive support.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions`

**Key Properties:**
- `string Schema` - Database schema name
- `bool Auditable` - Whether to include audit fields
- `Expression<Func<TEntity, object>>[] FieldsToIgnore` - Fields to ignore

**Example:**
```csharp
public class OrderConfiguration : EntityConfigurationAuditable<Order, Guid>
{
    public OrderConfiguration() : base(schema: "sales", auditable: true)
    {
    }

    public override void OnModelCreating(EntityTypeBuilder<Order> entityBuilder)
    {
        base.OnModelCreating(entityBuilder);
        
        entityBuilder
            .Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);
    }
}
```

---

### 3. Entity Base Classes

#### KeyedEntity<TPk>

Base class for entities with a primary key and standard audit fields.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base`

**Key Properties:**
- `TPk Id` - Primary key (auto-generated)
- `bool? IsActive` - Active status flag
- `DateTime DateCreated` - Creation timestamp (UTC)
- `DateTime? DateModified` - Last modification timestamp (UTC)

**Key Methods:**
- `void VerifyPropertyName(string propertyName)` - Debug helper to verify property names
- `IEnumerable<Type> GetImplementors()` - Gets all types implementing IKeyedEntity<TPk>
- `Type GetEntityIdentificationType()` - Gets the primary key type

**Example:**
```csharp
public class User : KeyedEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// Usage
var user = new User 
{ 
    Name = "John Doe", 
    Email = "john@example.com" 
    // Id, DateCreated, IsActive are automatically set
};
```

---

#### KeyedAuditableEntity<TPk>

Base class for entities with primary key, audit fields, and archive support.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base`

**Inherits from**: `KeyedEntity<TPk>`

**Additional Properties:**
- `bool? IsArchived` - Archive status flag
- `DateTime? DateArchived` - Archive timestamp
- `string? ReasonArchived` - Reason for archiving

**Example:**
```csharp
public class Order : KeyedAuditableEntity<Guid>
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
}

// Usage
var order = new Order 
{ 
    OrderNumber = "ORD-001",
    TotalAmount = 100.00m
};

// Archive the order
order.IsArchived = true;
order.DateArchived = DateTime.UtcNow;
order.ReasonArchived = "Customer cancelled";
```

---

#### GuidKeyedAuditableModel

DTO model with Guid primary key and audit fields.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.DtoModelAbstraction`

**Properties:**
- `Guid Id` (required)
- `bool? IsActive`
- `DateTime DateCreated`
- `DateTime? DateModified`
- `bool? IsArchived`
- `DateTime? DateArchived`
- `string? ReasonArchived`

**Example:**
```csharp
var model = new GuidKeyedAuditableModel
{
    Id = Guid.NewGuid(),
    IsActive = true,
    DateCreated = DateTime.UtcNow
};
```

---

### 4. Entity Interfaces

#### IEntity

Base interface for all entities with common audit fields.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces`

**Properties:**
- `bool? IsActive`
- `DateTime DateCreated`
- `DateTime? DateModified`

---

#### IKeyedEntity<TPk>

Interface for entities with a primary key.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces`

**Inherits from**: `IEntity`

**Properties:**
- `TPk Id`

---

#### IKeyedAuditableEntity<TPk>

Interface for entities with primary key and archive support.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces`

**Inherits from**: `IKeyedEntity<TPk>, IAuditableEntity`

---

#### IAuditableEntity

Interface for entities with archive support.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces`

**Properties:**
- `bool? IsArchived`
- `DateTime? DateArchived`
- `string? ReasonArchived`

---

### 5. Paging Support

> **⚠️ Important Note**: The Paging classes are currently excluded from compilation in the project file (`<Compile Remove="Paging\**" />`). These classes exist in the source code but are not included in the compiled library. To use paging functionality, you may need to remove this exclusion from the `.csproj` file or use alternative paging approaches.

#### PageRequest

Request model for pagination.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Paging`

**Properties:**
- `int Page` - Page number (default: 1)
- `int PageSize` - Items per page (default: 10)

**Note**: This class does not implement `IPageRequest` interface, but has matching properties.

**Example:**
```csharp
var pageRequest = new PageRequest 
{ 
    Page = 1, 
    PageSize = 20 
};
```

---

#### PagedResponse<TEntity>

Response model for paginated data.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Paging`

**Inherits from**: `PagedResponseBase`

**Properties:**
- `IEnumerable<TEntity> Data` - The paginated data

**Example:**
```csharp
var response = new PagedResponse<User>
{
    Data = users,
    PageRequest = pageRequest,
    TotalRecords = 100,
    TotalPages = 5
};
```

---

#### QueryableResponse<TEntity>

Response model for paginated queryable data.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Paging`

**Inherits from**: `PagedResponseBase`

**Properties:**
- `IQueryable<TEntity> Data` - The paginated queryable data

**Example:**
```csharp
var response = new QueryableResponse<User>
{
    Data = context.Users.Where(u => u.IsActive == true),
    PageRequest = pageRequest,
    TotalRecords = 100,
    TotalPages = 5
};
```

---

#### PagedResponseBase

Base class for paginated responses.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Paging`

**Implements**: `IPageResponse`

**Properties:**
- `IPageRequest PageRequest` - The page request
- `long TotalRecords` - Total number of records
- `int TotalPages` - Total number of pages

---

#### IPageRequest

Interface for page requests.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Paging.Interface`

**Properties:**
- `int Page`
- `int PageSize`

---

#### IPageResponse

Interface for page responses.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Paging.Interface`

**Properties:**
- `IPageRequest PageRequest`
- `long TotalRecords`
- `int TotalPages`

---

### 6. Exceptions

#### EntityNotFoundException<TEntity>

Exception thrown when an entity is not found.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Exceptions`

**Constructors:**
```csharp
public EntityNotFoundException()
public EntityNotFoundException(string message)
public EntityNotFoundException(string message, Exception innerException)
public EntityNotFoundException(object id)
```

**Example:**
```csharp
var user = await context.Users.FindAsync(userId);
if (user == null)
    throw new EntityNotFoundException<User>(userId);
```

---

### 7. Migration Configuration

#### IDbMigrationInformation

Interface for providing migration table and schema information.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.SettingsModel`

**Properties:**
- `string MigrationTblName` - Name of the migrations history table
- `string DbSchema` - Database schema for the migrations table

---

#### DbMigrationInformation

Default implementation of IDbMigrationInformation.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.SettingsModel`

**Properties:**
- `string MigrationTblName` (required)
- `string DbSchema` (default: "dbo")

**Example:**
```csharp
var migrationInfo = new DbMigrationInformation
{
    MigrationTblName = "MyMigrationsHistory",
    DbSchema = "dbo"
};
```

---

### 8. Extension Methods

#### DataAccessOperationsExtensions

Extension methods for data access operations.

**Namespace**: `SMEAppHouse.Core.Patterns.EF.Helpers`

**Key Methods:**

##### GetPrimaryKeyProperties<T>

```csharp
public static IReadOnlyList<IProperty> GetPrimaryKeyProperties<T>(this DbContext dbContext)
```

Gets the primary key properties for the specified entity type.

**Example:**
```csharp
var keyProperties = context.GetPrimaryKeyProperties<User>();
// Returns list of primary key properties
```

##### FilterByPrimaryKeyPredicate<T>

```csharp
public static Expression<Func<T, bool>> FilterByPrimaryKeyPredicate<T>(
    this DbContext dbContext, 
    object[] id)
```

Creates a predicate expression that filters entities by their primary key values.

**Example:**
```csharp
var predicate = context.FilterByPrimaryKeyPredicate<User>(new object[] { 1 });
var user = context.Users.Where(predicate).FirstOrDefault();
```

##### FilterByPrimaryKey<TEntity>

```csharp
public static IQueryable<TEntity> FilterByPrimaryKey<TEntity>(
    this DbSet<TEntity> dbSet, 
    DbContext context, 
    object[] id)
```

Filters a DbSet by primary key values.

**Example:**
```csharp
var users = context.Users.FilterByPrimaryKey(context, new object[] { 1 });
```

##### DetachLocal<TEntity, TPk>

```csharp
public static void DetachLocal<TEntity, TPk>(
    this DbContext context, 
    TEntity entity, 
    string entryId)
```

Detaches a local entity from the context and attaches the provided entity as modified.

**Example:**
```csharp
context.DetachLocal<User, int>(updatedUser, "1");
context.SaveChanges();
```

##### Paging<TEntity>

```csharp
public static IQueryable<TEntity> Paging<TEntity>(
    this IQueryable<TEntity> query, 
    int pageSize = 0, 
    int pageNumber = 0)
    where TEntity : class, IEntity
```

Applies paging to a queryable collection. **Note**: This method requires entities to implement `IEntity` interface.

**Example:**
```csharp
var pagedUsers = context.Users
    .Where(u => u.IsActive == true)
    .Paging(pageSize: 20, pageNumber: 1);
```

---

## Complete Usage Examples

### Example 1: Setting Up a DbContext

```csharp
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;

public class MyDbContext : AppDbContextExtended<MyDbContext>
{
    public MyDbContext(string connectionString) : base(connectionString) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
    }
}

// Usage
var connectionString = "Server=...;Database=...;";
var context = new MyDbContext(connectionString);
```

---

### Example 2: Creating Entities with Base Classes

```csharp
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

public class User : KeyedEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class Order : KeyedAuditableEntity<Guid>
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

// Usage
var user = new User 
{ 
    Name = "John Doe", 
    Email = "john@example.com" 
};

var order = new Order 
{ 
    OrderNumber = "ORD-001",
    TotalAmount = 100.00m,
    UserId = user.Id
};

context.Users.Add(user);
context.Orders.Add(order);
await context.SaveChangesAsync();
```

---

### Example 3: Entity Configuration

```csharp
using SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions;

public class UserConfiguration : EntityConfiguration<User, int>
{
    public UserConfiguration() : base(schema: "app", pluralizeTblName: true)
    {
    }

    public override void OnModelCreating(EntityTypeBuilder<User> entityBuilder)
    {
        base.OnModelCreating(entityBuilder);
        
        entityBuilder
            .Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        entityBuilder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
```

---

### Example 4: Paging

```csharp
using SMEAppHouse.Core.Patterns.EF.Paging;
using SMEAppHouse.Core.Patterns.EF.Helpers;

// Create page request
var pageRequest = new PageRequest { Page = 1, PageSize = 20 };

// Query with paging
var query = context.Users
    .Where(u => u.IsActive == true)
    .OrderBy(u => u.Name);

var totalRecords = await query.CountAsync();
var pagedQuery = query.Paging(pageRequest.PageSize, pageRequest.Page);
var users = await pagedQuery.ToListAsync();

// Create response
var response = new PagedResponse<User>
{
    Data = users,
    PageRequest = pageRequest,
    TotalRecords = totalRecords,
    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageRequest.PageSize)
};
```

---

### Example 5: Using DbContextFactory

```csharp
using Microsoft.Extensions.DependencyInjection;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;

// Register in Startup.cs or Program.cs
services.AddScoped<IDbContextFactory<MyDbContext>>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var appConfig = sp.GetRequiredService<IAppConfig>();
    return new DbContextFactory<MyDbContext>(config, appConfig);
});

// Usage in service
public class UserService
{
    private readonly IDbContextFactory<MyDbContext> _factory;
    
    public UserService(IDbContextFactory<MyDbContext> factory)
    {
        _factory = factory;
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        using var context = _factory.CreateDbContext();
        return await context.Users.FindAsync(id);
    }
}
```

---

### Example 6: Custom Migration Table

```csharp
// Create context with custom migration table
var context = AppDbContextExtended<MyDbContext>.CreateDbContext(
    connectionString,
    "MyMigrationsHistory",
    "dbo"
);

// Or using DbContextFactory with IAppConfig
// Note: AppConfig is abstract, so you need to create a concrete implementation
public class MyAppConfig : SMEAppHouse.Core.AppMgt.AppCfgs.Base.AppConfig
{
    public MyAppConfig()
    {
        AppEFBehaviorAttributes = new SMEAppHouse.Core.AppMgt.AppCfgs.Base.AppEFBehaviorAttributes
        {
            MigrationTblName = "MyMigrationsHistory",
            DbSchema = "dbo"
        };
    }

    public override void Validate()
    {
        // Add validation logic here
    }
}
```

---

### Example 7: Error Handling

```csharp
using SMEAppHouse.Core.Patterns.EF.Exceptions;

public async Task<User> GetUserOrThrowAsync(int id)
{
    var user = await context.Users.FindAsync(id);
    if (user == null)
        throw new EntityNotFoundException<User>(id);
    return user;
}
```

---

## Dependencies

- Microsoft.EntityFrameworkCore.Abstractions (v9.0.9)
- Microsoft.EntityFrameworkCore.Analyzers (v9.0.9)
- Microsoft.EntityFrameworkCore.Relational (v9.0.9)
- Microsoft.EntityFrameworkCore.SqlServer (v9.0.9)
- Microsoft.Extensions.Configuration.Abstractions (v9.0.9)
- SMEAppHouse.Core.CodeKits
- SMEAppHouse.Core.AppMgt

---

## Notes

- All date/time fields are stored as UTC and converted automatically
- Entity validation is performed automatically on SaveChanges/SaveChangesAsync
- Primary keys are auto-generated by default
- Table names are pluralized by default in EntityConfiguration
- Archive functionality is available through KeyedAuditableEntity
- Extension methods provide convenient query operations
- Migration table and schema can be customized per context

---

## License

Copyright © Nephiora IT Solutions 2025
