# SMEAppHouse.Core.Patterns.Repo

## Overview

`SMEAppHouse.Core.Patterns.Repo` is a comprehensive repository pattern library for implementing generic data composite modeling and repository pattern strategy. It provides generic repository implementations with support for Entity Framework Core, including CRUD operations, querying, paging, and includes retry logic for resilient data access.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.Patterns.Repo`

---

## Public Classes and Interfaces

### 1. Generic Repository

#### IRepositoryGeneric<TEntity, TPk>

Generic repository interface for any entity type with a primary key.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Abstractions`

**Key Properties:**
- `DbContext DbContext` - The database context
- `DbSet<TEntity> DbSet` - The entity set

**Key Methods:**

##### Querying

```csharp
IQueryable<TEntity> Query(string sql, params object[] parameters)
Task<TEntity> FindAsync(params object[] keyValues)
Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
    bool disableTracking = true)
Task<IEnumerable<TEntity>> GetListAsync(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    int fetchLimit = 0, bool disableTracking = true)
```

##### CRUD Operations

```csharp
Task AddAsync(TEntity entity)
Task AddAsync(params TEntity[] entities)
Task AddAsync(IEnumerable<TEntity> entities)
Task DeleteAsync(TEntity entity)
Task DeleteAsync(object id)
Task DeleteAsync(params TEntity[] entities)
Task DeleteAsync(IEnumerable<TEntity> entities)
Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
Task UpdateAsync(params TEntity[] entities)
Task UpdateAsync(IEnumerable<TEntity> entities)
Task CommitAsync()
```

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo.Abstractions;

public interface IUserRepository : IRepositoryGeneric<User, int>
{
}

// Implementation
public class UserRepository : RepositoryGeneric<User, int>, IUserRepository
{
    public UserRepository(DbContext dbContext) : base(dbContext) { }
}
```

---

#### RepositoryGeneric<TEntity, TPk>

Generic repository implementation for any entity type.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Abstractions`

**Implements**: `IRepositoryGeneric<TEntity, TPk>`

**Constructor:**
```csharp
public RepositoryGeneric(DbContext dbContext)
```

**Key Features:**
- Supports raw SQL queries
- Includes retry logic for resilient operations
- Supports includes, filtering, ordering, and tracking control
- Automatic change tracking management

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo.Abstractions;

public class UserRepository : RepositoryGeneric<User, int>
{
    public UserRepository(DbContext dbContext) : base(dbContext) { }
}

// Usage
var repository = new UserRepository(context);

// Find by ID
var user = await repository.FindAsync(1);

// Find with predicate
var activeUsers = await repository.FindAsync(u => u.IsActive == true);

// Get single with includes
var userWithOrders = await repository.GetSingleAsync(
    predicate: u => u.Id == 1,
    include: q => q.Include(u => u.Orders),
    disableTracking: false
);

// Get list with filtering and ordering
var users = await repository.GetListAsync(
    filter: u => u.IsActive == true,
    orderBy: q => q.OrderBy(u => u.Name),
    fetchLimit: 100,
    disableTracking: true
);

// Add
await repository.AddAsync(newUser);
await repository.CommitAsync();

// Update
await repository.UpdateAsync(updatedUser);
await repository.CommitAsync();

// Delete
await repository.DeleteAsync(user);
// or
await repository.DeleteAsync(1);
// or
await repository.DeleteAsync(u => u.IsActive == false);
await repository.CommitAsync();

// Raw SQL query
var users = repository.Query("SELECT * FROM Users WHERE IsActive = {0}", true);
```

---

### 2. Keyed Entity Repository

#### IRepositoryForKeyedEntity<TEntity, TPk>

Repository interface specifically for entities that implement `IKeyedEntity<TPk>`.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Abstractions`

**Inherits from**: `IDisposable`

**Key Properties:**
- `DbContext DbContext`
- `DbSet<TEntity> DbSet`

**Key Methods:**

##### Querying

