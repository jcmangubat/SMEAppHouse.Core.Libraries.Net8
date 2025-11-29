# SMEAppHouse.Core.Patterns.WebApi

## Overview

`SMEAppHouse.Core.Patterns.WebApi` is a patterns library for implementing Web API services and clients. It provides base classes and interfaces for creating RESTful API controllers (hosts) and API clients with standardized CRUD operations, error handling, and JSON serialization.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.Patterns.WebApi`

---

## Public Classes and Interfaces

### 1. API Host Pattern (Server-Side)

#### IWebApiServiceHost<TEntity, TPk>

Interface for Web API service hosts that provide CRUD operations.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.APIHostPattern`

**Key Properties:**
- `IRepositoryForKeyedEntity<TEntity, TPk> Repository` - Reference to the entity's data repository

**Key Methods:**

##### CRUD Operations

```csharp
Task<IActionResult> CreateSingleAsync(TEntity entity)
Task<IActionResult> CreateManyAsync(List<TEntity> entities)
Task<IActionResult> CreateFromJsonAsync(object jsonOfEntity)
Task<IActionResult> UpdateSingleAsync(TEntity entity)
Task<IActionResult> UpdateManyAsync(List<TEntity> entities)
Task<IActionResult> UpdateFromJsonAsync(object jsonOfEntity)
Task<IActionResult> RemoveByIdAsync(TPk id)
Task<IActionResult> RemoveManyAsync(TPk[] ids)
Task<IActionResult> ZapAsync() // Remove all
```

##### Query Operations

```csharp
Task<IActionResult> CountAsync()
Task<IActionResult> GetByIdAsync(TPk id)
Task<IActionResult> GetAllAsync()
Task<IActionResult> GetAndIncludeAsync(string entitiesToInclude)
Task<IActionResult> GetAndConditionalAsync(string whereStr, TEntity entityParam)
```

##### Utility Methods

```csharp
string GetHeader(string headerName)
DateTime GetLocalTz(DateTime utcDateTime, int timeZoneOffset)
DateTime ToClientTz(DateTime utcDateTime, int timeZoneOffset)
```

---

#### WebApiServiceHost<TEntity, TPk>

Base class for Web API controllers implementing CRUD operations.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.APIHostPattern`

**Inherits from**: `WebApiServiceHostExt`

**Implements**: `IWebApiServiceHost<TEntity, TPk>`

**Attributes:**
- `[EnableCors("SiteCorsPolicy")]` - Enables CORS
- `[Route("api/v{version:apiVersion}/[controller]")]` - API versioning route

**Constructor:**
```csharp
protected WebApiServiceHost(IRepositoryForKeyedEntity<TEntity, TPk> repository)
```

**Key Features:**
- Automatic error handling via `ExecuteAsync`
- Standardized HTTP status codes
- JSON serialization/deserialization
- CORS support
- API versioning support

**Example:**
```csharp
using Microsoft.AspNetCore.Mvc;
using SMEAppHouse.Core.Patterns.WebApi.APIHostPattern;
using SMEAppHouse.Core.Patterns.Repo.Abstractions;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

public class User : KeyedEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

[ApiController]
[ApiVersion("1.0")]
public class UsersController : WebApiServiceHost<User, int>
{
    public UsersController(IRepositoryForKeyedEntity<User, int> repository) 
        : base(repository)
    {
    }
}

// Available endpoints:
// POST   /api/v1/Users/CreateSingle
// POST   /api/v1/Users/CreateFromJson
// PUT    /api/v1/Users/UpdateSingle
// PUT    /api/v1/Users/UpdateFromJson
// DELETE /api/v1/Users/RemoveById?id=1
// DELETE /api/v1/Users/RemoveMany
// DELETE /api/v1/Users/Zap
// GET    /api/v1/Users/Count
// GET    /api/v1/Users/GetById?id=1
// GET    /api/v1/Users/GetAll
```

---

#### WebApiServiceHostExt

Abstract base class for Web API service hosts.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.APIHostPattern`

