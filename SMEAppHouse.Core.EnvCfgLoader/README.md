# SMEAppHouse.Core.EnvCfgLoader

## Overview

`SMEAppHouse.Core.EnvCfgLoader` is a lightweight .NET 8.0 library for loading environment variables from `.env` files into the .NET configuration system. It provides seamless integration with `Microsoft.Extensions.Configuration` to load environment variables from `.env` files, making it easy to manage application settings across different environments.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.EnvCfgLoader`

---

## Features

- Load environment variables from `.env` files
- Seamless integration with `IConfigurationBuilder`
- Support for comments (lines starting with `#`)
- Support for empty values
- Automatic section separator conversion (`__` to `:`)
- Optional file support (doesn't throw if file doesn't exist)

---

## Installation

Install via NuGet:
```bash
dotnet add package SMEAppHouse.Core.EnvCfgLoader
```

---

## Usage

### Basic Usage

```csharp
using Microsoft.Extensions.Configuration;
using SMEAppHouse.Core.EnvCfgLoader;

var builder = new ConfigurationBuilder();
builder.AddEnvFile(); // Loads from .env file in current directory

var configuration = builder.Build();
```

### Custom File Path

```csharp
var builder = new ConfigurationBuilder();
builder.AddEnvFile("path/to/your/.env"); // Specify custom path

var configuration = builder.Build();
```

### With ASP.NET Core

```csharp
using SMEAppHouse.Core.EnvCfgLoader;

var builder = WebApplication.CreateBuilder(args);

// Add .env file support
builder.Configuration.AddEnvFile();

var app = builder.Build();
```

### Reading Configuration Values

```csharp
// .env file content:
// DATABASE_CONNECTION_STRING=Server=localhost;Database=MyDb
// API_KEY=your-api-key-here
// LOG_LEVEL=Information

var connectionString = configuration["DATABASE_CONNECTION_STRING"];
var apiKey = configuration["API_KEY"];
var logLevel = configuration["LOG_LEVEL"];

// Using nested configuration (double underscore becomes colon)
// APP__DATABASE__CONNECTION_STRING=Server=localhost;Database=MyDb
var nestedValue = configuration["APP:DATABASE:CONNECTION_STRING"];
```

---

## .env File Format

The library supports standard `.env` file format:

```
# This is a comment
DATABASE_CONNECTION_STRING=Server=localhost;Database=MyDb
API_KEY=your-api-key-here
LOG_LEVEL=Information

# Empty values are allowed
OPTIONAL_VALUE=

# Nested configuration (double underscore becomes colon)
APP__DATABASE__HOST=localhost
APP__DATABASE__PORT=5432
```

**Rules:**
- Lines starting with `#` are treated as comments and ignored
- Empty lines are ignored
- Key-value pairs are separated by `=`
- The first `=` is used as the separator
- Keys with double underscores (`__`) are converted to colons (`:`) for nested configuration
- Empty values are allowed

---

## Public Classes

### ConfigurationBuilderExtensions

Extension methods for `IConfigurationBuilder` to add `.env` file support.

**Namespace**: `SMEAppHouse.Core.EnvCfgLoader`

#### Methods

```csharp
public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, string filePath = ".env")
```

Adds environment variables from a `.env` file to the configuration builder.

**Parameters:**
- `builder` - The configuration builder instance
- `filePath` - Path to the `.env` file (default: `.env`)

**Returns:** The same `IConfigurationBuilder` instance for method chaining

**Example:**
```csharp
var builder = new ConfigurationBuilder();
builder
    .AddEnvFile()
    .AddJsonFile("appsettings.json");
```

---

### EnvFileConfigurationSource

Configuration source for `.env` files.

**Namespace**: `SMEAppHouse.Core.EnvCfgLoader`

**Implements**: `IConfigurationSource`

#### Constructor

```csharp
public EnvFileConfigurationSource(string filePath)
```

Creates a new configuration source for the specified `.env` file.

**Parameters:**
- `filePath` - Path to the `.env` file

---

### EnvFileConfigurationProvider

Configuration provider that loads values from `.env` files.

**Namespace**: `SMEAppHouse.Core.EnvCfgLoader`

**Inherits**: `ConfigurationProvider`

#### Constructor

```csharp
public EnvFileConfigurationProvider(string filePath, bool optional = false)
```

Creates a new configuration provider for the specified `.env` file.

**Parameters:**
- `filePath` - Path to the `.env` file
- `optional` - If `true`, the file is optional and won't throw if missing (default: `false`)

#### Methods

```csharp
public override void Load()
```

Loads the configuration values from the `.env` file.

---

## Complete Usage Examples

### Example 1: Console Application

```csharp
using Microsoft.Extensions.Configuration;
using SMEAppHouse.Core.EnvCfgLoader;

var builder = new ConfigurationBuilder();
builder.AddEnvFile("app.env");

var config = builder.Build();

var dbConnection = config["DATABASE_CONNECTION_STRING"];
var apiKey = config["API_KEY"];

Console.WriteLine($"Database: {dbConnection}");
Console.WriteLine($"API Key: {apiKey}");
```

### Example 2: ASP.NET Core Web Application

```csharp
using SMEAppHouse.Core.EnvCfgLoader;

var builder = WebApplication.CreateBuilder(args);

// Load .env file before other configuration sources
builder.Configuration.AddEnvFile();

// Add other configuration sources
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var app = builder.Build();

// Use configuration
var connectionString = app.Configuration.GetConnectionString("DefaultConnection");
var apiSettings = app.Configuration.GetSection("API");
```

### Example 3: Worker Service

```csharp
using Microsoft.Extensions.Configuration;
using SMEAppHouse.Core.EnvCfgLoader;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // Load .env file
        config.AddEnvFile();
        
        // Add other sources
        config.AddJsonFile("appsettings.json", optional: true);
    })
    .Build();

await host.RunAsync();
```

### Example 4: Nested Configuration

```csharp
// .env file:
// APP__DATABASE__HOST=localhost
// APP__DATABASE__PORT=5432
// APP__API__BASE_URL=https://api.example.com

var builder = new ConfigurationBuilder();
builder.AddEnvFile();

var config = builder.Build();

// Access nested values using colon separator
var dbHost = config["APP:DATABASE:HOST"]; // "localhost"
var dbPort = config["APP:DATABASE:PORT"]; // "5432"
var apiUrl = config["APP:API:BASE_URL"]; // "https://api.example.com"
```

---

## Dependencies

- Microsoft.Extensions.Configuration (v8.0.0)

---

## Notes

- The library automatically converts double underscores (`__`) to colons (`:`) to support nested configuration sections
- Comments in `.env` files (lines starting with `#`) are ignored
- Empty lines are ignored
- If the `.env` file doesn't exist and `optional` is `false`, a `FileNotFoundException` will be thrown
- The first `=` character in a line is used as the key-value separator
- Empty values are allowed (e.g., `KEY=`)

---

## License

Copyright © Nephiora IT Solutions 2025