```csharp
IQueryable<TEntity> Query(string sql, params object[] parameters)
Task<TEntity?> FindAsync(params object[] keyValues)
Task<TEntity?> FindAsync(Expression<Func<TEntity, object>> includeSelector, params object[] keyValues)
Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? predicate = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
    bool disableTracking = true)
Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter)
Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    bool disableTracking = true)
Task<PagedResult<TEntity, TPk>> GetListAsync(PageRequest pageRequest, Expression<Func<TEntity, bool>> filter)
Task<PagedResult<TEntity, TPk>> GetListAsync(PageRequest pageRequest,
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    bool disableTracking = true)
```

##### CRUD Operations

```csharp
Task AddAsync(TEntity entity)
Task AddAsync(params TEntity[] entities)
Task AddAsync(IEnumerable<TEntity> entities)
Task DeleteAsync(TEntity entity)
Task DeleteAsync(TPk id)
Task DeleteAsync(params TEntity[] entities)
Task DeleteAsync(IEnumerable<TEntity> entities)
Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
Task UpdateAsync(TEntity entity)
Task UpdateAsync(params TEntity[] entities)
Task UpdateAsync(IEnumerable<TEntity> entities)
Task CommitAsync()
```

---

#### RepositoryForKeyedEntity<TEntity, TPk>

Repository implementation for entities with primary keys.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Abstractions`

**Implements**: `IRepositoryForKeyedEntity<TEntity, TPk>`

**Constructor:**
```csharp
public RepositoryForKeyedEntity(DbContext dbContext)
```

**Key Features:**
- Type-safe primary key operations
- Built-in paging support
- Includes retry logic
- Supports includes, filtering, and ordering
- Automatic change tracking management

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo.Abstractions;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

public class User : KeyedEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserRepository : RepositoryForKeyedEntity<User, int>
{
    public UserRepository(DbContext dbContext) : base(dbContext) { }
}

// Usage
var repository = new UserRepository(context);

// Find by ID
var user = await repository.FindAsync(1);

// Find with includes
var userWithOrders = await repository.FindAsync(
    includeSelector: u => u.Orders,
    keyValues: 1
);

// Check existence
bool exists = await repository.AnyAsync(u => u.Email == "test@example.com");

// Get single
var user = await repository.GetSingleAsync(u => u.Email == "test@example.com");

// Get list
var users = await repository.GetListAsync(
    filter: u => u.IsActive == true,
    include: q => q.Include(u => u.Orders),
    orderBy: q => q.OrderBy(u => u.Name),
    disableTracking: true
);

// Get paged list
var pageRequest = new PageRequest { PageNo = 1, PageSize = 20 };
var pagedResult = await repository.GetListAsync(
    pageRequest,
    filter: u => u.IsActive == true,
    orderBy: q => q.OrderBy(u => u.Name)
);

Console.WriteLine($"Total Records: {pagedResult.TotalRecords}");
Console.WriteLine($"Total Pages: {pagedResult.TotalPages}");
foreach (var user in pagedResult.Data)
{
    Console.WriteLine(user.Name);
}

// Add
await repository.AddAsync(newUser);
await repository.CommitAsync();

// Update
await repository.UpdateAsync(updatedUser);
await repository.CommitAsync();

// Delete
await repository.DeleteAsync(user);
// or
await repository.DeleteAsync(1);
// or
await repository.DeleteAsync(u => u.IsActive == false);
await repository.CommitAsync();
```

---

### 3. Convenience Repositories

#### RepositoryForIntKeyedEntity<TEntity>

Convenience repository for entities with integer primary keys.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo`

**Inherits from**: `RepositoryForKeyedEntity<TEntity, int>`

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo;

public class User : KeyedEntity<int>
{
    public string Name { get; set; }
}

public class UserRepository : RepositoryForIntKeyedEntity<User>
{
    public UserRepository(DbContext dbContext) : base(dbContext) { }
}

// Usage
var repository = new UserRepository(context);
var user = await repository.FindAsync(1);
```

---

#### RepositoryForGuidKeyedEntity<TEntity>

Convenience repository for entities with Guid primary keys.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo`

**Inherits from**: `RepositoryForKeyedEntity<TEntity, Guid>`

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo;

public class Order : KeyedEntity<Guid>
{
    public string OrderNumber { get; set; }
}

public class OrderRepository : RepositoryForGuidKeyedEntity<Order>
{
    public OrderRepository(DbContext dbContext) : base(dbContext) { }
}

// Usage
var repository = new OrderRepository(context);
var orderId = Guid.NewGuid();
var order = await repository.FindAsync(orderId);
```

