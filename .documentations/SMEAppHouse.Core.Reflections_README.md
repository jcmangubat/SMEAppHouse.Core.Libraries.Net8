# SMEAppHouse.Core.Reflections

## Overview

`SMEAppHouse.Core.Reflections` is a library of useful reusable functions and procedures extending Entity Framework based on patterns. It provides utilities for building dynamic LINQ expressions, filtering, and dynamic type creation using reflection.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.Reflections`

---

## Public Classes and Utilities

### 1. ExpressionBuilder

Static class for building LINQ expressions dynamically.

**Namespace**: `SMEAppHouse.Core.Reflections`

#### Operator Enum

```csharp
public enum Operator
{
    Contains,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqualTo,
    StartsWith,
    EndsWith,
    Equals,
    NotEqual
}
```

#### GetExpression<T>

Builds a LINQ expression from a list of filters.

```csharp
public static Expression<Func<T, bool>> GetExpression<T>(List<Filter> filters)
```

**Example:**
```csharp
using SMEAppHouse.Core.Reflections;
using System.Linq.Expressions;

public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

// Create filters
var filters = new ExpressionBuilder.Filters
{
    new ExpressionBuilder.Filter 
    { 
        PropertyName = "Name", 
        Value = "John", 
        Operator = ExpressionBuilder.Operator.Contains 
    },
    new ExpressionBuilder.Filter 
    { 
        PropertyName = "Age", 
        Value = "18", 
        DataType = typeof(int),
        Operator = ExpressionBuilder.Operator.GreaterThanOrEqual 
    }
};

// Build expression
var expression = ExpressionBuilder.GetExpression<User>(filters);

// Use with Entity Framework
var users = context.Users.Where(expression).ToList();
```

---

### 2. Filter Class

Represents a filter condition for building expressions.

**Namespace**: `SMEAppHouse.Core.Reflections` (nested in `ExpressionBuilder`)

**Properties:**
- `string PropertyName` - Name of the property to filter
- `string Value` - Filter value
- `Type DataType` - Data type of the value
- `Operator Operator` - Comparison operator (default: Contains)

**Example:**
```csharp
var filter = new ExpressionBuilder.Filter
{
    PropertyName = "Email",
    Value = "@example.com",
    Operator = ExpressionBuilder.Operator.EndsWith
};
```

---

### 3. Filters Class

Collection of filters with helper constructors.

**Namespace**: `SMEAppHouse.Core.Reflections` (nested in `ExpressionBuilder`)

**Inherits from**: `List<Filter>`

**Constructors:**

##### From JSON

```csharp
public Filters(string filtersjson)
```

Parses filters from JSON string.

**Example:**
```csharp
var json = @"{
    ""Name"": ""[string]John"",
    ""Age"": ""[int]25""
}";

var filters = new ExpressionBuilder.Filters(json);
var expression = ExpressionBuilder.GetExpression<User>(filters);
```

##### From String Array

```csharp
public Filters(params string[] filters)
```

Parses filters from string array in format: `"PropertyName:[Type]Value"`

**Example:**
```csharp
var filters = new ExpressionBuilder.Filters(
    "Name:[string]John",
    "Age:[int]25",
    "Email:[string]@example.com"
);
```

##### From Dictionary

```csharp
public Filters(IDictionary<string, string> filters)
```

Creates filters from dictionary.

**Methods:**

##### Add

```csharp
public void Add(string name, string value = "")
```

Adds a filter by property name and value.

**Example:**
```csharp
var filters = new ExpressionBuilder.Filters();
filters.Add("Name", "John");
filters.Add("Age", "25");
```

---

### 4. DynamicLibrary

Static class for dynamic type creation and expression parsing.

**Namespace**: `SMEAppHouse.Core.Reflections`

**Key Methods:**

##### CreateClass

```csharp
public static Type CreateClass(params DynamicProperty[] properties)
public static Type CreateClass(IEnumerable<DynamicProperty> properties)
```

Creates a dynamic class type with specified properties.

**Example:**
```csharp
var properties = new[]
{
    new DynamicProperty("Name", typeof(string)),
    new DynamicProperty("Age", typeof(int)),
    new DynamicProperty("Email", typeof(string))
};

var dynamicType = DynamicLibrary.CreateClass(properties);
var instance = Activator.CreateInstance(dynamicType);

// Set properties using reflection
dynamicType.GetProperty("Name").SetValue(instance, "John");
dynamicType.GetProperty("Age").SetValue(instance, 30);
```

##### ParseLambda

```csharp
public static LambdaExpression ParseLambda(Type itType, Type resultType, string expression, params object[] values)
public static Expression<Func<T, S>> ParseLambda<T, S>(string expression, params object[] values)
```

Parses a string expression into a lambda expression.

**Example:**
```csharp
// Parse lambda expression from string
var expression = DynamicLibrary.ParseLambda<User, bool>(
    "Age > 18 && Name.Contains(\"John\")"
);
```

---

## Complete Usage Examples

### Example 1: Dynamic Filtering with Entity Framework

```csharp
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Reflections;

public class UserService
{
    private readonly DbContext _context;

    public UserService(DbContext context)
    {
        _context = context;
    }

