using FluentAssertions;
using SMEAppHouse.Core.Reflections;
using Xunit;

namespace SMEAppHouse.Core.Reflections.Tests;

public class ExpressionBuilderTests
{
    public class TestEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    [Fact]
    public void GetExpression_WithEmptyFilters_ShouldReturnNull()
    {
        // Arrange
        var filters = new List<ExpressionBuilder.Filter>();

        // Act
        var expression = ExpressionBuilder.GetExpression<TestEntity>(filters);

        // Assert
        expression.Should().BeNull();
    }

    [Fact]
    public void GetExpression_WithEqualsOperator_ShouldCreateCorrectExpression()
    {
        // Arrange
        var filters = new List<ExpressionBuilder.Filter>
        {
            new()
            {
                PropertyName = "Name",
                Value = "John",
                Operator = ExpressionBuilder.Operator.Equals
            }
        };
        var entities = new List<TestEntity>
        {
            new() { Name = "John", Age = 30 },
            new() { Name = "Jane", Age = 25 }
        };

        // Act
        var expression = ExpressionBuilder.GetExpression<TestEntity>(filters);
        var result = entities.Where(expression!.Compile()).ToList();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("John");
    }

    [Fact]
    public void GetExpression_WithContainsOperator_ShouldCreateCorrectExpression()
    {
        // Arrange
        var filters = new List<ExpressionBuilder.Filter>
        {
            new()
            {
                PropertyName = "Name",
                Value = "Jo",
                Operator = ExpressionBuilder.Operator.Contains
            }
        };
        var entities = new List<TestEntity>
        {
            new() { Name = "John", Age = 30 },
            new() { Name = "Jane", Age = 25 }
        };

        // Act
        var expression = ExpressionBuilder.GetExpression<TestEntity>(filters);
        var result = entities.Where(expression!.Compile()).ToList();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("John");
    }

    [Fact]
    public void GetExpression_WithGreaterThanOperator_ShouldCreateCorrectExpression()
    {
        // Arrange
        var filters = new List<ExpressionBuilder.Filter>
        {
            new()
            {
                PropertyName = "Age",
                Value = "25",
                DataType = typeof(int),
                Operator = ExpressionBuilder.Operator.GreaterThan
            }
        };
        var entities = new List<TestEntity>
        {
            new() { Name = "John", Age = 30 },
            new() { Name = "Jane", Age = 25 },
            new() { Name = "Bob", Age = 20 }
        };

        // Act
        var expression = ExpressionBuilder.GetExpression<TestEntity>(filters);
        var result = entities.Where(expression!.Compile()).ToList();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("John");
    }

    [Fact]
    public void GetExpression_WithMultipleFilters_ShouldCombineWithAnd()
    {
        // Arrange
        var filters = new List<ExpressionBuilder.Filter>
        {
            new()
            {
                PropertyName = "Name",
                Value = "John",
                Operator = ExpressionBuilder.Operator.Equals
            },
            new()
            {
                PropertyName = "Age",
                Value = "30",
                DataType = typeof(int),
                Operator = ExpressionBuilder.Operator.Equals
            }
        };
        var entities = new List<TestEntity>
        {
            new() { Name = "John", Age = 30 },
            new() { Name = "John", Age = 25 },
            new() { Name = "Jane", Age = 30 }
        };

        // Act
        var expression = ExpressionBuilder.GetExpression<TestEntity>(filters);
        var result = entities.Where(expression!.Compile()).ToList();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("John");
        result.First().Age.Should().Be(30);
    }

    [Fact]
    public void GetExpression_WithStartsWithOperator_ShouldCreateCorrectExpression()
    {
        // Arrange
        var filters = new List<ExpressionBuilder.Filter>
        {
            new()
            {
                PropertyName = "Email",
                Value = "john",
                Operator = ExpressionBuilder.Operator.StartsWith
            }
        };
        var entities = new List<TestEntity>
        {
            new() { Name = "John", Email = "john@example.com" },
            new() { Name = "Jane", Email = "jane@example.com" }
        };

        // Act
        var expression = ExpressionBuilder.GetExpression<TestEntity>(filters);
        var result = entities.Where(expression!.Compile()).ToList();

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("john@example.com");
    }

    [Fact]
    public void GetExpression_WithEndsWithOperator_ShouldCreateCorrectExpression()
    {
        // Arrange
        var filters = new List<ExpressionBuilder.Filter>
        {
            new()
            {
                PropertyName = "Email",
                Value = "@example.com",
                Operator = ExpressionBuilder.Operator.EndsWith
            }
        };
        var entities = new List<TestEntity>
        {
            new() { Name = "John", Email = "john@example.com" },
            new() { Name = "Jane", Email = "jane@test.com" }
        };

        // Act
        var expression = ExpressionBuilder.GetExpression<TestEntity>(filters);
        var result = entities.Where(expression!.Compile()).ToList();

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("john@example.com");
    }
}

