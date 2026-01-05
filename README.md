# SMEAppHouse.Core Solution

## Overview

SMEAppHouse.Core is a comprehensive .NET 8.0 solution containing multiple reusable library projects organized into logical categories. The solution provides patterns, utilities, and integrations for building enterprise-level applications.

**Target Framework**: .NET 8.0  
**Total Projects**: 30+ library projects  
**Last Updated**: January 2025

---

## Solution Structure

The solution contains **2 main solution files**:
- `SMEAppHouse.Patterns.sln` - Contains core pattern libraries
- `SMEAppHouse.Core.AllLibs.sln` - Contains all library projects

### Shared Resources
- `.shared\common-assembly-info.proj` - Common assembly information (version, copyright, package metadata)
- `.documentations\` - All project documentation files

---

## Project Categories

### 1. Core Utilities & Helpers

#### SMEAppHouse.Core.CodeKits
- **Target Framework**: .NET 8.0
- **Purpose**: Library of reusable functions and procedures
- **Key Features**:
  - File operations (FileHelper)
  - Cryptographic utilities (Cryptor)
  - Data manipulation helpers
  - Extension methods for primitives, strings, dates
  - Expression builders and reflection helpers
  - Geo utilities
  - JSON/XML parsing helpers
- **Documentation**: `.documentations/SMEAppHouse.Core.CodeKits.README.md` (854 lines)

#### SMEAppHouse.Core.Reflections
- **Target Framework**: .NET 8.0
- **Purpose**: Library of useful reusable functions and procedures extending EF based on patterns
- **Key Features**:
  - Dynamic LINQ expression building
  - Filter collections with JSON support
  - Dynamic type creation
- **Documentation**: `.documentations/SMEAppHouse.Core.Reflections_README.md` (348 lines)

---

### 2. Data Access & Repository Patterns

#### SMEAppHouse.Core.Patterns.EF
- **Target Framework**: .NET 8.0
- **Purpose**: Patterns library for implementing entity structural and adapter patterns
- **Key Features**:
  - AppDbContextExtended<TDbContext> - Extended DbContext with migration support
  - DbEntityCfg<TEntity> - Fluent API entity configuration
  - IDbMigrationInformation - Migration metadata interface
  - Base entity classes (KeyedEntity, KeyedAuditableEntity)
  - Paging support (PagedResponse, PageRequest)
- **Dependencies**: 
  - Microsoft.EntityFrameworkCore (v9.0.9)
  - Microsoft.EntityFrameworkCore.SqlServer (v9.0.9)
- **Documentation**: `.documentations/SMEAppHouse.Core.Patterns.EF_README.md` (692 lines)

#### SMEAppHouse.Core.Patterns.Repo
- **Target Framework**: .NET 8.0
- **Purpose**: Library for implementing generic data composite modeling and repository pattern strategy
- **Key Features**:
  - Generic repository implementations
  - Repository for keyed entities (Guid, Int)
  - Paging support (PagedResult, PagingRequest)
  - Queryable result handling
  - Retry logic for resilient operations
- **Documentation**: `.documentations/SMEAppHouse.Core.Patterns.Repo_README.md` (617 lines)

#### SMEAppHouse.Core.Patterns.Repo.V2
- **Target Framework**: .NET 8.0
- **Purpose**: Enhanced repository patterns library with improved API design
- **Key Features**:
  - Both synchronous and asynchronous interfaces
  - Auto-save option for immediate persistence
  - Comprehensive querying (filtering, ordering, paging, includes)
  - Raw SQL support
  - Unit of Work pattern
- **Documentation**: `.documentations/SMEAppHouse.Core.Patterns.Repo.V2_README.md` (329 lines)

---

### 3. Web & API Patterns

#### SMEAppHouse.Core.Patterns.WebApi
- **Target Framework**: .NET 8.0
- **Purpose**: Patterns library for implementing web API
- **Key Features**:
  - WebApiServiceHost<TEntity, TPk> - Base API controller
  - IWebApiServiceHost - API host interface
  - Standard CRUD endpoints
  - Error handling and JSON serialization
- **Documentation**: `.documentations/SMEAppHouse.Core.Patterns.WebApi_README.md` (500 lines)

#### SMEAppHouse.Core.RestSharpClientLib
- **Target Framework**: .NET 8.0
- **Purpose**: Extension wrapper library for using the RestSharp package
- **Key Features**:
  - ApiClient - HTTP client wrapper
  - ApiResponse<T> - Typed response handling
  - Configuration management
- **Documentation**: `.documentations/SMEAppHouse.Core.RestSharpClientLib.README.md` (225 lines)

---

### 4. Application Management & Services

#### SMEAppHouse.Core.AppMgt
- **Target Framework**: .NET 8.0
- **Purpose**: Library for setting app configurations
- **Key Features**:
  - ServiceStarter<T> - Abstract service starter with timer support
  - IServiceStarter<T> - Service starter interface
  - ServicePulseBehaviorEnum - Synchronous/Asynchronous execution modes
  - Configuration management
  - Messaging support (IPayloadsEnvelope)
- **Dependencies**:
  - AutoMapper
  - Microsoft.Extensions.DependencyInjection.Abstractions
- **Documentation**: `.documentations/SMEAppHouse.Core.AppMgt.README.md` (326 lines)

#### SMEAppHouse.Core.ProcessService
- **Target Framework**: .NET 8.0
- **Purpose**: Library for implementing multi-threaded processes
- **Key Features**:
  - ProcessAgentBase - Base class for process agents
  - ProcessAgentViaThread - Thread-based execution
  - ProcessAgentViaTask - Task-based execution
  - Pause/resume functionality
- **Documentation**: `.documentations/SMEAppHouse.Core.ProcessService_README.md` (546 lines)

---

### 5. Scheduling & Background Jobs

#### SMEAppHouse.Core.QuartzExt
- **Target Framework**: .NET 8.0
- **Purpose**: Extension library for implementing Quartz scheduling services
- **Key Features**:
  - JobServiceBase<T> - Base class for Quartz jobs (CRTP pattern)
  - JobServiceStarter<T> - Helper for starting scheduled jobs
  - Singleton pattern implementation
  - Job execution management
- **Dependencies**: Quartz (v3.15.0)
- **Documentation**: `.documentations/SMEAppHouse.Core.QuartzExt_README.md` (552 lines)

#### SMEAppHouse.Core.Scheduler
- **Target Framework**: .NET 8.0
- **Purpose**: Library for handling scheduled services
- **Key Features**:
  - Schedule class - Time-based schedules with day-of-week constraints
  - Scheduler class - Monitors schedules and raises events
  - Time zone awareness (NodaTime)
- **Dependencies**: NodaTime (v3.2.2)
- **Documentation**: `.documentations/SMEAppHouse.Core.Scheduler_README.md` (363 lines)

---

### 6. Windows Service Integration

#### SMEAppHouse.Core.TopshelfAdapter
- **Target Framework**: .NET 8.0
- **Purpose**: Library for handling Topshelf implemented Windows service
- **Key Features**:
  - TopshelfSocket<T> - Abstract base class for services
  - Lifecycle management (Initialize, Resume, Suspend, Shutdown)
  - Lazy initialization support
- **Documentation**: `.documentations/SMEAppHouse.Core.TopshelfAdapter_README.md` (208 lines)

#### SMEAppHouse.Core.TopshelfAdapter.Common
- **Target Framework**: .NET 8.0
- **Purpose**: Container of common members shared across Topshelf adapter implementation
- **Key Features**:
  - ITopshelfClient interface
  - RuntimeBehaviorOptions configuration
  - Initialization status enums
- **Documentation**: `.documentations/SMEAppHouse.Core.TopshelfAdapter.Common_README.md` (112 lines)

#### SMEAppHouse.Core.TopshelfAdapter.Aggregation
- **Target Framework**: .NET 8.0
- **Purpose**: Library for handling collection of Topshelf implemented Windows service
- **Key Features**:
  - ServiceController - Manages multiple service workers
  - Unified control operations (ResumeAll, HaltAll, ShutdownAll)
- **Documentation**: `.documentations/SMEAppHouse.Core.TopshelfAdapter.Aggregation_README.md` (155 lines)

---

### 7. Web Scraping & Automation

#### SMEAppHouse.Core.ScraperBox
- **Target Framework**: .NET 8.0
- **Purpose**: Library for web scraping operations
- **Key Features**:
  - Helper class - Web page fetching, HTML parsing
  - ContentGenerator - Queue-based HTML content generator with ProcessAgentViaTask
  - IPProxy model - Proxy server representation
  - PageInstruction - Pagination support
  - UserAgents - Predefined user agent strings
  - HtmlSource/HtmlTarget models - Source configuration and target processing
  - IContentGenerator interface - Content generation abstraction
- **Dependencies**:
  - HtmlAgilityPack (v1.12.3)
  - ScrapySharp (v3.0.0)
  - SMEAppHouse.Core.ProcessService (for ProcessAgentViaTask)
- **Documentation**: `.documentations/SMEAppHouse.Core.ScraperBox_README.md` (447+ lines)

#### SMEAppHouse.Core.SeleniumExt
- **Target Framework**: .NET 8.0
- **Purpose**: Library for handling Selenium functionalities
- **Key Features**:
  - Extension methods for IWebDriver
  - Element waiting utilities
  - Full-page screenshot support
  - Attribute access helpers
- **Dependencies**:
  - Selenium.WebDriver (v4.35.0)
  - Selenium.Support (v4.35.0)
- **Documentation**: `.documentations/SMEAppHouse.Core.SeleniumExt_README.md` (340 lines)
- **Recent Updates** (January 2025):
  - Verified all GPS.Frameworks.SeleniumHelpers functionality already present

---

## Documentation

All project documentation is located in the `.documentations` folder. Each project has a comprehensive README.md file that includes:

1. **Overview** - Purpose and target framework
2. **Public Classes and Interfaces** - Complete list with descriptions
3. **Public Methods** - Method signatures and descriptions
4. **Usage Examples** - Complete code examples
5. **Dependencies** - Required packages
6. **Notes** - Important information and best practices
7. **License** - Copyright information

### Available Documentation Files

| Project | Documentation File | Lines |
|---------|-------------------|-------|
| CodeKits | `SMEAppHouse.Core.CodeKits.README.md` | 854 |
| AppMgt | `SMEAppHouse.Core.AppMgt.README.md` | 326 |
| Patterns.EF | `SMEAppHouse.Core.Patterns.EF_README.md` | 692 |
| Patterns.Repo | `SMEAppHouse.Core.Patterns.Repo_README.md` | 617 |
| Patterns.Repo.V2 | `SMEAppHouse.Core.Patterns.Repo.V2_README.md` | 329 |
| Patterns.WebApi | `SMEAppHouse.Core.Patterns.WebApi_README.md` | 500 |
| ProcessService | `SMEAppHouse.Core.ProcessService_README.md` | 546 |
| QuartzExt | `SMEAppHouse.Core.QuartzExt_README.md` | 552 |
| Reflections | `SMEAppHouse.Core.Reflections_README.md` | 348 |
| RestSharpClientLib | `SMEAppHouse.Core.RestSharpClientLib.README.md` | 225 |
| Scheduler | `SMEAppHouse.Core.Scheduler_README.md` | 363 |
| ScraperBox | `SMEAppHouse.Core.ScraperBox_README.md` | 447 |
| SeleniumExt | `SMEAppHouse.Core.SeleniumExt_README.md` | 340 |
| TopshelfAdapter | `SMEAppHouse.Core.TopshelfAdapter_README.md` | 208 |
| TopshelfAdapter.Common | `SMEAppHouse.Core.TopshelfAdapter.Common_README.md` | 112 |
| TopshelfAdapter.Aggregation | `SMEAppHouse.Core.TopshelfAdapter.Aggregation_README.md` | 155 |

### How to Use Documentation

1. Navigate to the `.documentations` folder
2. Open the README.md file for the project you're interested in
3. Find the class, interface, or method you need
4. Review the usage examples
5. Copy and adapt the code samples for your needs

---

## Project Dependencies

### Core Dependencies
- **SMEAppHouse.Core.CodeKits** - Used by most projects as a base utility library
- **SMEAppHouse.Core.Patterns.EF** - Used by repository pattern projects
- **SMEAppHouse.Core.Patterns.Repo** - Used by higher-level data access projects

### Dependency Graph
```
CodeKits (Base)
  ├── Patterns.EF
  │     └── Patterns.Repo
  │           └── Patterns.Repo.V2
  ├── AppMgt
  │     └── TopshelfAdapter.*
  ├── ScraperBox
  │     └── SeleniumExt
  └── ProcessService
        └── Scheduler
