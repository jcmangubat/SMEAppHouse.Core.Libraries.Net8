# SMEAppHouse.Core.Patterns.EF.Tests

Unit test project for `SMEAppHouse.Core.Patterns.EF` library.

## Overview

This test project provides comprehensive unit tests for the Entity Framework Core patterns library, covering:

- DbContext abstractions and extensions
- Entity base classes and interfaces
- Data access operation extensions
- Paging functionality
- Utility methods
- Exception handling

## Test Framework

- **xUnit** - Testing framework
- **FluentAssertions** - Fluent assertion library for better readability
- **Moq** - Mocking framework
- **Microsoft.EntityFrameworkCore.InMemory** - In-memory database for testing

## Test Structure

```
SMEAppHouse.Core.Patterns.EF.Tests/
├── DbContextAbstractions/
│   ├── AppDbContextExtendedTests.cs
│   └── AppDbContextBaseTests.cs
├── EntityCompositing/
│   └── Base/
│       └── KeyedEntityTests.cs
├── Exceptions/
│   └── EntityNotFoundExceptionTests.cs
├── Helpers/
│   ├── DataAccessOperationsExtensionsTests.cs
│   └── UtilitiesTests.cs
├── Paging/
│   ├── PageRequestTests.cs
│   ├── PagedResponseTests.cs
│   └── QueryableResponseTests.cs
└── TestHelpers/
    ├── TestDbContext.cs
    └── DbContextTestHelper.cs
```

## Running Tests

### Using Visual Studio
1. Open the solution
2. Build the solution (Ctrl+Shift+B)
3. Open Test Explorer (Test > Test Explorer)
4. Run All Tests

### Using .NET CLI
```bash
dotnet test SMEAppHouse.Core.Patterns.EF.Tests/SMEAppHouse.Core.Patterns.EF.Tests.csproj
```

### Using .NET CLI with Verbose Output
```bash
dotnet test SMEAppHouse.Core.Patterns.EF.Tests/SMEAppHouse.Core.Patterns.EF.Tests.csproj --verbosity normal
```

### Running Specific Tests
```bash
dotnet test --filter "FullyQualifiedName~AppDbContextExtendedTests"
```

## Test Coverage

### DbContext Tests
- Constructor validation
- SaveChanges with validation
- SaveChangesAsync with validation
- Factory method creation
- Options configuration

### Entity Tests
- Property initialization
- Date/time handling (UTC conversion)
- Primary key management
- Active status management

### Extension Method Tests
- Primary key property retrieval
- Filter by primary key
- Detach local entities
- Paging operations

### Utility Tests
- Property value copying
- Type conversion
- Nullable property handling

### Paging Tests
- Page request creation
- Paged response handling
- Queryable response handling

## Test Helpers

### TestDbContext
A test implementation of `AppDbContextExtended` used for unit testing.

### DbContextTestHelper
Helper methods for creating test DbContext instances:
- `CreateInMemoryContext()` - Creates an in-memory database context
- `CreateSqlServerContext()` - Creates a SQL Server context (for integration tests)

## Dependencies

- **SMEAppHouse.Core.Patterns.EF** - The library being tested
- **SMEAppHouse.Core.CodeKits** - Required dependency
- **SMEAppHouse.Core.AppMgt** - Required dependency

## Notes

- All tests use in-memory databases to avoid external dependencies
- Tests are isolated and can run in parallel
- Test data is created fresh for each test to ensure isolation

## Contributing

When adding new features to the main library, please add corresponding unit tests to ensure:
1. The feature works as expected
2. Edge cases are handled
3. Error conditions are properly handled
4. Performance is acceptable