---

#### EntityRepositoryForGenericEntities<TEntity>

Convenience repository for generic entities with Guid primary keys.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo`

**Inherits from**: `RepositoryGeneric<TEntity, Guid>`

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo;

public class LogEntry
{
    public Guid Id { get; set; }
    public string Message { get; set; }
}

public class LogRepository : EntityRepositoryForGenericEntities<LogEntry>
{
    public LogRepository(DbContext dbContext) : base(dbContext) { }
}
```

---

### 4. Paging Support

#### PageRequest

Request model for pagination.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Paging`

**Implements**: `IPageRequest`

**Properties:**
- `int PageNo` - Page number (default: 1)
- `int PageSize` - Items per page (default: 10)

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo.Paging;

var pageRequest = new PageRequest 
{ 
    PageNo = 1, 
    PageSize = 20 
};
```

---

#### PagedResult<TEntity, TPk>

Response model for paginated data.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Paging`

**Inherits from**: `PagedResultBase`

**Properties:**
- `IEnumerable<TEntity> Data` - The paginated data

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo.Paging;

var result = await repository.GetListAsync(pageRequest, filter: u => u.IsActive == true);

Console.WriteLine($"Total Records: {result.TotalRecords}");
Console.WriteLine($"Total Pages: {result.TotalPages}");
Console.WriteLine($"Current Page: {result.PageRequest.PageNo}");
Console.WriteLine($"Page Size: {result.PageRequest.PageSize}");

foreach (var user in result.Data)
{
    Console.WriteLine(user.Name);
}
```

---

#### PagedResultBase

Base class for paginated results.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Paging`

**Implements**: `IPageResult`

**Properties:**
- `long TotalRecords` - Total number of records
- `int TotalPages` - Total number of pages
- `IPageRequest PageRequest` (required) - The page request

---

#### QueryableResult<TEntity>

Response model for paginated queryable data.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Paging`

**Inherits from**: `PagedResultBase`

**Properties:**
- `IQueryable<TEntity> Data` - The paginated queryable data

**Example:**
```csharp
var result = new QueryableResult<User>
{
    Data = context.Users.Where(u => u.IsActive == true),
    TotalRecords = 100,
    TotalPages = 5,
    PageRequest = pageRequest
};
```

---

#### IPageRequest

Interface for page requests.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Paging.Interface`

**Properties:**
- `int PageNo`
- `int PageSize`

---

#### IPageResult

Interface for page results.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.Paging.Interface`

**Properties:**
- `IPageRequest PageRequest`
- `long TotalRecords`
- `int TotalPages`

---

## Complete Usage Examples

### Example 1: Basic Repository Usage

```csharp
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.Repo.Abstractions;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

public class User : KeyedEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserRepository : RepositoryForKeyedEntity<User, int>
{
    public UserRepository(DbContext dbContext) : base(dbContext) { }
}

// Usage in service
public class UserService
{
    private readonly UserRepository _repository;
    
    public UserService(UserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        return await _repository.FindAsync(id);
    }
    
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _repository.GetListAsync(
            filter: u => u.IsActive == true,
            orderBy: q => q.OrderBy(u => u.Name)
        );
    }
    
    public async Task<User> CreateUserAsync(string name, string email)
    {
        var user = new User { Name = name, Email = email };
        await _repository.AddAsync(user);
        await _repository.CommitAsync();
        return user;
    }
    
    public async Task UpdateUserAsync(User user)
    {
        await _repository.UpdateAsync(user);
        await _repository.CommitAsync();
    }
    
    public async Task DeleteUserAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.CommitAsync();
    }
}
```

---

### Example 2: Paging

```csharp
using SMEAppHouse.Core.Patterns.Repo.Paging;

public class UserService
{
    private readonly UserRepository _repository;
    
    public async Task<PagedResult<User, int>> GetUsersPagedAsync(int pageNo, int pageSize)
    {
        var pageRequest = new PageRequest 
        { 
            PageNo = pageNo, 
            PageSize = pageSize 
        };
        
        return await _repository.GetListAsync(
            pageRequest,
            filter: u => u.IsActive == true,
            orderBy: q => q.OrderBy(u => u.Name),
            disableTracking: true
        );
    }
}

// Usage
var result = await userService.GetUsersPagedAsync(1, 20);

Console.WriteLine($"Page {result.PageRequest.PageNo} of {result.TotalPages}");
Console.WriteLine($"Showing {result.Data.Count()} of {result.TotalRecords} records");

foreach (var user in result.Data)
{
    Console.WriteLine($"{user.Id}: {user.Name}");
}
```

