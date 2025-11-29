# SMEAppHouse.Core.Patterns.Repo.V2

## Overview

`SMEAppHouse.Core.Patterns.Repo.V2` is an enhanced version of the repository pattern library. It provides both synchronous and asynchronous repository implementations with improved API design, auto-save options, and comprehensive querying capabilities including paging, filtering, and raw SQL execution.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2`

---

## Public Classes and Interfaces

### 1. Repository Interfaces

#### IRepository<TEntity, TPk>

Main repository interface with async operations.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2.Base`

**Key Properties:**
- `DbContext Context`
- `DbSet<TEntity> DbSet`

**Key Methods:**

##### CRUD Operations

```csharp
Task<TEntity> CreateAsync(TEntity entity, bool autoSave = false)
Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
Task RemoveAsync(TPk id, bool autoSave = false)
Task RemoveAsync(TEntity entity, bool autoSave = false)
Task RemoveAsync(Expression<Func<TEntity, bool>> filter, bool autoSave = false)
Task RefreshAsync(TEntity entity)
Task SaveAsync()
```

##### Query Operations

```csharp
Task<TEntity> GetAsync(TPk id)
Task<TEntity> GetAsync(TPk id, string includeProperties)
Task<IEnumerable<TEntity>> GetAllAsync()
Task<IEnumerable<TEntity>> GetAllAsync(string includeProperties)
Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit)
Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit, string includeProperties)
```

##### Paging Operations

```csharp
Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take)
Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter)
Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize)
Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter)
Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
```

##### SQL Operations

```csharp
Task<IEnumerable<TEntity>> GetWithSqlAsync(string query, params object[] parameters)
Task<IEnumerable<TEntity>> GetWithSqlAsync(string query)
Task<int> ExecuteNonQueryAsync(string query, params object[] parameters)
Task<string> ExecuteQueryAsync(string query, params object[] parameters)
Task<T> ExecuteSqlAsync<T>(string query, params object[] parameters)
Task<T> ExecuteSqlAsync<T>(string query, string readField, params object[] parameters)
```

##### Count and Existence

```csharp
int Count()
int Count(Expression<Func<TEntity, bool>> filter)
bool Any()
bool Any(Expression<Func<TEntity, bool>> filter)
```

---

#### IRepositorySync<TEntity, TPk>

Synchronous repository interface.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2.Base`

Similar to `IRepository` but with synchronous methods (no `Async` suffix).

**Key Methods:**
```csharp
TEntity Create(TEntity entity, bool autoSave = false)
void Update(TEntity entity, bool autoSave = false)
void Remove(TPk id, bool autoSave = false)
void Remove(TEntity entity, bool autoSave = false)
void Remove(Expression<Func<TEntity, bool>> filter, bool autoSave = false)
void Refresh(TEntity entity)
void Save()
TEntity Get(TPk id)
TEntity Get(TPk id, string includeProperties)
IEnumerable<TEntity> GetAll()
// ... (similar synchronous versions of async methods)
```

---

#### IRepositoryAsync<TEntity, TPk>

Async-only repository interface (alias for IRepository).

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2.Base`

---

### 2. Repository Implementations

#### RepositoryBaseSync<TEntity, TPk, TDbContext>

Synchronous repository base implementation.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2.Base`

**Implements**: `IRepositorySync<TEntity, TPk>`, `IDisposable`

**Constructors:**
```csharp
public RepositoryBaseSync()
public RepositoryBaseSync(TDbContext context)
```

**Key Features:**
- Retry logic for resilient operations
- Auto-save option for immediate persistence
- Support for includes, filtering, ordering
- Paging support
- Raw SQL execution

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.Repo.V2.Base;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

public class User : KeyedEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserRepository : RepositoryBaseSync<User, int, MyDbContext>
{
    public UserRepository(MyDbContext context) : base(context) { }
}

// Usage
var repository = new UserRepository(context);

// Create with auto-save
var user = repository.Create(new User { Name = "John", Email = "john@example.com" }, autoSave: true);

// Get by ID
var found = repository.Get(1);

// Get with includes
var userWithOrders = repository.Get(1, "Orders");

// Get all with filtering
var activeUsers = repository.GetAll(u => u.IsActive == true);

// Get paged
var page1 = repository.GetPage(0, 20, u => u.IsActive == true, q => q.OrderBy(u => u.Name));

// Count
int total = repository.Count(u => u.IsActive == true);

// Execute raw SQL
var users = repository.GetWithSql("SELECT * FROM Users WHERE IsActive = 1");
```

---

#### RepositoryBaseAsync<TEntity, TPk, TDbContext>

Async repository base implementation (currently throws `NotImplementedException` - use sync version).

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2.Base`

**Implements**: `IRepositoryAsync<TEntity, TPk>`, `IDisposable`

---

### 3. Unit of Work

#### IUnitOfWork

Unit of Work pattern interface for managing repositories and transactions.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2.UnitOfWork`

**Key Methods:**
```csharp
IRepositorySync<TEntity, TPk> GetRepository<TEntity, TPk>()
void Save()
```

**Example:**
```csharp
using (var unitOfWork = new UnitOfWorkBase(context))
{
    var userRepo = unitOfWork.GetRepository<User, int>();
    var orderRepo = unitOfWork.GetRepository<Order, int>();

    var user = userRepo.Create(new User { Name = "John" });
    var order = orderRepo.Create(new Order { UserId = user.Id, Total = 100 });

    // Save all changes in one transaction
    unitOfWork.Save();
}
```

---

#### UnitOfWorkBase

Base implementation of Unit of Work pattern.

