using FluentAssertions;
using NodaTime;
using SMEAppHouse.Core.Scheduler;
using System;
using Xunit;

namespace SMEAppHouse.Core.Scheduler.Tests;

public class ScheduleTests
{
    [Fact]
    public void Constructor_Default_ShouldInitializeWithDefaultValues()
    {
        // Act
        var schedule = new Schedule();

        // Assert
        schedule.Id.Should().NotBeEmpty();
        schedule.StartOfTime.Should().Be(default(LocalTime));
        schedule.EndOfTime.Should().Be(default(LocalTime).PlusSeconds(30));
        schedule.DayOfWeek.Should().BeNull();
        schedule.Title.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithStartOfTime_ShouldSetEndTime30SecondsLater()
    {
        // Arrange
        var startTime = LocalTime.FromHoursSinceMidnight(9 * 60);

        // Act
        var schedule = new Schedule(startTime);

        // Assert
        schedule.StartOfTime.Should().Be(startTime);
        schedule.EndOfTime.Should().Be(startTime.PlusSeconds(30));
    }

    [Fact]
    public void Constructor_WithStartAndEndTime_ShouldSetBothTimes()
    {
        // Arrange
        var startTime = LocalTime.FromHoursSinceMidnight(9 * 60);
        var endTime = LocalTime.FromHoursSinceMidnight(10 * 60);

        // Act
        var schedule = new Schedule(startTime, endTime);

        // Assert
        schedule.StartOfTime.Should().Be(startTime);
        schedule.EndOfTime.Should().Be(endTime);
    }

    [Fact]
    public void Constructor_WithLocalDate_ShouldSetDate()
    {
        // Arrange
        var date = new LocalDate(2025, 1, 15);

        // Act
        var schedule = new Schedule(date);

        // Assert
        schedule.StartOfTime.Should().Be(default(LocalTime));
        schedule.EndOfTime.Should().Be(default(LocalTime).PlusSeconds(30));
        schedule.Actual.Date.Should().Be(date);
    }

    [Fact]
    public void Constructor_WithDateAndTimes_ShouldSetAllProperties()
    {
        // Arrange
        var date = new LocalDate(2025, 1, 15);
        var startTime = LocalTime.FromHoursSinceMidnight(9 * 60);
        var endTime = LocalTime.FromHoursSinceMidnight(10 * 60);

        // Act
        var schedule = new Schedule(date, startTime, endTime);

        // Assert
        schedule.StartOfTime.Should().Be(startTime);
        schedule.EndOfTime.Should().Be(endTime);
        schedule.Actual.Date.Should().Be(date);
        schedule.Actual.TimeOfDay.Should().Be(startTime);
    }

    [Fact]
    public void Actual_ShouldReturnDatePlusStartTime()
    {
        // Arrange
        var date = new LocalDate(2025, 1, 15);
        var startTime = LocalTime.FromHoursSinceMidnight(14 * 60 + 30); // 14:30
        var schedule = new Schedule(date, startTime, startTime.PlusHours(1));

        // Act
        var actual = schedule.Actual;

        // Assert
        actual.Date.Should().Be(date);
        actual.TimeOfDay.Should().Be(startTime);
    }

    [Theory]
    [InlineData("9:00AM", 9, 0)]
    [InlineData("5:30PM", 17, 30)]
    [InlineData("12:00PM", 12, 0)]
    [InlineData("12:00AM", 0, 0)]
    [InlineData("11:59PM", 23, 59)]
    public void Parse_WithValidTime_ShouldParseCorrectly(string timeString, int expectedHour, int expectedMinute)
    {
        // Act
        var result = Schedule.Parse(timeString);

        // Assert
        result.Hour.Should().Be(expectedHour);
        result.Minute.Should().Be(expectedMinute);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("25:00")]
    [InlineData("")]
    [InlineData("9:00")]
    public void Parse_WithInvalidTime_ShouldThrowException(string invalidTime)
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => Schedule.Parse(invalidTime));
    }

    [Fact]
    public void GetTimezoneCurrentDateTime_ShouldReturnCurrentDateTime()
    {
        // Act
        var result = Schedule.GetTimezoneCurrentDateTime();

        // Assert
        result.Should().NotBeNull();
        var today = LocalDate.FromDateTime(DateTime.Now);
        var resultDate = result.LocalDateTime.Date;
        // Verify it's within 1 day (should be today)
        (resultDate - today).Days.Should().Be(0);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var schedule = new Schedule();
        var dayOfWeek = System.DayOfWeek.Monday;
        var title = "Test Schedule";

        // Act
        schedule.DayOfWeek = dayOfWeek;
        schedule.Title = title;

        // Assert
        schedule.DayOfWeek.Should().Be(dayOfWeek);
        schedule.Title.Should().Be(title);
    }
}

