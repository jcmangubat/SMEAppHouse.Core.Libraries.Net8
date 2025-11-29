# SMEAppHouse.Core.ProcessService

## Overview

`SMEAppHouse.Core.ProcessService` is a library for implementing multi-threaded processes with support for background agents, task-based execution, pause/resume functionality, and parallel operation management. It provides base classes for creating long-running processes that can be controlled programmatically.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.ProcessService`

---

## Public Classes and Interfaces

### 1. Process Agent Base Classes

#### ProcessAgentBase

Abstract base class for process agents that execute actions at specified intervals.

**Namespace**: `SMEAppHouse.Core.ProcessService.Engines`

**Implements**: `IProcessAgent`

**Key Properties:**
- `EngineStatusEnum EngineStatus` - Current status of the engine
- `TimeSpan IntervalDelay` - Delay between action executions
- `bool AutoActivate` - Automatically activate on construction
- `bool AutoStart` - Automatically start after activation

**Key Events:**
- `BeforeEngineStatusChangedEventHandler OnBeforeEngineChanged` - Raised before status change
- `EngineStatusChangedEventHandler OnEngineStatusChanged` - Raised when status changes
- `EventHandler OnReadyState` - Raised when ready

**Key Methods:**

##### Control Methods

```csharp
public Task Activate()
public void Resume()
public void Suspend(TimeSpan timeout)
public void Suspend(int timeoutMilliSecs = 0)
public void Shutdown(int timeOut = 0)
```

##### Abstract Methods (Must Implement)

```csharp
protected abstract void ServiceActionCallback()
internal abstract void ServiceActionInitialize()
internal abstract void ServiceActionOnShutdown(int timeOut)
```

**Constructors:**
```csharp
protected ProcessAgentBase()
protected ProcessAgentBase(int intervalDelay, bool autoActivate = false, bool autoStart = false)
protected ProcessAgentBase(TimeSpan intervalDelay, bool autoActivate = false, bool autoStart = false)
```

**Example:**
```csharp
using SMEAppHouse.Core.ProcessService.Engines;

public class MyProcessAgent : ProcessAgentViaTask
{
    public MyProcessAgent() : base(TimeSpan.FromSeconds(5))
    {
        OnEngineStatusChanged += (sender, e) =>
        {
            Console.WriteLine($"Status changed to: {e.EngineStatus}");
        };
    }

    protected override void ServiceActionCallback()
    {
        // This method is called at the specified interval
        Console.WriteLine($"Processing at {DateTime.Now}");
        // Your processing logic here
    }

    internal override void ServiceActionInitialize()
    {
        // Called when the agent is activated
        Console.WriteLine("Agent initialized");
    }

    internal override void ServiceActionOnShutdown(int timeOut)
    {
        // Called when shutting down
        Console.WriteLine("Agent shutting down");
    }
}

// Usage
var agent = new MyProcessAgent();
await agent.Activate();
agent.Resume(); // Start processing

// Later...
agent.Suspend(5000); // Pause for 5 seconds
agent.Resume(); // Resume
agent.Shutdown(1000); // Shutdown with 1 second timeout
```

---

#### ProcessAgentViaTask

Base class for process agents using Task-based execution.

**Namespace**: `SMEAppHouse.Core.ProcessService.Engines`

**Inherits from**: `ProcessAgentBase`

**Constructors:**
```csharp
protected ProcessAgentViaTask()
protected ProcessAgentViaTask(int pauseMilliSeconds, bool autoInitialize = false, bool autoStart = false)
protected ProcessAgentViaTask(TimeSpan pauseTime, bool autoInitialize = false, bool autoStart = false)
```

**Example:**
```csharp
public class TaskBasedAgent : ProcessAgentViaTask
{
    public TaskBasedAgent() : base(1000, autoInitialize: true, autoStart: true)
    {
    }

    protected override void ServiceActionCallback()
    {
        // Executed every 1 second
        DoWork();
    }

    internal override void ServiceActionInitialize()
    {
        Console.WriteLine("Task-based agent initialized");
    }

    internal override void ServiceActionOnShutdown(int timeOut)
    {
        Console.WriteLine("Task-based agent shutting down");
    }

