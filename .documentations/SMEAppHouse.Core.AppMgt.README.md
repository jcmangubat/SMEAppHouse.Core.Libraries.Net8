# SMEAppHouse.Core.AppMgt

## Overview

`SMEAppHouse.Core.AppMgt` is a library for setting up application configurations and managing service lifecycle. It provides abstractions for service starters, authentication token providers, configuration validation, and messaging.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.AppMgt`

---

## Public Classes and Interfaces

### 1. ServiceStarter<T> (Abstract Class)

Base class for implementing scheduled services with timer-based execution.

**Namespace**: `SMEAppHouse.Core.AppMgt.ServiceTemplate`

#### Properties

- `ILogger<T> Logger` - Logger instance for the service
- `IConfiguration Configuration` - Application configuration
- `IMapper Mapper` - AutoMapper instance for object mapping
- `IPayloadsEnvelope PayloadsEnvelope` - Messaging envelope for payloads
- `int TaskIntervalInSeconds` - Interval between service executions (default: 60 seconds)
- `ServicePulseBehaviorEnum PulseBehavior` - Execution mode: Synchronous or Asynchronous

#### Methods

```csharp
public abstract void PerformServiceTask()
```
Override this method to implement your service logic.

```csharp
public Task Execute(CancellationToken cancellationToken)
```
Starts the service timer.

```csharp
public Task Terminate(CancellationToken cancellationToken)
```
Stops the service timer.

#### Constructors

```csharp
protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper)
protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper, IPayloadsEnvelope pyloadEnv)
protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper, int taskIntervalInSeconds)
protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper, IPayloadsEnvelope pyloadEnv, int taskIntervalInSeconds)
```

#### Usage Example

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SMEAppHouse.Core.AppMgt.ServiceTemplate;

public class MyScheduledService : ServiceStarter<MyScheduledService>
{
    public MyScheduledService(
        IConfiguration config, 
        ILogger<MyScheduledService> logger, 
        IMapper mapper) 
        : base(config, logger, mapper, 30) // Run every 30 seconds
    {
        PulseBehavior = ServicePulseBehaviorEnum.Synchronous;
    }

    public override void PerformServiceTask()
    {
        Logger.LogInformation("Performing scheduled task...");
        
        // Your service logic here
        ProcessData();
        SendNotifications();
        
        Logger.LogInformation("Task completed.");
    }

    private void ProcessData()
    {
        // Implementation
    }

    private void SendNotifications()
    {
        // Implementation
    }
}

// Usage in Program.cs or Startup.cs
services.AddHostedService<MyScheduledService>();
```

---

### 2. IServiceStarter<T> (Interface)

Interface for service starter implementations.

**Namespace**: `SMEAppHouse.Core.AppMgt.ServiceTemplate`

#### Properties

- `IConfiguration Configuration { get; set; }`
- `ILogger<T> Logger { get; set; }`
- `IMapper Mapper { get; set; }`
- `IPayloadsEnvelope PayloadsEnvelope { get; set; }`
- `int TaskIntervalInSeconds { get; set; }`
- `ServicePulseBehaviorEnum PulseBehavior { get; set; }`

#### Methods

- `void PerformServiceTask()`
- `Task Execute(CancellationToken cancellationToken)`
- `Task Terminate(CancellationToken cancellationToken)`

---

### 3. ServicePulseBehaviorEnum (Enum)

Defines the execution behavior of the service.

**Values:**
- `Synchronous` - Executes tasks synchronously
- `Asynchronous` - Executes tasks asynchronously

---

### 4. ITokenProvider (Interface)

Interface for token generation and validation.

**Namespace**: `SMEAppHouse.Core.AppMgt.AuthMgr.Provider`

#### Methods

```csharp
string CreateToken(string username, DateTime expiry)
```
Creates a JWT token for the specified username with expiration.

```csharp
TokenValidationParameters GetValidationParameters()
```
Returns token validation parameters for JWT validation.

#### Usage Example

```csharp
using SMEAppHouse.Core.AppMgt.AuthMgr.Provider;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenProvider : ITokenProvider
{
    private readonly string _secretKey;

    public JwtTokenProvider(string secretKey)
    {
        _secretKey = secretKey;
    }

    public string CreateToken(string username, DateTime expiry)
    {
        // Implementation for creating JWT token
        // ...
    }

    public TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            // Configure validation parameters
            // ...
        };
    }
}
```

---

### 5. RsaJwtTokenProvider (Class)

RSA-based JWT token provider implementation.

**Namespace**: `SMEAppHouse.Core.AppMgt.AuthMgr.Provider`

#### Usage Example

```csharp
var tokenProvider = new RsaJwtTokenProvider(rsaParameters, issuer, audience);
string token = tokenProvider.CreateToken("username", DateTime.UtcNow.AddHours(1));
```

---

### 6. IPayloadsEnvelope (Interface)

Interface for message payload envelope.

**Namespace**: `SMEAppHouse.Core.AppMgt.Messaging`

#### Usage Example

```csharp
using SMEAppHouse.Core.AppMgt.Messaging;

public class MyService : ServiceStarter<MyService>
{
    public MyService(
        IConfiguration config, 
        ILogger<MyService> logger, 
        IMapper mapper,
        IPayloadsEnvelope payloadsEnvelope) 
        : base(config, logger, mapper, payloadsEnvelope)
    {
    }

    public override void PerformServiceTask()
    {
        // Use PayloadsEnvelope to send messages
        PayloadsEnvelope?.Send(new { Message = "Task completed" });
    }
}
```