```

---

## Key Technologies & Packages

### Entity Framework
- Microsoft.EntityFrameworkCore (v9.0.9)
- Microsoft.EntityFrameworkCore.SqlServer (v9.0.9)

### Web & HTTP
- RestSharp (v112.1.0)
- System.Net.Http

### Web Scraping
- Selenium.WebDriver (v4.35.0)
- Selenium.Support (v4.35.0)
- HtmlAgilityPack (v1.12.3)
- ScrapySharp (v3.0.0)

### Scheduling
- Quartz.NET (v3.15.0)
- NodaTime (v3.2.2)

### Utilities
- AutoMapper (v13.0.1)
- Newtonsoft.Json (v13.0.4)
- Microsoft.Extensions.DependencyInjection.Abstractions (v8.0.1)

---

## Build Status

All projects are targeting **.NET 8.0**, providing:
- Modern language features (C# 12)
- Improved performance
- Better async/await support
- Enhanced security
- Long-term support (LTS)

### Common Assembly Information

All projects use shared assembly information from `.shared\common-assembly-info.proj`:
- **Version**: 9.0.7.00
- **Authors**: James Mangubat
- **Company**: SME App House
- **Copyright**: Copyright © Nephiora IT Solutions 2025
- **Package Tags**: .Net Core, SMEAppHouse, SMEDigital

---

## Quick Start

### Using a Library

1. **Add Project Reference**:
   ```xml
   <ProjectReference Include="..\SMEAppHouse.Core.CodeKits\SMEAppHouse.Core.CodeKits.csproj" />
   ```

2. **Add Using Statement**:
   ```csharp
   using SMEAppHouse.Core.CodeKits;
   using SMEAppHouse.Core.CodeKits.Helpers;
   ```

3. **Use the Library**:
   ```csharp
   // Example: Using FileHelper
   FileHelper.WriteToFile("log.txt", "Hello World");
   string content = FileHelper.ReadFromFile("log.txt");
   ```

### Example: Repository Pattern

```csharp
using SMEAppHouse.Core.Patterns.Repo.V2.Base;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;