    private void DoWork()
    {
        // Your work here
    }
}
```

---

#### ProcessAgentViaThread

Base class for process agents using Thread-based execution.

**Namespace**: `SMEAppHouse.Core.ProcessService.Engines`

**Inherits from**: `ProcessAgentBase`

**Constructors:**
```csharp
protected ProcessAgentViaThread()
protected ProcessAgentViaThread(int pauseMilliSeconds, bool isBackground = true, bool autoInitialize = false, bool autoStart = false)
protected ProcessAgentViaThread(TimeSpan pauseTime, bool isBackground = true, bool autoInitialize = false, bool autoStart = false)
```

**Example:**
```csharp
public class ThreadBasedAgent : ProcessAgentViaThread
{
    public ThreadBasedAgent() : base(2000, isBackground: true, autoInitialize: false)
    {
    }

    protected override void ServiceActionCallback()
    {
        // Executed every 2 seconds on a background thread
        ProcessData();
    }

    internal override void ServiceActionInitialize()
    {
        Console.WriteLine("Thread-based agent initialized");
    }

    internal override void ServiceActionOnShutdown(int timeOut)
    {
        Console.WriteLine("Thread-based agent shutting down");
    }

    private void ProcessData()
    {
        // Your processing logic
    }
}
```

---

### 2. Interfaces

#### IProcessAgent

Interface for process agents.

**Namespace**: `SMEAppHouse.Core.ProcessService.Engines.Interfaces`

**Inherits from**: `IProcessAgentBasic`

**Properties:**
- `EngineStatusEnum EngineStatus`
- `TimeSpan IntervalDelay`
- `bool AutoStart`
- `bool AutoActivate`

**Events:**
- `BeforeEngineStatusChangedEventHandler OnBeforeEngineChanged`
- `EngineStatusChangedEventHandler OnEngineStatusChanged`

---

#### IProcessAgentBasic

Basic interface for process agent control.

**Namespace**: `SMEAppHouse.Core.ProcessService.Engines.Interfaces`

**Methods:**
```csharp
void Resume()
void Suspend(TimeSpan timeout)
void Suspend(int timeoutMilliSecs = 0)
void Shutdown(int timeout = 0)
```

---

### 3. Enums and Events

#### EngineStatusEnum

Enumeration for engine status.

**Namespace**: `SMEAppHouse.Core.ProcessService`

**Values:**
- `NonState` - Not running
- `RunningState` - Currently running
- `PausedState` - Paused

---

#### EngineStatusChangedEventArgs

Event arguments for engine status changes.

**Namespace**: `SMEAppHouse.Core.ProcessService`

**Properties:**
- `EngineStatusEnum EngineStatus`

---

#### BeforeEngineStatusChangedEventArgs

Event arguments for before engine status changes (allows cancellation).

**Namespace**: `SMEAppHouse.Core.ProcessService`

**Inherits from**: `EngineStatusChangedEventArgs`

**Properties:**
- `bool Cancel` - Set to true to cancel the status change

**Example:**
```csharp
agent.OnBeforeEngineChanged += (sender, e) =>
{
    if (e.EngineStatus == EngineStatusEnum.NonState)
    {
        // Prevent shutdown if important work is in progress
        if (IsImportantWorkInProgress())
            e.Cancel = true;
    }
};
```

---

### 4. Special Utilities

#### Forker

Utility for managing parallel operations with completion tracking.

**Namespace**: `SMEAppHouse.Core.ProcessService.Specials`

**Key Methods:**

```csharp
public Forker Fork(ThreadStart action)
public Forker Fork(ThreadStart action, object state)
public Forker OnItemComplete(EventHandler<ParallelEventArgs> handler)
public Forker OnAllComplete(EventHandler handler)
public void Join()
public bool Join(int millisecondsTimeout)
public int CountRunning()
```

**Events:**
- `EventHandler AllComplete` - Raised when all operations complete
- `EventHandler<ParallelEventArgs> ItemComplete` - Raised when each operation completes

**Example:**
```csharp
using SMEAppHouse.Core.ProcessService.Specials;

var forker = new Forker();

forker.OnItemComplete((sender, e) =>
{
    if (e.Exception != null)
        Console.WriteLine($"Error in {e.State}: {e.Exception.Message}");
    else
        Console.WriteLine($"Completed: {e.State}");
})
.OnAllComplete((sender, e) =>
{
    Console.WriteLine("All operations completed!");
});