**Inherits from**: `ControllerBase`

**Key Methods:**
```csharp
protected abstract Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> executeActionAsync)
```

Provides error handling wrapper for async operations.

---

#### HttpActionResultWrapper<TEntity>

Wrapper for HTTP action results with entity serialization.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.APIHostPattern`

**Implements**: `IActionResult`

**Constructor:**
```csharp
public HttpActionResultWrapper(TEntity value, HttpRequestMessage request)
```

**Example:**
```csharp
var wrapper = new HttpActionResultWrapper<User>(user, request);
return wrapper;
```

---

### 2. API Client Pattern (Client-Side)

#### IWebApiServiceClient<TEntity, TPk>

Interface for Web API service clients.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.APIClientPattern`

**Key Properties:**
- `HttpClient HttpClient` - HTTP client for making requests
- `string BaseServiceAddress` - Base URL of the API service

**Key Methods:**

```csharp
TEntity Create(TEntity entity)
TEntity Update(TEntity entity)
void RemoveById(TPk id)
void RemoveAll()
int Count()
TEntity GetById(TPk id)
IEnumerable<TEntity> GetAll()
IEnumerable<TEntity> GetAllWithEntities(params string[] entities)
```

---

#### WebApiServiceClientBase<TEntity, TIdType>

Abstract base class for Web API service clients implementing the Template Method pattern.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.APIClientPattern`

**Implements**: `IWebApiServiceClient<TEntity, TIdType>`

**Key Properties:**
- `HttpClient HttpClient`
- `string BaseServiceAddress`
- `string ClientNameUrl` - Auto-generated URL based on class name (removes "Client" suffix)

**Protected Properties (Service URLs):**
- `string ServiceUrlForCreate` - `{ClientNameUrl}/Create`
- `string ServiceUrlForUpdate` - `{ClientNameUrl}/Update`
- `string ServiceUrlForRemoveById` - `{ClientNameUrl}/RemoveById?id=[id]`
- `string ServiceUrlForRemoveAll` - `{ClientNameUrl}/RemoveAll`
- `string ServiceUrlForCount` - `{ClientNameUrl}/Count`
- `string ServiceUrlForGetById` - `{ClientNameUrl}/GetById?id=[id]`
- `string ServiceUrlForGetAll` - `{ClientNameUrl}/GetAll`
- `string ServiceUrlForGetAllWithEntities` - `{ClientNameUrl}/GetAll?entitiesToInclude=[entitiesToInclude]`

**Constructors:**
```csharp
protected WebApiServiceClientBase(string baseSrvcAddress)
protected WebApiServiceClientBase(HttpClient client, string baseSrvcAddress)
```

**Example:**
```csharp
using SMEAppHouse.Core.Patterns.WebApi.APIClientPattern;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

public class User : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool? IsActive { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}

public class UserServiceClient : WebApiServiceClientBase<User, int>
{
    public UserServiceClient(string baseServiceAddress) 
        : base(baseServiceAddress)
    {
        // ClientNameUrl will be "api/v1/User" (removes "Client" from class name)
    }
}

// Usage
var client = new UserServiceClient("https://api.example.com");

// Create
var newUser = new User { Name = "John", Email = "john@example.com" };
var created = client.Create(newUser);

// Get by ID
var user = client.GetById(1);

// Get all
var allUsers = client.GetAll();

// Get all with related entities
var usersWithOrders = client.GetAllWithEntities("Orders", "Addresses");

// Update
user.Name = "John Updated";
var updated = client.Update(user);

// Count
int count = client.Count();

// Delete
client.RemoveById(1);
```

---

### 3. Exceptions

#### HttpActionException

Exception for HTTP action errors.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.Exceptions`

**Inherits from**: `Exception`

**Properties:**
- `HttpStatusCode HttpStatusCode` - HTTP status code

**Constructors:**
```csharp
public HttpActionException()
public HttpActionException(string message)
public HttpActionException(string message, Exception inner)
```