**Namespace**: `SMEAppHouse.Core.Patterns.Repo.V2.UnitOfWork`

**Implements**: `IUnitOfWork`, `IDisposable`

---

## Complete Usage Examples

### Example 1: Basic Repository Usage

```csharp
using SMEAppHouse.Core.Patterns.Repo.V2.Base;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

public class Product : KeyedEntity<int>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}

public class ProductRepository : RepositoryBaseSync<Product, int, MyDbContext>
{
    public ProductRepository(MyDbContext context) : base(context) { }
}

// Usage
var repo = new ProductRepository(context);

// Create
var product = repo.Create(new Product 
{ 
    Name = "Laptop", 
    Price = 999.99m,
    IsActive = true
}, autoSave: true);

// Update
product.Price = 899.99m;
repo.Update(product, autoSave: true);

// Get by ID
var laptop = repo.Get(product.Id);

// Get all active
var activeProducts = repo.GetAll(p => p.IsActive == true);

// Delete
repo.Remove(product.Id, autoSave: true);
```

---

### Example 2: Advanced Querying

```csharp
public class OrderRepository : RepositoryBaseSync<Order, int, MyDbContext>
{
    public OrderRepository(MyDbContext context) : base(context) { }
}

var repo = new OrderRepository(context);

// Get with includes
var orders = repo.GetAll(
    filter: o => o.TotalAmount > 100,
    orderBy: q => q.OrderByDescending(o => o.OrderDate),
    fetchLimit: 50,
    includeProperties: "User,OrderItems"
);

// Paging
var page1 = repo.GetPage(
    pageNo: 0,
    pageSize: 20,
    filter: o => o.IsActive == true,
    orderBy: q => q.OrderBy(o => o.OrderDate),
    includeProperties: "User"
);

// Skip/Take
var next10 = repo.GetAll(
    skip: 20,
    take: 10,
    filter: o => o.TotalAmount > 50,
    orderBy: q => q.OrderBy(o => o.OrderDate)
);
```

---

### Example 3: Raw SQL Operations

```csharp
var repo = new UserRepository(context);

// Execute query returning entities
var users = repo.GetWithSql(
    "SELECT * FROM Users WHERE IsActive = {0} AND CreatedDate > {1}",
    true,
    DateTime.UtcNow.AddDays(-30)
);

// Execute non-query
int affected = repo.ExecuteNonQuery(
    "UPDATE Users SET IsActive = 0 WHERE LastLoginDate < {0}",
    DateTime.UtcNow.AddMonths(-6)
);

// Execute scalar query
string result = repo.ExecuteQuery(
    "SELECT COUNT(*) FROM Users WHERE IsActive = 1"
);

// Execute typed SQL
int count = repo.ExecuteSql<int>(
    "SELECT COUNT(*) FROM Users"
);

// Execute with field name
decimal total = repo.ExecuteSql<decimal>(
    "SELECT SUM(TotalAmount) FROM Orders",
    "TotalAmount"
);
```

---

### Example 4: Unit of Work Pattern

```csharp
using SMEAppHouse.Core.Patterns.Repo.V2.UnitOfWork;

public class OrderService
{
    public void CreateOrderWithItems(Order order, List<OrderItem> items)
    {
        using (var unitOfWork = new UnitOfWorkBase(context))
        {
            var orderRepo = unitOfWork.GetRepository<Order, int>();
            var itemRepo = unitOfWork.GetRepository<OrderItem, int>();

            // Create order
            var createdOrder = orderRepo.Create(order);

            // Create items
            foreach (var item in items)
            {
                item.OrderId = createdOrder.Id;
                itemRepo.Create(item);
            }

            // Save all in one transaction
            unitOfWork.Save();
        }
    }
}
```

---

### Example 5: Auto-Save vs Manual Save

```csharp
var repo = new UserRepository(context);

// Auto-save (immediate persistence)
var user1 = repo.Create(new User { Name = "User1" }, autoSave: true);
// User1 is immediately saved to database

// Manual save (batch operations)
var user2 = repo.Create(new User { Name = "User2" }, autoSave: false);
var user3 = repo.Create(new User { Name = "User3" }, autoSave: false);
var user4 = repo.Create(new User { Name = "User4" }, autoSave: false);

// Save all at once
repo.Save(); // All three users saved in one transaction
```

---

## Key Features

1. **Dual API**: Both synchronous and asynchronous interfaces
2. **Auto-Save Option**: Optional immediate persistence on operations
3. **Comprehensive Querying**: Filtering, ordering, paging, includes
4. **Raw SQL Support**: Execute custom SQL queries
5. **Retry Logic**: Built-in retry for resilient operations
6. **Unit of Work**: Transaction management pattern
7. **Type Safety**: Strongly-typed repository operations
8. **Paging Support**: Multiple paging methods (GetPage, Skip/Take)

---

## Dependencies

- Microsoft.EntityFrameworkCore (via Patterns.EF)
- System.Data.SqlClient
- SMEAppHouse.Core.CodeKits
- SMEAppHouse.Core.Patterns.EF

---

## Notes

- Use `autoSave: true` for immediate persistence
- Use `autoSave: false` for batch operations and call `Save()` manually
- Include properties are comma-separated strings (e.g., "Orders,User")
- Page numbers are 0-based in `GetPage` method
- Raw SQL uses parameterized queries for safety
- Retry logic is built into query operations
- Unit of Work manages transactions across multiple repositories
- Async repository implementation is not yet complete (throws NotImplementedException)

---

## License

Copyright © Nephiora IT Solutions 2025
