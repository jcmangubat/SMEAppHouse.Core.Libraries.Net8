# SMEAppHouse.Core.Scheduler

## Overview

`SMEAppHouse.Core.Scheduler` is a library for handling scheduled services. It provides a scheduler that monitors time-based schedules and raises events when specific time periods are reached, using NodaTime for time zone-aware date/time handling.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.Scheduler`

---

## Public Classes

### 1. Scheduler

Main scheduler class that monitors schedules and raises events when time periods are reached.

**Namespace**: `SMEAppHouse.Core.Scheduler`

**Inherits from**: `ProcessAgentViaTask`

**Key Properties:**
- `Schedule[] Schedules` - Array of schedules to monitor
- `Duration Duration` - Duration window for schedule detection (default: 30 seconds)
- `Schedule LastScheduleReached` - The last schedule that was reached

**Key Events:**
- `ScheduleReachedEventHandler OnScheduleReached` - Raised when a schedule is reached

**Constructors:**
```csharp
public Scheduler()
public Scheduler(Schedule[] schedules)
public Scheduler(Schedule[] schedules, Duration duration)
```

**Example:**
```csharp
using SMEAppHouse.Core.Scheduler;
using NodaTime;

// Create schedules
var schedules = new[]
{
    new Schedule(LocalTime.FromHoursSinceMidnight(9, 0), LocalTime.FromHoursSinceMidnight(9, 30))
    {
        Title = "Morning Meeting",
        DayOfWeek = DayOfWeek.Monday
    },
    new Schedule(LocalTime.FromHoursSinceMidnight(14, 0), LocalTime.FromHoursSinceMidnight(14, 30))
    {
        Title = "Afternoon Break"
    }
};

// Create scheduler with 30-second check interval
var scheduler = new Scheduler(schedules, Duration.FromSeconds(30));

// Subscribe to schedule events
scheduler.OnScheduleReached += (sender, e) =>
{
    Console.WriteLine($"Schedule reached: {e.NewSchedule.Title}");
    Console.WriteLine($"Time: {e.NewSchedule.StartOfTime}");
    
    // Perform action when schedule is reached
    PerformScheduledAction(e.NewSchedule);
};

// Activate and start
await scheduler.Activate();
scheduler.Resume();
```

---

### 2. Schedule

Represents a time schedule with optional day-of-week constraint.

**Namespace**: `SMEAppHouse.Core.Scheduler`

**Key Properties:**
- `Guid Id` - Unique identifier
- `DayOfWeek? DayOfWeek` - Optional day of week constraint
- `LocalTime StartOfTime` - Start time
- `LocalTime? EndOfTime` - End time (optional)
- `string Title` - Schedule title/description
- `LocalDateTime Actual` - Calculated actual date/time

**Constructors:**
```csharp
public Schedule()
public Schedule(LocalTime startOfTime)
public Schedule(LocalTime startOfTime, LocalTime endOfTime)
public Schedule(LocalDate setDate)
public Schedule(LocalDate setDate, LocalTime startOfTime, LocalTime endOfTime)
```

**Static Methods:**

##### GetTimezoneCurrentDateTime

```csharp
public static ZonedDateTime GetTimezoneCurrentDateTime()
```

Gets the current date/time in the system's time zone.

##### Parse

```csharp
public static LocalTime Parse(string time)
```

Parses a time string in format "h:mmtt" (e.g., "5:30PM").

**Example:**
```csharp
using NodaTime;

// Schedule for specific time (defaults to 30-second window)
var schedule1 = new Schedule(LocalTime.FromHoursSinceMidnight(9, 0));

// Schedule with start and end time
var schedule2 = new Schedule(
    LocalTime.FromHoursSinceMidnight(14, 0),
    LocalTime.FromHoursSinceMidnight(15, 0)
)
{
    Title = "Afternoon Window"
};

// Schedule for specific day of week
var schedule3 = new Schedule(LocalTime.FromHoursSinceMidnight(10, 0))
{
    Title = "Monday Meeting",
    DayOfWeek = DayOfWeek.Monday
};