**Example:**
```csharp
throw new HttpActionException("User not found")
{
    HttpStatusCode = HttpStatusCode.NotFound
};
```

---

### 4. Extensions

#### HttpRequestExtension

Extension methods for HttpRequest.

**Namespace**: `SMEAppHouse.Core.Patterns.WebApi.Extensions`

**Key Methods:**

##### GetRawBodyTypeFormaterAsync<T>

```csharp
public static async Task<T> GetRawBodyTypeFormaterAsync<T>(
    this HttpRequest httpRequest, 
    ILogger logger = null, 
    Encoding encoding = null)
```

Deserializes the raw request body to the specified type.

**Example:**
```csharp
var user = await Request.GetRawBodyTypeFormaterAsync<User>(logger);
```

##### GetRawBodyStringFormaterAsync

```csharp
public static async Task<string> GetRawBodyStringFormaterAsync(
    this HttpRequest httpRequest, 
    Encoding encoding = null)
```

Gets the raw request body as a string.

**Example:**
```csharp
var body = await Request.GetRawBodyStringFormaterAsync();
```

##### GetFormValueAsync

```csharp
public static async Task<string> GetFormValueAsync(
    this HttpRequest httpRequest, 
    string parameterName)
```

Gets a form value by parameter name.

**Example:**
```csharp
var email = await Request.GetFormValueAsync("email");
```

##### RemoveParameters (RestSharp)

```csharp
public static void RemoveParameters(
    this RestRequest restRequest, 
    Func<Parameter, bool> condition)
```

Removes parameters from a RestRequest based on a condition.

**Example:**
```csharp
var request = new RestRequest();
request.AddParameter("name", "value");
request.AddParameter("id", 1);

// Remove all parameters with name "id"
request.RemoveParameters(p => p.Name == "id");
```

---

## Complete Usage Examples

### Example 1: Creating an API Controller

```csharp
using Microsoft.AspNetCore.Mvc;
using SMEAppHouse.Core.Patterns.WebApi.APIHostPattern;
using SMEAppHouse.Core.Patterns.Repo.Abstractions;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

public class Product : KeyedEntity<int>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}

[ApiController]
[ApiVersion("1.0")]
public class ProductsController : WebApiServiceHost<Product, int>
{
    public ProductsController(IRepositoryForKeyedEntity<Product, int> repository) 
        : base(repository)
    {
    }
    
    // All CRUD endpoints are automatically available:
    // POST   /api/v1/Products/CreateSingle
    // PUT    /api/v1/Products/UpdateSingle
    // DELETE /api/v1/Products/RemoveById?id=1
    // GET    /api/v1/Products/GetById?id=1
    // GET    /api/v1/Products/GetAll
    // GET    /api/v1/Products/Count
}
```

---

### Example 2: Using API Client

```csharp
using SMEAppHouse.Core.Patterns.WebApi.APIClientPattern;

public class ProductServiceClient : WebApiServiceClientBase<Product, int>
{
    public ProductServiceClient(string baseServiceAddress) 
        : base(baseServiceAddress)
    {
    }
}

// Usage
var client = new ProductServiceClient("https://api.example.com");

// Create product
var product = new Product 
{ 
    Name = "Laptop", 
    Price = 999.99m,
    Description = "High-performance laptop"
};
var created = client.Create(product);

// Get all products
var products = client.GetAll().ToList();

// Get product by ID
var laptop = client.GetById(created.Id);

// Update product
laptop.Price = 899.99m;
var updated = client.Update(laptop);

// Count products
int totalProducts = client.Count();

// Delete product
client.RemoveById(created.Id);
```

---

### Example 3: Custom Controller with Additional Methods

