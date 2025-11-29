# SMEAppHouse.Core.RestSharpClientLib

## Overview

`SMEAppHouse.Core.RestSharpClientLib` is an extension wrapper library for using the RestSharp package. It provides a simplified API client interface and utilities for making HTTP requests.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

---

## Public Classes and Interfaces

### 1. IApiAccessor (Interface)

Interface for API access operations.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

#### Methods

```csharp
ApiResponse<T> CallApi<T>(string path, Method method, List<KeyValuePair<string, string>> queryParams, object postBody, Dictionary<string, string> headerParams, Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType)
```

---

### 2. ApiClient (Class)

Main client class for making API requests using RestSharp.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

#### Properties

- `Configuration Configuration` - Client configuration
- `string BasePath` - Base URL for API requests

#### Methods

```csharp
public ApiResponse<T> CallApi<T>(string path, Method method, ...)
public string ParameterToString(object obj)
public string SelectHeaderContentType(string[] contentTypes)
public string SelectHeaderAccept(string[] accepts)
public string EscapeString(string str)
```

#### Usage Example

```csharp
using RestSharp;
using SMEAppHouse.Core.RestSharpClientLib;

var client = new ApiClient
{
    BasePath = "https://api.example.com"
};

var response = client.CallApi<Product>(
    "/products/1",
    Method.Get,
    queryParams: null,
    postBody: null,
    headerParams: null,
    formParams: null,
    fileParams: null,
    pathParams: null,
    contentType: "application/json"
);

if (response.StatusCode == 200)
{
    var product = response.Data;
    Console.WriteLine($"Product: {product.Name}");
}
```

---

### 3. ApiResponse<T> (Class)

Represents an API response with data and status information.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

#### Properties

- `int StatusCode` - HTTP status code
- `Dictionary<string, string> Headers` - Response headers
- `T Data` - Response data
- `string ErrorText` - Error message if any

#### Usage Example

```csharp
var response = client.CallApi<List<Product>>("/products", Method.Get, ...);

if (response.StatusCode == 200)
{
    foreach (var product in response.Data)
    {
        Console.WriteLine(product.Name);
    }
}
else
{
    Console.WriteLine($"Error: {response.ErrorText}");
}
```

---

### 4. ApiException (Class)

Exception thrown for API errors.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

#### Properties

- `int ErrorCode` - Error code
- `string ErrorContent` - Error content

#### Usage Example

```csharp
try
{
    var response = client.CallApi<Product>(...);
}
catch (ApiException ex)
{
    Console.WriteLine($"API Error {ex.ErrorCode}: {ex.ErrorContent}");
}
```

---

### 5. Configuration (Class)

Configuration for API client.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

#### Properties

- `string BasePath` - Base URL
- `int Timeout` - Request timeout in milliseconds
- `string UserAgent` - User agent string
- `Dictionary<string, string> DefaultHeaders` - Default headers

#### Usage Example

```csharp
var config = new Configuration
{
    BasePath = "https://api.example.com",
    Timeout = 30000,
    DefaultHeaders = new Dictionary<string, string>
    {
        { "Authorization", "Bearer token123" }
    }
};

var client = new ApiClient { Configuration = config };
```

---

### 6. IReadableConfiguration (Interface)

Interface for readable configuration.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

---

### 7. GlobalConfiguration (Class)

Global configuration singleton.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

#### Usage Example

```csharp
GlobalConfiguration.Instance.BasePath = "https://api.example.com";
GlobalConfiguration.Instance.Timeout = 30000;
```

---

### 8. ExceptionFactory (Delegate)

Delegate for creating exceptions from API responses.

**Namespace**: `SMEAppHouse.Core.RestSharpClientLib`

---

## Complete Usage Examples

### Example 1: GET Request

```csharp
using RestSharp;
using SMEAppHouse.Core.RestSharpClientLib;

var client = new ApiClient
{
    BasePath = "https://jsonplaceholder.typicode.com"
};

var response = client.CallApi<List<Post>>(
    "/posts",
    Method.Get,
    queryParams: null,
    postBody: null,
    headerParams: null,
    formParams: null,
    fileParams: null,
    pathParams: null,
    contentType: "application/json"
);

if (response.StatusCode == 200)
{
    foreach (var post in response.Data)
    {
        Console.WriteLine($"{post.Id}: {post.Title}");
    }
}
```

### Example 2: POST Request

```csharp
var newPost = new Post
{
    Title = "New Post",
    Body = "Post content",
    UserId = 1
};

var response = client.CallApi<Post>(
    "/posts",
    Method.Post,
    queryParams: null,
    postBody: newPost,
    headerParams: null,
    formParams: null,
    fileParams: null,
    pathParams: null,
    contentType: "application/json"
);

if (response.StatusCode == 201)
{
    Console.WriteLine($"Created post with ID: {response.Data.Id}");
}
```

### Example 3: Request with Headers and Query Parameters

```csharp
var queryParams = new List<KeyValuePair<string, string>>
{
    new KeyValuePair<string, string>("userId", "1")
};

var headerParams = new Dictionary<string, string>
{
    { "Authorization", "Bearer token123" },
    { "X-API-Key", "key456" }
};

var response = client.CallApi<List<Post>>(
    "/posts",
    Method.Get,
    queryParams: queryParams,
    postBody: null,
    headerParams: headerParams,
    formParams: null,
    fileParams: null,
    pathParams: null,
    contentType: "application/json"
);
```

---

## Dependencies

- RestSharp (v112.1.0)
- Newtonsoft.Json

---

## Notes

- Wraps RestSharp for simplified API access
- Provides typed responses
- Supports all HTTP methods
- Includes error handling
- Configurable timeouts and headers

---

## License

Copyright © Nephiora IT Solutions 2025