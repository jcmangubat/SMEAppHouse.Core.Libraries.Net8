# SMEAppHouse.Core.QuartzExt

## Overview

`SMEAppHouse.Core.QuartzExt` is an extension library for implementing Quartz scheduling services. It provides base classes and utilities for creating scheduled jobs using the Quartz.NET scheduler with a simplified API and singleton pattern support.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.QuartzExt`

---

## Public Classes and Interfaces

### 1. Job Service Base Classes

#### JobServiceBase<T>

Abstract base class for Quartz job services implementing the Curiously Recurring Template Pattern (CRTP) for singleton behavior.

**Namespace**: `SMEAppHouse.Core.QuartzExt`

**Implements**: `IJobService`

**Key Features:**
- Singleton pattern via static `Instance` property
- Thread-safe execution with mutex locks
- Prevents concurrent execution of the same job

**Key Properties:**

##### Static Instance

```csharp
public static T Instance { get; }
```

Returns the singleton instance of the job service. Creates and initializes the instance on first access.

**Key Methods:**

##### Abstract Methods (Must Implement)

```csharp
public abstract void SubscriberInitialize()
public abstract void SubscriberExecute()
```

##### Execute

```csharp
public void Execute(ThreadPriority? threadPrio = ThreadPriority.Normal)
```

Executes the job service in a separate thread with optional thread priority. Prevents concurrent execution.

**Example:**
```csharp
using SMEAppHouse.Core.QuartzExt;

public class DataSyncJob : JobServiceBase<DataSyncJob>
{
    public override void SubscriberInitialize()
    {
        // Called once when instance is created
        Console.WriteLine("DataSyncJob initialized");
        // Initialize resources, connections, etc.
    }

    public override void SubscriberExecute()
    {
        // Called each time the job executes
        Console.WriteLine($"DataSyncJob executing at {DateTime.Now}");
        
        try
        {
            // Your job logic here
            SyncData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DataSyncJob: {ex.Message}");
        }
    }

    private void SyncData()
    {
        // Data synchronization logic
    }
}

// Usage - Get singleton instance
var job = DataSyncJob.Instance;

// Execute manually (optional - usually called by Quartz scheduler)
job.Execute(ThreadPriority.Normal);
```

---

#### JobServiceMemberBase<T>

Base class for job service members that can run as background threads.

**Namespace**: `SMEAppHouse.Core.QuartzExt`

**Implements**: `IJobServiceMember`

**Key Properties:**
- `bool AsBackground` - Run as background thread
- `bool AutoRun` - Automatically start on construction
- `bool Success` - Execution success status
- `Exception Exception` - Exception if execution failed
- `bool Executing` - Whether currently executing
- `Thread InstanceThread` - The thread instance
- `Action InstanceAction` - Action to execute

**Key Methods:**

```csharp
public virtual void ExecuteInstance()
public void Start()
public void DestroySelf()
```

**Events:**
- `SelfDestroyerDelegate OnSelfDestructNow` - Raised when destroying the instance

**Example:**
```csharp
public class BackgroundWorker : JobServiceMemberBase<BackgroundWorker>
{
    public BackgroundWorker() : base()
    {
        AsBackground = true;
        AutoRun = false;
        InstanceAction = DoWork;
    }

    public override void ExecuteInstance()
    {
        base.ExecuteInstance();
        try
        {
            InstanceAction?.Invoke();
            Success = true;
        }
        catch (Exception ex)
        {
            Exception = ex;
            Success = false;
        }
    }

    private void DoWork()
    {
        // Background work logic
    }
}

// Usage
var worker = new BackgroundWorker();
worker.Start();
```

---

### 2. Job Service Starter

#### JobServiceStarter<T>

Static helper class for starting Quartz scheduled jobs.

**Namespace**: `SMEAppHouse.Core.QuartzExt`

**Key Methods:**

##### Start

```csharp
public static async void Start(
    int recurrenceInterval = 60,
    Rules.TimeIntervalTypesEnum intervalType = Rules.TimeIntervalTypesEnum.Seconds,
    string jobGroupName = "",
    string triggerGroupName = "")
```

Starts a Quartz scheduler for the specified job service type.

**Parameters:**
- `recurrenceInterval` - Interval between job executions (default: 60)
- `intervalType` - Type of interval (Seconds, Minutes, Hours, Days, etc.)
- `jobGroupName` - Optional job group name
- `triggerGroupName` - Optional trigger group name

**Example:**
```csharp
using SMEAppHouse.Core.QuartzExt;
using SMEAppHouse.Core.CodeKits;

// Start job to run every 5 minutes
JobServiceStarter<DataSyncJob>.Start(
    recurrenceInterval: 5,
    intervalType: Rules.TimeIntervalTypesEnum.Minutes
);

// Start job to run every 30 seconds
JobServiceStarter<DataSyncJob>.Start(
    recurrenceInterval: 30,
    intervalType: Rules.TimeIntervalTypesEnum.Seconds
);

// Start job to run every hour
JobServiceStarter<DataSyncJob>.Start(
    recurrenceInterval: 1,
    intervalType: Rules.TimeIntervalTypesEnum.Hours
);