// Fork multiple operations
forker.Fork(() => ProcessFile("file1.txt"), "file1")
      .Fork(() => ProcessFile("file2.txt"), "file2")
      .Fork(() => ProcessFile("file3.txt"), "file3");

// Wait for all to complete
forker.Join(5000); // Wait up to 5 seconds

void ProcessFile(string filename)
{
    // Process file logic
}
```

---

#### TaskMultiplexer<T>

Task multiplexer for processing multiple targets with worker pool.

**Namespace**: `SMEAppHouse.Core.ProcessService.Specials`

**Inherits from**: `ProcessAgentViaTask`

**Properties:**
- `int NumberOfWorkers` - Number of concurrent workers
- `Queue<TaskSlug> FIFOTargets` - Queue of targets to process

**Key Methods:**

```csharp
public virtual bool EnactOnTarget(T target)
```

**Example:**
```csharp
public class DataProcessor : TaskMultiplexer<DataItem>
{
    public DataProcessor() : base()
    {
        NumberOfWorkers = 5; // 5 concurrent workers
    }

    public override bool EnactOnTarget(DataItem target)
    {
        // Process the data item
        ProcessDataItem(target);
        return true;
    }

    protected override void ServiceActionCallback()
    {
        // Dequeue and process items
        while (FIFOTargets.Count > 0)
        {
            var slug = FIFOTargets.Dequeue();
            EnactOnTarget(slug.Target);
        }
    }

    private void ProcessDataItem(DataItem item)
    {
        // Processing logic
    }
}

// Usage
var processor = new DataProcessor();
processor.FIFOTargets.Enqueue(new TaskMultiplexer<DataItem>.TaskSlug(new DataItem()));
processor.FIFOTargets.Enqueue(new TaskMultiplexer<DataItem>.TaskSlug(new DataItem()));
await processor.Activate();
processor.Resume();
```

---

### 5. Exceptions

#### DoNotExecMethodException

Exception for preventing method execution.

**Namespace**: `SMEAppHouse.Core.ProcessService.Exceptions`

**Inherits from**: `Exception`

**Properties:**
- `Exception DetailException` - The inner exception or this exception

**Constructors:**
```csharp
public DoNotExecMethodException(string message)
public DoNotExecMethodException(string message, Exception inner)
```

---

## Complete Usage Examples

### Example 1: Simple Background Process

```csharp
using SMEAppHouse.Core.ProcessService.Engines;

public class DataSyncAgent : ProcessAgentViaTask
{
    public DataSyncAgent() : base(TimeSpan.FromMinutes(5))
    {
        OnEngineStatusChanged += (sender, e) =>
        {
            Logger.LogInformation($"Agent status: {e.EngineStatus}");
        };
    }

    protected override void ServiceActionCallback()
    {
        try
        {
            SyncData();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during data sync");
        }
    }

    internal override void ServiceActionInitialize()
    {
        Logger.LogInformation("Data sync agent initialized");
    }

    internal override void ServiceActionOnShutdown(int timeOut)
    {
        Logger.LogInformation("Data sync agent shutting down");
    }

    private void SyncData()
    {
        // Sync logic here
    }
}

// Usage
var agent = new DataSyncAgent();
await agent.Activate();
agent.Resume();
```

---

### Example 2: Process with Pause/Resume Control

```csharp
public class MonitoringAgent : ProcessAgentViaThread
{
    public MonitoringAgent() : base(1000, isBackground: true)
    {
        OnBeforeEngineChanged += (sender, e) =>
        {
            // Prevent shutdown during critical monitoring
            if (e.EngineStatus == EngineStatusEnum.NonState && IsCriticalMonitoring())
            {
                e.Cancel = true;
                Logger.LogWarning("Shutdown cancelled - critical monitoring in progress");
            }
        };
    }

    protected override void ServiceActionCallback()
    {
        CheckSystemHealth();
    }

    internal override void ServiceActionInitialize()
    {
        Logger.LogInformation("Monitoring agent started");
    }

    internal override void ServiceActionOnShutdown(int timeOut)
    {
        Logger.LogInformation("Monitoring agent stopped");
    }

    private void CheckSystemHealth()
    {
        // Health check logic
    }