public class User : KeyedEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserRepository : RepositoryBaseSync<User, int, MyDbContext>
{
    public UserRepository(MyDbContext context) : base(context) { }
}

// Usage
var repository = new UserRepository(context);
var user = repository.Create(new User { Name = "John", Email = "john@example.com" }, autoSave: true);
var found = repository.Get(1);
```

### Example: Scheduled Job

```csharp
using SMEAppHouse.Core.QuartzExt;
using SMEAppHouse.Core.CodeKits;

public class DataSyncJob : JobServiceBase<DataSyncJob>
{
    public override void SubscriberInitialize()
    {
        // Initialize resources
    }

    public override void SubscriberExecute()
    {
        // Job logic here
    }
}

// Start job to run every 5 minutes
JobServiceStarter<DataSyncJob>.Start(
    recurrenceInterval: 5,
    intervalType: Rules.TimeIntervalTypesEnum.Minutes
);
```

---

## Solution Organization

### Solution Files
1. **SMEAppHouse.Patterns.sln**
   - Focused on core pattern libraries
   - Contains: CodeKits, Patterns.EF, Patterns.Repo, Patterns.Repo.V2

2. **SMEAppHouse.Core.AllLibs.sln**
   - Complete solution with all library projects
   - Organized into logical folders
   - Contains all 30+ projects

---

## Recommendations

### Immediate Actions
1. ✅ All projects migrated to .NET 8.0
2. ✅ Obsolete conditional references removed
3. ✅ Hardcoded assembly references cleaned up
4. ✅ Comprehensive documentation created
5. ✅ Legacy GPS.Frameworks projects merged/deleted (January 2025)
   - GPS.Frameworks.FreeProxyProvider → Merged into SMEAppHouse.Core.FreeProxyProvider
   - GPS.Frameworks.HtmlContentGenerator → Merged into SMEAppHouse.Core.ScraperBox
   - GPS.Frameworks.HtmlHelper → Already existed, deleted
   - GPS.Frameworks.SeleniumHelpers → Already existed, deleted

### Future Considerations
1. **Package Updates**: Consider updating packages to latest versions where appropriate
2. **Testing**: Verify all projects build successfully
3. **Dependency Review**: Audit package versions for security updates
4. **Documentation**: Keep documentation updated as APIs evolve

### Architecture Improvements
1. ✅ Consolidated legacy GPS.Frameworks projects into core libraries
2. Standardize naming conventions across projects
3. Review project organization in solution folders
4. Consider creating a unified NuGet package structure

---

## Contributing

When contributing to this solution:

1. **Follow .NET 8.0 Standards**: Use modern C# features and best practices
2. **Update Documentation**: Keep README files in `.documentations` up to date
3. **Use Common Assembly Info**: Import `.shared\common-assembly-info.proj` in all projects
4. **Test Thoroughly**: Ensure all projects build and tests pass
5. **Document Public APIs**: All public classes, interfaces, and methods should be documented

---

## License

Copyright © Nephiora IT Solutions 2025

All projects in this solution are licensed under the same terms. See individual project documentation for specific details.

---

## Support

For questions, issues, or contributions:
- Review project-specific documentation in `.documentations` folder
- Check individual README.md files for usage examples
- Refer to Solution_Analysis.md for architectural overview
- Refer to DOCUMENTATION_INDEX.md for documentation structure

---

**Last Updated**: January 2025  
**.NET Version**: 8.0  
**Total Projects**: 30+ library projects  
**Documentation**: Complete for all major projects  
**Recent Changes**: Merged legacy GPS.Frameworks projects into core libraries (January 2025)