// Start job with custom group names
JobServiceStarter<DataSyncJob>.Start(
    recurrenceInterval: 10,
    intervalType: Rules.TimeIntervalTypesEnum.Minutes,
    jobGroupName: "SyncJobs",
    triggerGroupName: "SyncTriggers"
);
```

---

#### JobServiceInstance<TJobSvc>

Internal Quartz job wrapper that executes the job service.

**Namespace**: `SMEAppHouse.Core.QuartzExt` (nested in `JobServiceStarter<T>`)

**Implements**: `IJob`

**Properties:**
- `JobServiceBase<TJobSvc> Instance` - The job service instance

**Methods:**
```csharp
public Task Execute(IJobExecutionContext context)
```

This class is used internally by Quartz to execute the job service.

---

### 3. Interfaces

#### IJobService

Interface for job services.

**Namespace**: `SMEAppHouse.Core.QuartzExt`

**Methods:**
```csharp
void Execute(ThreadPriority? threadPrio = ThreadPriority.Normal)
void SubscriberInitialize()
void SubscriberExecute()
```

---

#### IJobServiceMember

Interface for job service members.

**Namespace**: `SMEAppHouse.Core.QuartzExt`

**Properties:**
- `bool AsBackground`
- `bool AutoRun`
- `bool Success`
- `Exception Exception`
- `bool Executing`
- `Thread InstanceThread`

**Methods:**
```csharp
void Start()
void ExecuteInstance()
void DestroySelf()
```

---

### 4. Time Interval Types

The library uses `Rules.TimeIntervalTypesEnum` from `SMEAppHouse.Core.CodeKits`:

- `MilliSeconds` - Milliseconds
- `Seconds` - Seconds
- `Minutes` - Minutes
- `Hours` - Hours
- `Days` - Days
- `Months` - Months
- `Years` - Years

---

## Complete Usage Examples

### Example 1: Simple Scheduled Job

```csharp
using SMEAppHouse.Core.QuartzExt;
using SMEAppHouse.Core.CodeKits;

public class EmailNotificationJob : JobServiceBase<EmailNotificationJob>
{
    public override void SubscriberInitialize()
    {
        // Initialize email service, database connections, etc.
        Console.WriteLine("EmailNotificationJob initialized");
    }

    public override void SubscriberExecute()
    {
        try
        {
            Console.WriteLine($"Checking for pending emails at {DateTime.Now}");
            
            // Get pending emails from database
            var pendingEmails = GetPendingEmails();
            
            foreach (var email in pendingEmails)
            {
                SendEmail(email);
                MarkAsSent(email);
            }
            
            Console.WriteLine($"Processed {pendingEmails.Count} emails");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in EmailNotificationJob: {ex.Message}");
            // Log error
        }
    }

    private List<Email> GetPendingEmails()
    {
        // Database query logic
        return new List<Email>();
    }

    private void SendEmail(Email email)
    {
        // Email sending logic
    }

    private void MarkAsSent(Email email)
    {
        // Update database
    }
}

// In your application startup (e.g., Program.cs or Startup.cs)
public void ConfigureServices(IServiceCollection services)
{
    // Start the job to run every 2 minutes
    JobServiceStarter<EmailNotificationJob>.Start(
        recurrenceInterval: 2,
        intervalType: Rules.TimeIntervalTypesEnum.Minutes
    );
}
```

---

### Example 2: Multiple Scheduled Jobs

```csharp
public class DataBackupJob : JobServiceBase<DataBackupJob>
{
    public override void SubscriberInitialize()
    {
        Console.WriteLine("DataBackupJob initialized");
    }

    public override void SubscriberExecute()
    {
        Console.WriteLine($"Running backup at {DateTime.Now}");
        PerformBackup();
    }

    private void PerformBackup()
    {
        // Backup logic
    }
}

public class HealthCheckJob : JobServiceBase<HealthCheckJob>
{
    public override void SubscriberInitialize()
    {
        Console.WriteLine("HealthCheckJob initialized");
    }

    public override void SubscriberExecute()
    {
        Console.WriteLine($"Health check at {DateTime.Now}");
        CheckSystemHealth();
    }

    private void CheckSystemHealth()
    {
        // Health check logic
    }
}

// Start multiple jobs
public void StartAllJobs()
{
    // Backup every 6 hours
    JobServiceStarter<DataBackupJob>.Start(
        recurrenceInterval: 6,
        intervalType: Rules.TimeIntervalTypesEnum.Hours
    );

    // Health check every 30 seconds
    JobServiceStarter<HealthCheckJob>.Start(
        recurrenceInterval: 30,
        intervalType: Rules.TimeIntervalTypesEnum.Seconds
    );
}
```

---

### Example 3: Job with Resource Management

```csharp
public class DatabaseCleanupJob : JobServiceBase<DatabaseCleanupJob>
{
    private IDbConnection _connection;
    private bool _disposed = false;

    public override void SubscriberInitialize()
    {
        // Initialize database connection
        _connection = CreateDatabaseConnection();
        Console.WriteLine("DatabaseCleanupJob initialized");
    }