// Parse time from string
var time = Schedule.Parse("5:30PM"); // 17:30:00
```

---

### 3. Helpers

Static helper class for schedule operations.

**Namespace**: `SMEAppHouse.Core.Scheduler`

**Key Methods:**

##### IsMomentInSchedule

```csharp
public static bool IsMomentInSchedule(Schedule[] schedules)
public static bool IsMomentInSchedule(Schedule[] schedules, out Schedule scheduleDetected)
```

Checks if the current moment is within any schedule.

##### GetScheduleOfTheMoment

```csharp
public static Schedule GetScheduleOfTheMoment(Schedule[] schedules)
public static Schedule GetScheduleOfTheMoment(Schedule[] schedules, Duration? duration)
```

Gets the schedule that matches the current moment.

##### GetScheduleForTime

```csharp
public static Schedule GetScheduleForTime(Schedule[] schedules, LocalDateTime current)
public static Schedule GetScheduleForTime(Schedule[] schedules, LocalDateTime current, Duration? duration)
```

Gets the schedule that matches a specific time.

**Extension Methods:**

##### GetTrailing

```csharp
public static Schedule GetTrailing(this IReadOnlyList<Schedule> schedules, Schedule schedule)
```

Gets the schedule that comes before the specified schedule.

**Example:**
```csharp
var schedules = new[]
{
    new Schedule(LocalTime.FromHoursSinceMidnight(9, 0)) { Title = "Morning" },
    new Schedule(LocalTime.FromHoursSinceMidnight(14, 0)) { Title = "Afternoon" },
    new Schedule(LocalTime.FromHoursSinceMidnight(18, 0)) { Title = "Evening" }
};

// Check if current moment is in a schedule
if (Helpers.IsMomentInSchedule(schedules, out var currentSchedule))
{
    Console.WriteLine($"Currently in: {currentSchedule.Title}");
}

// Get schedule for specific time
var time = LocalDateTime.FromDateTime(DateTime.Now);
var schedule = Helpers.GetScheduleForTime(schedules, time);

// Get previous schedule
var previous = schedules.GetTrailing(currentSchedule);
```

---

### 4. ScheduleReachedEventArg

Event arguments for schedule reached events.

**Namespace**: `SMEAppHouse.Core.Scheduler`

**Properties:**
- `Schedule NewSchedule` - The schedule that was reached
- `Schedule LastSchedule` - The previous schedule

**Example:**
```csharp
scheduler.OnScheduleReached += (sender, e) =>
{
    Console.WriteLine($"New schedule: {e.NewSchedule.Title}");
    if (e.LastSchedule != null)
    {
        Console.WriteLine($"Previous schedule: {e.LastSchedule.Title}");
    }
};
```

---

## Complete Usage Examples

### Example 1: Daily Schedule Monitoring

```csharp
using SMEAppHouse.Core.Scheduler;
using NodaTime;

public class DailyScheduleService
{
    private Scheduler _scheduler;

    public void StartMonitoring()
    {
        var schedules = new[]
        {
            new Schedule(LocalTime.FromHoursSinceMidnight(8, 0), LocalTime.FromHoursSinceMidnight(8, 30))
            {
                Title = "Morning Standup"
            },
            new Schedule(LocalTime.FromHoursSinceMidnight(12, 0), LocalTime.FromHoursSinceMidnight(13, 0))
            {
                Title = "Lunch Break"
            },
            new Schedule(LocalTime.FromHoursSinceMidnight(17, 0), LocalTime.FromHoursSinceMidnight(17, 30))
            {
                Title = "End of Day"
            }
        };

        _scheduler = new Scheduler(schedules, Duration.FromSeconds(30));
        _scheduler.OnScheduleReached += OnScheduleReached;
        
        _scheduler.Activate();
        _scheduler.Resume();
    }

    private void OnScheduleReached(object sender, ScheduleReachedEventArg e)
    {
        Console.WriteLine($"Schedule reached: {e.NewSchedule.Title} at {DateTime.Now}");
        
        switch (e.NewSchedule.Title)
        {
            case "Morning Standup":
                StartStandupMeeting();
                break;
            case "Lunch Break":
                NotifyLunchTime();
                break;
            case "End of Day":
                GenerateDailyReport();
                break;
        }
    }

