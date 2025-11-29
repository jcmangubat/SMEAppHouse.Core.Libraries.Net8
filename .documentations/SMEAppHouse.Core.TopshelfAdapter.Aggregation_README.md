# SMEAppHouse.Core.TopshelfAdapter.Aggregation

## Overview

`SMEAppHouse.Core.TopshelfAdapter.Aggregation` is a library for aggregating and managing multiple Topshelf services. It provides a controller pattern for managing collections of service workers with unified control operations.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.TopshelfAdapter.Aggregation`

---

## Public Classes and Interfaces

### 1. ServiceController

Controller for managing multiple Topshelf service workers.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter.Aggregation`

**Implements**: `IServiceController`

**Key Properties:**
- `ObservableCollection<ITopshelfClientExt> ServiceWorkers` - Collection of service workers

**Key Events:**
- `ServiceWorkerInitializedEventHandler OnServiceWorkerInitialized` - Raised when a worker is initialized

**Key Methods:**

##### Control Methods

```csharp
public void ResumeAll()
public void HaltAll()
public void ShutdownAll()
```

**Constructor:**
```csharp
public ServiceController()
```

**Example:**
```csharp
using SMEAppHouse.Core.TopshelfAdapter;
using SMEAppHouse.Core.TopshelfAdapter.Aggregation;
using SMEAppHouse.Core.TopshelfAdapter.Common;

var controller = new ServiceController();

// Add service workers
var service1 = new DataProcessingService(options1, logger);
var service2 = new EmailService(options2, logger);
var service3 = new ReportService(options3, logger);

controller.ServiceWorkers.Add(service1);
controller.ServiceWorkers.Add(service2);
controller.ServiceWorkers.Add(service3);

// Subscribe to initialization events
controller.OnServiceWorkerInitialized += (sender, e) =>
{
    Console.WriteLine($"Service worker initialized: {e.ServiceWorker.GetType().Name}");
};

// Control all services
controller.ResumeAll(); // Start all services
// ...
controller.HaltAll(); // Pause all services
// ...
controller.ShutdownAll(); // Shutdown all services
```

---

### 2. IServiceController

Interface for service controllers.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter.Aggregation`

**Properties:**
- `ObservableCollection<ITopshelfClientExt> ServiceWorkers`
- `ServiceWorkerInitializedEventHandler OnServiceWorkerInitialized`

**Methods:**
```csharp
void ResumeAll()
void HaltAll()
void ShutdownAll()
```

---

### 3. ServiceWorkerInitializedEventArgs

Event arguments for service worker initialization.

**Namespace**: `SMEAppHouse.Core.TopshelfAdapter.Aggregation`

**Properties:**
- `ITopshelfClient ServiceWorker` - The initialized service worker

---

## Complete Usage Examples

### Example 1: Managing Multiple Services

```csharp
using SMEAppHouse.Core.TopshelfAdapter.Aggregation;
using SMEAppHouse.Core.TopshelfAdapter.Common;

public class ServiceManager
{
    private ServiceController _controller;

    public void InitializeServices()
    {
        _controller = new ServiceController();

        // Create and add services
        var dataService = new DataProcessingService(GetOptions(), GetLogger());
        var emailService = new EmailService(GetOptions(), GetLogger());
        var reportService = new ReportService(GetOptions(), GetLogger());

        _controller.ServiceWorkers.Add(dataService);
        _controller.ServiceWorkers.Add(emailService);
        _controller.ServiceWorkers.Add(reportService);

        // Monitor initialization
        _controller.OnServiceWorkerInitialized += (sender, e) =>
        {
            Console.WriteLine($"Initialized: {e.ServiceWorker.GetType().Name}");
        };
    }

    public void StartAllServices()
    {
        _controller.ResumeAll();
    }

    public void PauseAllServices()
    {
        _controller.HaltAll();
    }

    public void StopAllServices()
    {
        _controller.ShutdownAll();
    }
}
```

---

### Example 2: Service Lifecycle Management

```csharp
public class ApplicationServiceHost
{
    private ServiceController _controller;

    public void Start()
    {
        _controller = new ServiceController();

        // Register services
        RegisterServices();

        // Start all
        _controller.ResumeAll();
    }

    public void Stop()
    {
        _controller?.ShutdownAll();
    }

    private void RegisterServices()
    {
        // Add your services here
    }
}
```

---

## Key Features

1. **Service Aggregation**: Manage multiple services in one controller
2. **Unified Control**: Control all services with single method calls
3. **Observable Collection**: Track service additions/removals
4. **Event Monitoring**: Track when services are initialized
5. **Lifecycle Management**: Resume, Halt, and Shutdown all services

---

## Dependencies

- SMEAppHouse.Core.CodeKits
- SMEAppHouse.Core.TopshelfAdapter
- SMEAppHouse.Core.TopshelfAdapter.Common

---

## Notes

- Services are managed in an `ObservableCollection`
- Collection changes are automatically tracked
- All control methods operate on all services in the collection
- Services can be added/removed from the collection at runtime
- Initialization events are automatically wired when services are added

---

## License

Copyright © Nephiora IT Solutions 2025
