# SMEAppHouse.Core.TopshelfAdapter.Common

## Overview

`SMEAppHouse.Core.TopshelfAdapter.Common` is a common library shared by Topshelf adapter projects. It provides base interfaces, enums, and configuration classes for Topshelf service implementations.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.TopshelfAdapter.Common`

---

## Public Classes and Interfaces

### 1. ITopshelfClient

Base interface for Topshelf clients.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter.Common`

**Key Properties:**
- `InitializationStatusEnum InitializationStatus` - Current initialization status
- `bool IsPaused` - Whether paused
- `bool IsResumed` - Whether resumed
- `bool IsTerminated` - Whether terminated
- `RuntimeBehaviorOptions RuntimeBehaviorOptions` - Runtime configuration
- `ILogger Logger` - Logger instance

**Key Methods:**
```csharp
void Resume()
void Suspend()
void Shutdown()
```

---

### 2. RuntimeBehaviorOptions

Configuration class for service runtime behavior.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter.Common`

**Properties:**
- `int MilliSecsDelay` - Delay between service action calls (default: 1)
- `bool IsBackground` - Run as background thread
- `bool LazyInitialization` - Delay initialization until Resume() is called

**Example:**
```csharp
var options = new RuntimeBehaviorOptions
{
    MilliSecsDelay = 5000, // 5 seconds between actions
    IsBackground = true,
    LazyInitialization = false
};
```

---

### 3. Enums

#### InitializationStatusEnum

Service initialization status.

**Values:**
- `NonState` - Not initialized
- `Initializing` - Currently initializing
- `Initialized` - Successfully initialized

#### NLogLevelEnum

Log levels for NLog integration.

**Values:**
- `Fatal` - Highest level, critical errors
- `Error` - Application crashes/exceptions
- `Warn` - Incorrect behavior but app continues
- `Info` - Normal behavior (mail sent, profile updated, etc.)
- `Debug` - Executed queries, authentication, session expiry
- `Trace` - Method entry/exit, detailed tracing

---

## Complete Usage Examples

### Example 1: Basic Configuration

```csharp
using SMEAppHouse.Core.TopshelfAdapter.Common;

var options = new RuntimeBehaviorOptions
{
    MilliSecsDelay = 1000, // 1 second delay
    IsBackground = true,
    LazyInitialization = false
};

// Use with service
var service = new MyService(options, logger);
```

---

### Example 2: Checking Initialization Status

```csharp
public class ServiceManager
{
    public void MonitorService(ITopshelfClient service)
    {
        switch (service.InitializationStatus)
        {
            case InitializationStatusEnum.NonState:
                Console.WriteLine("Service not initialized");
                break;
            case InitializationStatusEnum.Initializing:
                Console.WriteLine("Service initializing...");
                break;
            case InitializationStatusEnum.Initialized:
                Console.WriteLine("Service initialized");
                break;
        }
    }
}
```

---

## Key Features

1. **Common Interfaces**: Shared interfaces for all Topshelf adapters
2. **Configuration**: Centralized runtime behavior options
3. **Status Tracking**: Enum-based status management
4. **Logging Levels**: Standardized log level definitions

---

## Dependencies

- None (pure .NET library)

---

## Notes

- `MilliSecsDelay` controls the delay between `ServiceActionCallback()` invocations
- `IsBackground = true` prevents the thread from blocking application shutdown
- `LazyInitialization = true` delays initialization until `Resume()` is called
- Initialization status can be checked at any time
- NLog levels follow standard logging conventions

---

## License

Copyright © Nephiora IT Solutions 2025