    private void StartStandupMeeting() { }
    private void NotifyLunchTime() { }
    private void GenerateDailyReport() { }
}
```

---

### Example 2: Day-Specific Schedules

```csharp
public class WeeklyScheduleService
{
    public void SetupWeeklySchedules()
    {
        var schedules = new[]
        {
            // Monday morning meeting
            new Schedule(LocalTime.FromHoursSinceMidnight(9, 0))
            {
                Title = "Monday Morning Meeting",
                DayOfWeek = DayOfWeek.Monday
            },
            // Friday afternoon review
            new Schedule(LocalTime.FromHoursSinceMidnight(15, 0))
            {
                Title = "Friday Review",
                DayOfWeek = DayOfWeek.Friday
            },
            // Daily standup (all weekdays)
            new Schedule(LocalTime.FromHoursSinceMidnight(10, 0))
            {
                Title = "Daily Standup"
                // No DayOfWeek specified = every day
            }
        };

        var scheduler = new Scheduler(schedules, Duration.FromSeconds(60));
        scheduler.OnScheduleReached += (sender, e) =>
        {
            Console.WriteLine($"{e.NewSchedule.DayOfWeek}: {e.NewSchedule.Title}");
        };

        scheduler.Activate();
        scheduler.Resume();
    }
}
```

---

### Example 3: Time Window Monitoring

```csharp
public class TimeWindowService
{
    public void MonitorTimeWindows()
    {
        var schedules = new[]
        {
            // Business hours: 9 AM - 5 PM
            new Schedule(
                LocalTime.FromHoursSinceMidnight(9, 0),
                LocalTime.FromHoursSinceMidnight(17, 0)
            )
            {
                Title = "Business Hours"
            },
            // After hours: 5 PM - 9 AM next day
            new Schedule(
                LocalTime.FromHoursSinceMidnight(17, 0),
                LocalTime.FromHoursSinceMidnight(9, 0)
            )
            {
                Title = "After Hours"
            }
        };

        var scheduler = new Scheduler(schedules, Duration.FromSeconds(30));
        scheduler.OnScheduleReached += (sender, e) =>
        {
            if (e.NewSchedule.Title == "Business Hours")
            {
                EnableBusinessMode();
            }
            else if (e.NewSchedule.Title == "After Hours")
            {
                EnableAfterHoursMode();
            }
        };

        scheduler.Activate();
        scheduler.Resume();
    }

    private void EnableBusinessMode() { }
    private void EnableAfterHoursMode() { }
}
```

---

### Example 4: Schedule Validation

```csharp
public class ScheduleValidator
{
    public bool IsTimeInSchedule(LocalTime time, Schedule[] schedules)
    {
        var testDateTime = Schedule.GetTimezoneCurrentDateTime().Date.At(time);
        var schedule = Helpers.GetScheduleForTime(schedules, testDateTime);
        return schedule != null;
    }

    public Schedule GetCurrentSchedule(Schedule[] schedules)
    {
        return Helpers.GetScheduleOfTheMoment(schedules);
    }

    public Schedule GetNextSchedule(Schedule[] schedules, Schedule current)
    {
        var sorted = schedules.OrderBy(s => s.StartOfTime).ToList();
        var currentIndex = sorted.FindIndex(s => s.Id == current.Id);
        
        if (currentIndex >= 0 && currentIndex < sorted.Count - 1)
        {
            return sorted[currentIndex + 1];
        }
        
        return sorted.FirstOrDefault();
    }
}
```

---

## Key Features

1. **Time Zone Awareness**: Uses NodaTime for proper time zone handling
2. **Day-of-Week Constraints**: Optional day-of-week filtering
3. **Time Windows**: Support for start and end times
4. **Event-Driven**: Raises events when schedules are reached
5. **Duration Windows**: Configurable detection windows
6. **Process Agent Integration**: Built on ProcessAgentViaTask for background monitoring

---

## Dependencies

- NodaTime (v3.2.2)
- SMEAppHouse.Core.ProcessService

---

## Notes

- Scheduler runs as a background task using ProcessAgentViaTask
- Default duration window is 30 seconds
- Schedules without DayOfWeek apply to all days
- EndOfTime defaults to StartOfTime + 30 seconds if not specified
- Events are raised only when entering a new schedule (not continuously)
- Use NodaTime types (LocalTime, LocalDate, Duration) for all time operations
- Time parsing supports format "h:mmtt" (e.g., "5:30PM")

---

## License

Copyright © Nephiora IT Solutions 2025