    public List<User> SearchUsers(string nameFilter, int? minAge, string emailFilter)
    {
        var filters = new ExpressionBuilder.Filters();

        if (!string.IsNullOrEmpty(nameFilter))
        {
            filters.Add(new ExpressionBuilder.Filter
            {
                PropertyName = "Name",
                Value = nameFilter,
                Operator = ExpressionBuilder.Operator.Contains
            });
        }

        if (minAge.HasValue)
        {
            filters.Add(new ExpressionBuilder.Filter
            {
                PropertyName = "Age",
                Value = minAge.Value.ToString(),
                DataType = typeof(int),
                Operator = ExpressionBuilder.Operator.GreaterThanOrEqual
            });
        }

        if (!string.IsNullOrEmpty(emailFilter))
        {
            filters.Add(new ExpressionBuilder.Filter
            {
                PropertyName = "Email",
                Value = emailFilter,
                Operator = ExpressionBuilder.Operator.EndsWith
            });
        }

        if (filters.Count == 0)
            return _context.Set<User>().ToList();

        var expression = ExpressionBuilder.GetExpression<User>(filters);
        return _context.Set<User>().Where(expression).ToList();
    }
}
```

---

### Example 2: Filters from JSON

```csharp
public class ProductService
{
    public List<Product> FilterProducts(string filterJson)
    {
        var filters = new ExpressionBuilder.Filters(filterJson);
        var expression = ExpressionBuilder.GetExpression<Product>(filters);
        
        return context.Products.Where(expression).ToList();
    }
}

// Usage
var filterJson = @"{
    ""Name"": ""[string]Laptop"",
    ""Price"": ""[decimal]1000"",
    ""InStock"": ""[bool]true""
}";

var products = productService.FilterProducts(filterJson);
```

---

### Example 3: Complex Filtering

```csharp
public class OrderService
{
    public List<Order> GetFilteredOrders(OrderFilterModel filter)
    {
        var filters = new ExpressionBuilder.Filters();

        if (!string.IsNullOrEmpty(filter.OrderNumber))
        {
            filters.Add(new ExpressionBuilder.Filter
            {
                PropertyName = "OrderNumber",
                Value = filter.OrderNumber,
                Operator = ExpressionBuilder.Operator.StartsWith
            });
        }

        if (filter.MinAmount.HasValue)
        {
            filters.Add(new ExpressionBuilder.Filter
            {
                PropertyName = "TotalAmount",
                Value = filter.MinAmount.Value.ToString(),
                DataType = typeof(decimal),
                Operator = ExpressionBuilder.Operator.GreaterThanOrEqual
            });
        }

        if (filter.MaxAmount.HasValue)
        {
            filters.Add(new ExpressionBuilder.Filter
            {
                PropertyName = "TotalAmount",
                Value = filter.MaxAmount.Value.ToString(),
                DataType = typeof(decimal),
                Operator = ExpressionBuilder.Operator.LessThanOrEqualTo
            });
        }

        if (filter.Status.HasValue)
        {
            filters.Add(new ExpressionBuilder.Filter
            {
                PropertyName = "Status",
                Value = filter.Status.Value.ToString(),
                Operator = ExpressionBuilder.Operator.Equals
            });
        }

        var expression = ExpressionBuilder.GetExpression<Order>(filters);
        return context.Orders.Where(expression).ToList();
    }
}
```

---

### Example 4: Dynamic Type Creation

```csharp
using SMEAppHouse.Core.Reflections;

public class DynamicDataService
{
    public object CreateDynamicObject(Dictionary<string, object> data)
    {
        var properties = data.Select(kvp => 
            new DynamicProperty(kvp.Key, kvp.Value?.GetType() ?? typeof(object))
        ).ToArray();

        var dynamicType = DynamicLibrary.CreateClass(properties);
        var instance = Activator.CreateInstance(dynamicType);

        foreach (var kvp in data)
        {
            var property = dynamicType.GetProperty(kvp.Key);
            if (property != null)
            {
                property.SetValue(instance, kvp.Value);
            }
        }

        return instance;
    }
}

// Usage
var data = new Dictionary<string, object>
{
    { "Name", "John" },
    { "Age", 30 },
    { "Email", "john@example.com" }
};

var dynamicObject = service.CreateDynamicObject(data);
```

---

## Key Features

1. **Dynamic Expression Building**: Build LINQ expressions from filter collections
2. **Multiple Operators**: Support for Contains, Equals, GreaterThan, LessThan, StartsWith, EndsWith, etc.
3. **JSON Support**: Parse filters from JSON strings
4. **Type Safety**: Support for different data types in filters
5. **Dynamic Types**: Create classes dynamically at runtime
6. **Entity Framework Integration**: Seamless integration with EF queries

---

## Dependencies

- Microsoft.EntityFrameworkCore (v9.0.9)
- Newtonsoft.Json (v13.0.4)

---

## Notes

- Filters are combined using AND logic (all conditions must match)
- Property names are case-sensitive
- Use `DataType` property to ensure correct type conversion
- JSON filter format: `"PropertyName": "[Type]Value"`
- String array format: `"PropertyName:[Type]Value"`
- Dynamic types are created at runtime and cached
- Expression building supports nested properties (e.g., "User.Name")

---

## License

Copyright © Nephiora IT Solutions 2025
