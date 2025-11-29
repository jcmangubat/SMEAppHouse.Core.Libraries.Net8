# SMEAppHouse.Core.TopshelfAdapter

## Overview

`SMEAppHouse.Core.TopshelfAdapter` is a library for handling Topshelf implemented Windows services. It provides base classes and abstractions for creating Windows services with lifecycle management, pause/resume functionality, and initialization control.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.TopshelfAdapter`

---

## Public Classes and Interfaces

### 1. TopshelfSocket<T>

Abstract base class for Topshelf service implementations.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter`

**Implements**: `ITopshelfClientExt`

**Key Properties:**
- `InitializationStatusEnum InitializationStatus` - Current initialization status
- `bool IsPaused` - Whether the service is paused
- `bool IsResumed` - Whether the service is resumed
- `bool IsTerminated` - Whether the service is terminated
- `ILogger Logger` - Logger instance
- `RuntimeBehaviorOptions RuntimeBehaviorOptions` - Runtime behavior configuration
- `Thread ServiceThread` - The service thread

**Key Events:**
- `ServiceInitializedEventHandler OnServiceInitialized` - Raised when service is initialized

**Key Methods:**

##### Control Methods

```csharp
public void Resume()
public void Suspend()
public void Shutdown()
```

##### Abstract Methods (Must Implement)

```csharp
protected abstract void ServiceInitializeCallback()
protected abstract void ServiceTerminateCallback()
protected abstract void ServiceActionCallback()
```

**Constructor:**
```csharp
protected TopshelfSocket(RuntimeBehaviorOptions runtimeBehaviorOptions, ILogger logger)
```

**Example:**
```csharp
using Microsoft.Extensions.Logging;
using SMEAppHouse.Core.TopshelfAdapter;
using SMEAppHouse.Core.TopshelfAdapter.Common;

public class MyService : TopshelfSocket<MyService>
{
    public MyService(RuntimeBehaviorOptions options, ILogger logger) 
        : base(options, logger)
    {
        OnServiceInitialized += (sender, e) =>
        {
            Logger.LogInformation("Service initialized");
        };
    }

    protected override void ServiceInitializeCallback()
    {
        // Initialize resources, connections, etc.
        Logger.LogInformation("Initializing service resources");
    }

    protected override void ServiceTerminateCallback()
    {
        // Cleanup resources
        Logger.LogInformation("Terminating service");
    }

    protected override void ServiceActionCallback()
    {
        // Main service logic - called repeatedly
        Logger.LogInformation($"Service action at {DateTime.Now}");
        // Your service work here
    }
}

// Usage
var options = new RuntimeBehaviorOptions
{
    MilliSecsDelay = 1000,
    IsBackground = true,
    LazyInitialization = false
};

var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<MyService>();
var service = new MyService(options, logger);
service.Resume();
```

---

### 2. ITopshelfClientExt

Extended interface for Topshelf clients.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter`

**Inherits from**: `ITopshelfClient`

**Additional Properties:**
- `Thread ServiceThread` - The service thread

**Additional Events:**
- `ServiceInitializedEventHandler OnServiceInitialized` - Service initialization event

---

### 3. ServiceInitializedEventArgs

Event arguments for service initialization.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter`

**Example:**
```csharp
service.OnServiceInitialized += (sender, e) =>
{
    Console.WriteLine("Service initialized successfully");
};
```

---

## Complete Usage Examples

### Example 1: Basic Service Implementation

```csharp
using Microsoft.Extensions.Logging;
using SMEAppHouse.Core.TopshelfAdapter;
using SMEAppHouse.Core.TopshelfAdapter.Common;

public class DataProcessingService : TopshelfSocket<DataProcessingService>
{
    private IDbConnection _connection;

    public DataProcessingService(RuntimeBehaviorOptions options, ILogger logger) 
        : base(options, logger)
    {
    }

    protected override void ServiceInitializeCallback()
    {
        Logger.LogInformation("Initializing database connection");
        _connection = CreateDatabaseConnection();
        Logger.LogInformation("Service initialized");
    }

    protected override void ServiceTerminateCallback()
    {
        Logger.LogInformation("Closing database connection");
        _connection?.Dispose();
        Logger.LogInformation("Service terminated");
    }

    protected override void ServiceActionCallback()
    {
        try
        {
            Logger.LogInformation("Processing data...");
            ProcessData();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in service action");
        }
    }

    private void ProcessData()
    {
        // Data processing logic
    }

    private IDbConnection CreateDatabaseConnection()
    {
        // Create connection
        return null; // Placeholder
    }
}
```

---

### Example 2: Service with Lazy Initialization

```csharp
public class LazyService : TopshelfSocket<LazyService>
{
    public LazyService(RuntimeBehaviorOptions options, ILogger logger) 
        : base(options, logger)
    {
    }

    protected override void ServiceInitializeCallback()
    {
        Logger.LogInformation("Lazy initialization triggered");
        // Initialize only when Resume() is called
    }

    protected override void ServiceTerminateCallback()
    {
        Logger.LogInformation("Service cleanup");
    }

    protected override void ServiceActionCallback()
    {
        Logger.LogInformation("Service running");
    }
}

// Usage with lazy initialization
var options = new RuntimeBehaviorOptions
{
    MilliSecsDelay = 5000,
    IsBackground = true,
    LazyInitialization = true // Don't initialize until Resume() is called
};

var service = new LazyService(options, logger);
// Service not initialized yet

// Initialize and start when ready
service.Resume(); // Now initialization happens
```

---

## Key Features

1. **Lifecycle Management**: Initialize, Resume, Suspend, Shutdown controls
2. **Thread Management**: Background thread execution with configurable behavior
3. **Lazy Initialization**: Optional delayed initialization
4. **Event-Driven**: Initialization events for monitoring
5. **Logging Integration**: Built-in ILogger support
6. **Console Ticker**: Visual feedback during service execution

---

## Dependencies

- SMEAppHouse.Core.CodeKits
- SMEAppHouse.Core.TopshelfAdapter.Common

---

## Notes

- Services run in background threads by default
- `ServiceActionCallback()` is called repeatedly based on `MilliSecsDelay`
- Use `LazyInitialization = true` to delay initialization until `Resume()` is called
- Always implement cleanup in `ServiceTerminateCallback()`
- The service thread is created but not started until `Resume()` is called (if lazy initialization is enabled)
- Console ticker provides visual feedback with rotating characters

---

## License

Copyright © Nephiora IT Solutions 2025