```csharp
[ApiController]
[ApiVersion("1.0")]
public class UsersController : WebApiServiceHost<User, int>
{
    public UsersController(IRepositoryForKeyedEntity<User, int> repository) 
        : base(repository)
    {
    }
    
    // Add custom endpoint
    [HttpGet]
    [Route("[Action]")]
    public async Task<IActionResult> GetActiveUsersAsync()
    {
        return await ExecuteAsync(async () =>
        {
            var users = await Repository.GetListAsync(
                filter: u => u.IsActive == true,
                orderBy: q => q.OrderBy(u => u.Name)
            );
            return Ok(users);
        });
    }
    
    [HttpGet]
    [Route("[Action]")]
    public async Task<IActionResult> SearchAsync(string searchTerm)
    {
        return await ExecuteAsync(async () =>
        {
            var users = await Repository.GetListAsync(
                filter: u => u.Name.Contains(searchTerm) || 
                            u.Email.Contains(searchTerm)
            );
            return Ok(users);
        });
    }
}
```

---

### Example 4: Using HttpRequest Extensions

```csharp
[HttpPost]
[Route("[Action]")]
public async Task<IActionResult> CreateFromRawBodyAsync()
{
    try
    {
        // Get raw body as string
        var body = await Request.GetRawBodyStringFormaterAsync();
        logger.LogInformation($"Received body: {body}");
        
        // Deserialize to type
        var user = await Request.GetRawBodyTypeFormaterAsync<User>(logger);
        
        if (user == null)
            return BadRequest("Invalid request body");
        
        await Repository.AddAsync(user);
        await Repository.CommitAsync();
        
        return Ok(user);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
```

---

### Example 5: Error Handling

```csharp
[ApiController]
[ApiVersion("1.0")]
public class OrdersController : WebApiServiceHost<Order, int>
{
    public OrdersController(IRepositoryForKeyedEntity<Order, int> repository) 
        : base(repository)
    {
    }
    
    // The base class automatically handles errors via ExecuteAsync
    // All methods are wrapped in try-catch and return appropriate HTTP status codes
    
    // Custom error handling example
    [HttpPost]
    [Route("[Action]")]
    public override async Task<IActionResult> CreateSingleAsync(Order entity)
    {
        if (entity == null)
            return BadRequest("Order cannot be null");
        
        // Validate business rules
        if (entity.TotalAmount <= 0)
            return BadRequest("Order total must be greater than zero");
        
        // Call base implementation
        return await base.CreateSingleAsync(entity);
    }
}
```

---

### Example 6: Dependency Injection Setup

```csharp
// In Startup.cs or Program.cs
public void ConfigureServices(IServiceCollection services)
{
    // Register repositories
    services.AddScoped<IRepositoryForKeyedEntity<User, int>, UserRepository>();
    services.AddScoped<IRepositoryForKeyedEntity<Product, int>, ProductRepository>();
    
    // API versioning
    services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
    });
    
    // CORS
    services.AddCors(options =>
    {
        options.AddPolicy("SiteCorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });
}
```

---

## Key Features

1. **Template Method Pattern**: Base classes provide standard CRUD operations
2. **API Versioning**: Built-in support for API versioning via route templates
3. **CORS Support**: Automatic CORS policy application
4. **Error Handling**: Centralized error handling via `ExecuteAsync`
5. **JSON Serialization**: Automatic JSON serialization/deserialization using Newtonsoft.Json
6. **Repository Integration**: Seamless integration with repository pattern
7. **HTTP Client Support**: Client-side base class for consuming APIs
8. **Extension Methods**: Utilities for request body parsing and RestSharp operations

---

## Dependencies

- Microsoft.AspNetCore.App (Framework Reference)
- RestSharp (v112.1.0)
- Newtonsoft.Json (via RestSharp)
- SMEAppHouse.Core.Patterns.EF
- SMEAppHouse.Core.Patterns.Repo

---

## Notes

- Controllers automatically handle CORS via `[EnableCors("SiteCorsPolicy")]`
- API versioning is supported via route template `api/v{version:apiVersion}/[controller]`
- All operations are asynchronous
- Error handling is centralized in `ExecuteAsync` method
- Client class names should end with "Client" for automatic URL generation
- Use `GetRawBodyTypeFormaterAsync` for complex request body deserialization
- Repository operations must be committed separately (not automatic)

---

## License

Copyright © Nephiora IT Solutions 2025