---

### Example 3: Repository with Includes

```csharp
public class Order : KeyedEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
}

public class OrderRepository : RepositoryForKeyedEntity<Order, int>
{
    public OrderRepository(DbContext dbContext) : base(dbContext) { }
}

// Get order with user
var order = await orderRepository.GetSingleAsync(
    predicate: o => o.Id == orderId,
    include: q => q.Include(o => o.User),
    disableTracking: false
);

Console.WriteLine($"Order: {order.OrderNumber}");
Console.WriteLine($"User: {order.User.Name}");
```

---

### Example 4: Bulk Operations

```csharp
// Bulk add
var users = new List<User>
{
    new User { Name = "User 1", Email = "user1@example.com" },
    new User { Name = "User 2", Email = "user2@example.com" },
    new User { Name = "User 3", Email = "user3@example.com" }
};

await repository.AddAsync(users);
await repository.CommitAsync();

// Bulk update
var usersToUpdate = await repository.GetListAsync(
    filter: u => u.IsActive == false
);
foreach (var user in usersToUpdate)
{
    user.IsActive = true;
}
await repository.UpdateAsync(usersToUpdate);
await repository.CommitAsync();

// Bulk delete
await repository.DeleteAsync(u => u.IsActive == false);
await repository.CommitAsync();
```

---

### Example 5: Raw SQL Queries

```csharp
// Execute raw SQL
var users = repository.Query(
    "SELECT * FROM Users WHERE IsActive = {0} AND CreatedDate > {1}",
    true,
    DateTime.UtcNow.AddDays(-30)
);

// Use with LINQ
var activeUsers = repository.Query(
    "SELECT * FROM Users WHERE IsActive = {0}",
    true
)
.Where(u => u.Email.Contains("@example.com"))
.OrderBy(u => u.Name)
.ToList();
```

---

### Example 6: Dependency Injection Setup

```csharp
using Microsoft.Extensions.DependencyInjection;
using SMEAppHouse.Core.Patterns.Repo.Abstractions;

// In Startup.cs or Program.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<MyDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    // Register repositories
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    
    // Or register generic repository factory
    services.AddScoped(typeof(IRepositoryGeneric<,>), typeof(RepositoryGeneric<,>));
}

// Usage in controller
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;
    
    public UsersController(IUserRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _repository.GetListAsync(
            filter: u => u.IsActive == true
        );
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _repository.FindAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        await _repository.AddAsync(user);
        await _repository.CommitAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}
```

---

## Key Features

1. **Generic Repository Pattern**: Type-safe repository implementations for any entity type
2. **Keyed Entity Support**: Specialized repositories for entities with primary keys
3. **Paging Support**: Built-in pagination with total records and page count
4. **Include Support**: Eager loading of related entities
5. **Retry Logic**: Automatic retry for resilient data access operations
6. **Change Tracking Control**: Optional change tracking for read-only operations
7. **Bulk Operations**: Support for bulk add, update, and delete operations
8. **Raw SQL Support**: Execute raw SQL queries when needed
9. **Async/Await**: All operations are asynchronous
10. **Disposable**: Proper resource disposal

---

## Dependencies

- Microsoft.EntityFrameworkCore (via Patterns.EF)
- System.Data.SqlClient (v4.8.6)
- SMEAppHouse.Core.CodeKits
- SMEAppHouse.Core.Patterns.EF

---

## Notes

- All repository operations are asynchronous
- Use `CommitAsync()` to persist changes to the database
- `disableTracking: true` is recommended for read-only operations to improve performance
- Bulk operations are more efficient than individual operations
- Retry logic is built-in for `GetListAsync` operations
- Repositories implement `IDisposable` and should be disposed properly
- Use dependency injection for repository lifecycle management

---

## License

Copyright © Nephiora IT Solutions 2025