---

### 7. PayloadsEnvelope (Class)

Implementation of IPayloadsEnvelope for message handling.

**Namespace**: `SMEAppHouse.Core.AppMgt.Messaging`

---

### 8. AppConfig (Abstract Class)

Base abstract class for application configuration. Implements `IAppConfig` interface.

**Namespace**: `SMEAppHouse.Core.AppMgt.AppCfgs.Base`

**Properties:**
- `bool InDemoMode` - Indicates if the application is running in demo mode
- `AppEFBehaviorAttributes AppEFBehaviorAttributes` - Entity Framework behavior configuration (migration table, schema, etc.)

**Methods:**
- `abstract void Validate()` - Must be implemented to validate configuration

#### Usage Example

```csharp
public class MyAppConfig : AppConfig
{
    public string DatabaseConnectionString { get; set; }
    public int MaxRetries { get; set; }
    public bool EnableLogging { get; set; }

    public override void Validate()
    {
        if (string.IsNullOrEmpty(DatabaseConnectionString))
            throw new ValidationException("DatabaseConnectionString is required");
    }
}

// In appsettings.json
{
  "MyAppConfig": {
    "DatabaseConnectionString": "Server=...",
    "MaxRetries": 3,
    "EnableLogging": true,
    "AppEFBehaviorAttributes": {
      "MigrationTblName": "MyMigrationsHistory",
      "DbSchema": "dbo"
    }
  }
}
```

---

### 9. IAppConfig (Interface)

Interface for application configuration.

**Namespace**: `SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces`

---

### 10. IValidatable (Interface)

Interface for configuration validation.

**Namespace**: `SMEAppHouse.Core.AppMgt.AppCfgs.Validator`

#### Usage Example

```csharp
public class MyConfig : IValidatable
{
    public string RequiredProperty { get; set; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(RequiredProperty))
            throw new ValidationException("RequiredProperty is required");
    }
}
```

---

### 11. AppConfigValidationStartupFilter

Startup filter for validating application configuration.

**Namespace**: `SMEAppHouse.Core.AppMgt.AppCfgs.Validator`

#### Usage Example

```csharp
// In Program.cs or Startup.cs
services.AddTransient<IStartupFilter, AppConfigValidationStartupFilter>();
```

---

### 12. Token (Class)

Represents an authentication token.

**Namespace**: `SMEAppHouse.Core.AppMgt.AuthMgr.Models`

#### Properties

- `string Value` - Token value
- `DateTime Expiry` - Token expiration date
- `string Username` - Associated username

---

### 13. User (Class)

Represents a user entity.

**Namespace**: `SMEAppHouse.Core.AppMgt.AuthMgr.Models`

---

## Configuration Extensions

### Extensions Class

**Namespace**: `SMEAppHouse.Core.AppMgt.AppCfgs`

Provides extension methods for configuration setup.

#### Usage Example

```csharp
using SMEAppHouse.Core.AppMgt.AppCfgs;

// In Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configure your app settings
builder.Services.Configure<MyAppSettings>(
    builder.Configuration.GetSection("MyAppSettings"));
```

---

## Complete Usage Example

### Creating a Scheduled Service

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using SMEAppHouse.Core.AppMgt.ServiceTemplate;
using SMEAppHouse.Core.AppMgt.Messaging;

public class DataProcessingService : ServiceStarter<DataProcessingService>
{
    public DataProcessingService(
        IConfiguration config,
        ILogger<DataProcessingService> logger,
        IMapper mapper,
        IPayloadsEnvelope payloadsEnvelope)
        : base(config, logger, mapper, payloadsEnvelope, 60) // Every 60 seconds
    {
        PulseBehavior = ServicePulseBehaviorEnum.Asynchronous;
    }

    public override void PerformServiceTask()
    {
        try
        {
            Logger.LogInformation("Starting data processing...");
            
            // Process data
            var data = FetchData();
            ProcessData(data);
            
            // Send notification
            PayloadsEnvelope?.Send(new 
            { 
                Event = "DataProcessed", 
                Timestamp = DateTime.UtcNow 
            });
            
            Logger.LogInformation("Data processing completed.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during data processing");
        }
    }

    private IEnumerable<DataModel> FetchData()
    {
        // Fetch data from source
        return new List<DataModel>();
    }

    private void ProcessData(IEnumerable<DataModel> data)
    {
        // Process the data
    }
}

// Register in Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton<IPayloadsEnvelope, PayloadsEnvelope>();
builder.Services.AddHostedService<DataProcessingService>();
```

---

## Dependencies

- AutoMapper (v13.0.1)
- Microsoft.Extensions.DependencyInjection.Abstractions (v8.0.1)
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Logging
- Microsoft.IdentityModel.Tokens (for token providers)

---

## Notes

- Services run on a timer-based schedule
- Supports both synchronous and asynchronous execution modes
- Integrates with .NET dependency injection
- Provides configuration validation on startup
- Supports messaging through payload envelopes

---

## License

Copyright © Nephiora IT Solutions 2025