    private bool IsCriticalMonitoring()
    {
        // Check if critical monitoring is active
        return false;
    }
}

// Usage
var monitor = new MonitoringAgent();
await monitor.Activate();
monitor.Resume();

// Pause for maintenance
monitor.Suspend(TimeSpan.FromMinutes(10));

// Resume after maintenance
monitor.Resume();

// Shutdown
monitor.Shutdown(5000);
```

---

### Example 3: Parallel Operations with Forker

```csharp
using SMEAppHouse.Core.ProcessService.Specials;

public class BatchProcessor
{
    public void ProcessBatch(List<string> files)
    {
        var forker = new Forker();

        int successCount = 0;
        int errorCount = 0;

        forker.OnItemComplete((sender, e) =>
        {
            if (e.Exception != null)
            {
                errorCount++;
                Logger.LogError(e.Exception, $"Error processing {e.State}");
            }
            else
            {
                successCount++;
                Logger.LogInformation($"Successfully processed {e.State}");
            }
        })
        .OnAllComplete((sender, e) =>
        {
            Logger.LogInformation($"Batch complete: {successCount} success, {errorCount} errors");
        });

        // Fork all file processing operations
        foreach (var file in files)
        {
            forker.Fork(() => ProcessFile(file), file);
        }

        // Wait for all to complete (with 5 minute timeout)
        bool completed = forker.Join(300000);

        if (!completed)
        {
            Logger.LogWarning($"Batch processing timed out. {forker.CountRunning()} operations still running");
        }
    }

    private void ProcessFile(string filename)
    {
        // File processing logic
    }
}
```

---

### Example 4: Task Multiplexer for Queue Processing

```csharp
public class QueueProcessor : TaskMultiplexer<WorkItem>
{
    public QueueProcessor() : base()
    {
        NumberOfWorkers = 10; // 10 concurrent workers
        IntervalDelay = TimeSpan.FromMilliseconds(100);
    }

    public override bool EnactOnTarget(WorkItem target)
    {
        try
        {
            ProcessWorkItem(target);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error processing work item {target.Id}");
            return false;
        }
    }

    protected override void ServiceActionCallback()
    {
        // Process items from queue
        int processed = 0;
        while (FIFOTargets.Count > 0 && processed < NumberOfWorkers)
        {
            var slug = FIFOTargets.Dequeue();
            if (EnactOnTarget(slug.Target))
            {
                slug.Success = true;
            }
            processed++;
        }
    }

    internal override void ServiceActionInitialize()
    {
        Logger.LogInformation("Queue processor initialized");
    }

    internal override void ServiceActionOnShutdown(int timeOut)
    {
        // Wait for remaining items
        while (FIFOTargets.Count > 0)
        {
            Thread.Sleep(100);
        }
        Logger.LogInformation("Queue processor shut down");
    }

    private void ProcessWorkItem(WorkItem item)
    {
        // Process work item
    }
}

// Usage
var processor = new QueueProcessor();

// Add items to queue
foreach (var item in workItems)
{
    processor.FIFOTargets.Enqueue(new TaskMultiplexer<WorkItem>.TaskSlug(item));
}

await processor.Activate();
processor.Resume();
```

---

## Key Features

1. **Multi-threading Support**: Both Task-based and Thread-based execution models
2. **Lifecycle Management**: Activate, Resume, Suspend, and Shutdown controls
3. **Event-driven**: Status change events for monitoring and control
4. **Cancellation Support**: Prevent status changes via event handlers
5. **Parallel Operations**: Forker utility for managing parallel tasks
6. **Queue Processing**: TaskMultiplexer for processing queues with worker pools
7. **Auto-start Options**: Automatic activation and startup
8. **Configurable Intervals**: Customizable delay between executions

---

## Dependencies

- None (pure .NET library)

---

## Notes

- Use `ProcessAgentViaTask` for modern async/await patterns
- Use `ProcessAgentViaThread` for traditional thread-based execution
- Background threads don't prevent application shutdown
- Always implement `ServiceActionOnShutdown` for cleanup
- Use `OnBeforeEngineChanged` event to prevent unwanted status changes
- TaskMultiplexer processes items from FIFO queue
- Forker is thread-safe and suitable for parallel operations

---

## License

Copyright © Nephiora IT Solutions 2025