    public override void SubscriberExecute()
    {
        try
        {
            Console.WriteLine($"Starting database cleanup at {DateTime.Now}");
            
            // Clean up old records
            CleanupOldRecords();
            
            // Optimize tables
            OptimizeTables();
            
            Console.WriteLine("Database cleanup completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during cleanup: {ex.Message}");
            throw;
        }
    }

    private void CleanupOldRecords()
    {
        // Cleanup logic using _connection
    }

    private void OptimizeTables()
    {
        // Optimization logic
    }

    private IDbConnection CreateDatabaseConnection()
    {
        // Create and return connection
        return null; // Placeholder
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }
}
```

---

### Example 4: Job with Configuration

```csharp
public class ReportGenerationJob : JobServiceBase<ReportGenerationJob>
{
    private readonly IConfiguration _configuration;
    private string _reportPath;

    public ReportGenerationJob()
    {
        // Note: In a real scenario, you might use dependency injection
        // For singleton pattern, you may need to access configuration differently
    }

    public override void SubscriberInitialize()
    {
        // Load configuration
        _reportPath = GetReportPath();
        Console.WriteLine($"ReportGenerationJob initialized. Output path: {_reportPath}");
    }

    public override void SubscriberExecute()
    {
        try
        {
            var reportDate = DateTime.Now;
            Console.WriteLine($"Generating report for {reportDate:yyyy-MM-dd}");
            
            var report = GenerateReport(reportDate);
            SaveReport(report, reportDate);
            
            Console.WriteLine("Report generated successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating report: {ex.Message}");
        }
    }

    private Report GenerateReport(DateTime date)
    {
        // Report generation logic
        return new Report();
    }

    private void SaveReport(Report report, DateTime date)
    {
        var filename = Path.Combine(_reportPath, $"report_{date:yyyyMMdd}.pdf");
        // Save report logic
    }

    private string GetReportPath()
    {
        // Get from configuration or default
        return @"C:\Reports";
    }
}

// Start job to run daily at specific interval
JobServiceStarter<ReportGenerationJob>.Start(
    recurrenceInterval: 24,
    intervalType: Rules.TimeIntervalTypesEnum.Hours
);
```

---

### Example 5: Manual Execution (Without Scheduler)

```csharp
public class OnDemandJob : JobServiceBase<OnDemandJob>
{
    public override void SubscriberInitialize()
    {
        Console.WriteLine("OnDemandJob initialized");
    }

    public override void SubscriberExecute()
    {
        Console.WriteLine("Executing on-demand job");
        ProcessOnDemand();
    }

    private void ProcessOnDemand()
    {
        // Processing logic
    }
}

// Execute manually without Quartz scheduler
public void ExecuteManually()
{
    var job = OnDemandJob.Instance;
    
    // Execute with normal priority
    job.Execute();
    
    // Or execute with high priority
    job.Execute(ThreadPriority.Highest);
}
```

---

### Example 6: Job Service Member Usage

```csharp
public class BackgroundProcessor : JobServiceMemberBase<BackgroundProcessor>
{
    private Queue<WorkItem> _workQueue = new();

    public BackgroundProcessor() : base()
    {
        AsBackground = true;
        AutoRun = true; // Start automatically
        InstanceAction = ProcessWorkQueue;
    }

    public override void ExecuteInstance()
    {
        base.ExecuteInstance();
        try
        {
            InstanceAction?.Invoke();
            Success = true;
        }
        catch (Exception ex)
        {
            Exception = ex;
            Success = false;
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ProcessWorkQueue()
    {
        while (_workQueue.Count > 0)
        {
            var item = _workQueue.Dequeue();
            ProcessItem(item);
        }
    }

    public void Enqueue(WorkItem item)
    {
        _workQueue.Enqueue(item);
    }

    private void ProcessItem(WorkItem item)
    {
        // Process work item
    }
}

// Usage
var processor = new BackgroundProcessor(); // Auto-starts
processor.Enqueue(new WorkItem());
```

---

## Key Features

1. **Singleton Pattern**: Automatic singleton management via CRTP
2. **Thread Safety**: Mutex locks prevent concurrent execution
3. **Quartz Integration**: Seamless integration with Quartz.NET scheduler
4. **Flexible Scheduling**: Support for various time intervals (seconds, minutes, hours, days, etc.)
5. **Background Execution**: Jobs run in separate threads
6. **Lifecycle Management**: Initialize and execute methods for proper resource management
7. **Error Handling**: Built-in exception handling in execution flow

---

## Dependencies

- Quartz (v3.15.0)
- SMEAppHouse.Core.CodeKits

---

## Notes

- Jobs are singletons - only one instance exists per job type
- `SubscriberInitialize()` is called once when the instance is first created
- `SubscriberExecute()` is called each time the job runs
- Use `JobServiceStarter<T>.Start()` to schedule jobs with Quartz
- Jobs can also be executed manually via `Instance.Execute()`
- Thread priority can be specified for manual execution
- The scheduler uses binary serialization by default
- Job and trigger names are auto-generated based on the job type name
- Custom group names can be specified for organization

---

## License

Copyright © Nephiora IT Solutions 2025
