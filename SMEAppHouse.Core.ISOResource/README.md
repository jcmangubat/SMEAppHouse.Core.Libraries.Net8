# SMEAppHouse.Core.ISOResource

## Overview

`SMEAppHouse.Core.ISOResource` is a .NET 8.0 library that provides comprehensive ISO standard country codes and related data. It includes ISO 3166-1 country codes (Alpha-2, Alpha-3, numeric), ISO 4217 currency codes, country names in multiple languages, and regional classifications based on UN Statistics Division data.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.ISOResource.CountryCodes`

---

## Features

- **ISO 3166-1 Country Codes**: Alpha-2, Alpha-3, and numeric codes
- **ISO 4217 Currency Codes**: Currency alphabetic codes, numeric codes, and minor unit information
- **Multi-language Support**: Official country names in Arabic, Chinese, English, Spanish, French, and Russian
- **UN Terminology**: UNTERM formal and short names in multiple languages
- **Regional Classifications**: Continent, region, intermediate region, and development status
- **Singleton Pattern**: Thread-safe singleton instance for efficient data access
- **Embedded Resources**: Country data embedded as JSON resource

---

## Installation

Install via NuGet:
```bash
dotnet add package SMEAppHouse.Core.ISOResource
```

---

## Usage

### Basic Usage - Accessing Countries

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

// Get the singleton instance
var countries = Countries.Instance;

// Find a country by ISO 3166-1 Alpha-2 code
var usa = countries.FirstOrDefault(c => c.Iso31661Alpha2 == "US");

if (usa != null)
{
    Console.WriteLine($"Country: {usa.OfficialNameEn}");
    Console.WriteLine($"Alpha-2: {usa.Iso31661Alpha2}");
    Console.WriteLine($"Alpha-3: {usa.Iso31661Alpha3}");
    Console.WriteLine($"Numeric: {usa.Iso31661Numeric}");
}
```

### Finding Countries by Code

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

var countries = Countries.Instance;

// Find by Alpha-2 code
var country = countries.FirstOrDefault(c => c.Iso31661Alpha2 == "PH");

// Find by Alpha-3 code
var country2 = countries.FirstOrDefault(c => c.Iso31661Alpha3 == "PHL");

// Find by numeric code
var country3 = countries.FirstOrDefault(c => c.Iso31661Numeric.ToString() == "608");
```

### Accessing Multi-language Names

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

var countries = Countries.Instance;
var philippines = countries.FirstOrDefault(c => c.Iso31661Alpha2 == "PH");

if (philippines != null)
{
    Console.WriteLine($"English: {philippines.OfficialNameEn}");
    Console.WriteLine($"Arabic: {philippines.OfficialNameAr}");
    Console.WriteLine($"Chinese: {philippines.OfficialNameCn}");
    Console.WriteLine($"Spanish: {philippines.OfficialNameEs}");
    Console.WriteLine($"French: {philippines.OfficialNameFr}");
    Console.WriteLine($"Russian: {philippines.OfficialNameRu}");
}
```

### Accessing Currency Information

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

var countries = Countries.Instance;
var philippines = countries.FirstOrDefault(c => c.Iso31661Alpha2 == "PH");

if (philippines != null)
{
    Console.WriteLine($"Currency Code: {philippines.Iso4217CurrencyAlphabeticCode}");
    Console.WriteLine($"Currency Name: {philippines.Iso4217CurrencyName}");
    Console.WriteLine($"Currency Numeric: {philippines.Iso4217CurrencyNumericCode}");
    Console.WriteLine($"Minor Unit: {philippines.Iso4217CurrencyMinorUnit}");
}
```

### Filtering by Region or Continent

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

var countries = Countries.Instance;

// Get all countries in Asia
var asianCountries = countries.Where(c => c.RegionName == Rules.RegionName.Asia).ToList();

// Get all countries in Europe
var europeanCountries = countries.Where(c => c.Continent == Rules.Continent.Eu).ToList();

// Get developing countries
var developingCountries = countries
    .Where(c => c.DevelopedDevelopingCountries == Rules.DevelopedDevelopingCountries.Developing)
    .ToList();
```

### Accessing UN Terminology

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

var countries = Countries.Instance;
var country = countries.FirstOrDefault(c => c.Iso31661Alpha2 == "US");

if (country != null)
{
    Console.WriteLine($"UNTERM English Formal: {country.UntermEnglishFormal}");
    Console.WriteLine($"UNTERM English Short: {country.UntermEnglishShort}");
    Console.WriteLine($"UNTERM French Formal: {country.UntermFrenchFormal}");
    Console.WriteLine($"UNTERM Spanish Formal: {country.UntermSpanishFormal}");
}
```

---

## Public Classes

### Countries

Singleton class that provides access to all country data.

**Namespace**: `SMEAppHouse.Core.ISOResource.CountryCodes`

**Inherits**: `List<Country>`

#### Properties

```csharp
public static Countries Instance { get; }
```

Gets the singleton instance of the Countries collection. The data is loaded from an embedded JSON resource on first access.

**Thread Safety**: The singleton pattern is implemented with thread-safe lazy initialization.

**Example:**
```csharp
var countries = Countries.Instance;
var allCountries = countries.ToList(); // Returns all countries
```

---

### Country

Represents a country with all ISO standard information.

**Namespace**: `SMEAppHouse.Core.ISOResource.CountryCodes`

#### Properties

**ISO 3166-1 Codes:**
- `Iso31661Alpha2` (string): Two-letter country code (e.g., "US", "PH")
- `Iso31661Alpha3` (string): Three-letter country code (e.g., "USA", "PHL")
- `Iso31661Numeric` (Dial): Numeric country code

**Official Names (Multi-language):**
- `OfficialNameEn` (string): English official name
- `OfficialNameAr` (string): Arabic official name
- `OfficialNameCn` (string): Chinese official name
- `OfficialNameEs` (string): Spanish official name
- `OfficialNameFr` (string): French official name
- `OfficialNameRu` (string): Russian official name

**ISO 4217 Currency:**
- `Iso4217CurrencyAlphabeticCode` (string): Currency alphabetic code (e.g., "USD", "PHP")
- `Iso4217CurrencyName` (string): Currency name
- `Iso4217CurrencyNumericCode` (Dial): Currency numeric code
- `Iso4217CurrencyMinorUnit` (Iso4217CurrencyMinorUnitUnion): Currency minor unit
- `Iso4217CurrencyCountryName` (string): Country name for currency

**UN Terminology:**
- `UntermEnglishFormal` / `UntermEnglishShort`
- `UntermFrenchFormal` / `UntermFrenchShort`
- `UntermSpanishFormal` / `UntermSpanishShort`
- `UntermArabicFormal` / `UntermArabicShort`
- `UntermChineseFormal` / `UntermChineseShort`
- `UntermRussianFormal` / `UntermRussianShort`

**Regional Classifications:**
- `Continent` (Rules.Continent): Continent code
- `RegionName` (Rules.RegionName): Region name
- `IntermediateRegionName` (Rules.IntermediateRegionName): Intermediate region
- `DevelopedDevelopingCountries` (Rules.DevelopedDevelopingCountries): Development status
- `LandLockedDevelopingCountriesLldc` (Rules.LandLockedDevelopingCountriesLldc): LLDC status

**Other:**
- `M49` (Dial): UN M49 code
- `GlobalCode` (Rules.GlobalCode): Global code
- `GlobalName` (Rules.GlobalName): Global name

#### Methods

```csharp
public static Country[] FromJson(string json)
```

Deserializes country data from JSON string.

**Parameters:**
- `json` - JSON string containing country data

**Returns:** Array of Country objects

---

### Rules

Contains enumerations for various ISO classifications.

**Namespace**: `SMEAppHouse.Core.ISOResource.CountryCodes`

#### Enumerations

- `Continent`: Continent codes (Af, An, As, Eu, Na, Oc, Sa)
- `DevelopedDevelopingCountries`: Development status (Developed, Developing)
- `GlobalCode`: Global code values
- `GlobalName`: Global name values
- `IntermediateRegionName`: Intermediate region names
- `Iso4217CurrencyMinorUnitEnum`: Currency minor unit values
- `LandLockedDevelopingCountriesLldc`: LLDC status
- `RegionName`: Region names (Africa, Americas, Asia, Europe, Oceania)

---

## Complete Usage Examples

### Example 1: Country Lookup Service

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;
using System.Linq;

public class CountryService
{
    public Country GetCountryByCode(string code)
    {
        var countries = Countries.Instance;
        
        // Try Alpha-2 first
        var country = countries.FirstOrDefault(c => c.Iso31661Alpha2 == code);
        
        // Try Alpha-3 if not found
        if (country == null)
        {
            country = countries.FirstOrDefault(c => c.Iso31661Alpha3 == code);
        }
        
        return country;
    }
    
    public List<Country> GetCountriesByRegion(Rules.RegionName region)
    {
        return Countries.Instance
            .Where(c => c.RegionName == region)
            .ToList();
    }
}
```

### Example 2: Currency Information Lookup

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

public class CurrencyService
{
    public string GetCurrencyCode(string countryCode)
    {
        var country = Countries.Instance
            .FirstOrDefault(c => c.Iso31661Alpha2 == countryCode);
        
        return country?.Iso4217CurrencyAlphabeticCode ?? "N/A";
    }
    
    public Dictionary<string, string> GetAllCurrencies()
    {
        return Countries.Instance
            .Where(c => !string.IsNullOrEmpty(c.Iso4217CurrencyAlphabeticCode))
            .ToDictionary(
                c => c.Iso31661Alpha2,
                c => c.Iso4217CurrencyAlphabeticCode
            );
    }
}
```

### Example 3: Multi-language Country Names

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

public class LocalizationService
{
    public string GetCountryName(string countryCode, string language)
    {
        var country = Countries.Instance
            .FirstOrDefault(c => c.Iso31661Alpha2 == countryCode);
        
        if (country == null) return null;
        
        return language.ToUpper() switch
        {
            "EN" => country.OfficialNameEn,
            "AR" => country.OfficialNameAr,
            "CN" => country.OfficialNameCn,
            "ES" => country.OfficialNameEs,
            "FR" => country.OfficialNameFr,
            "RU" => country.OfficialNameRu,
            _ => country.OfficialNameEn
        };
    }
}
```

### Example 4: Regional Analysis

```csharp
using SMEAppHouse.Core.ISOResource.CountryCodes;

var countries = Countries.Instance;

// Count countries by region
var regionCounts = countries
    .GroupBy(c => c.RegionName)
    .Select(g => new { Region = g.Key, Count = g.Count() })
    .ToList();

// Count developed vs developing
var developed = countries.Count(c => 
    c.DevelopedDevelopingCountries == Rules.DevelopedDevelopingCountries.Developed);
    
var developing = countries.Count(c => 
    c.DevelopedDevelopingCountries == Rules.DevelopedDevelopingCountries.Developing);
```

---

## Data Source

The country data is based on:
- **ISO 3166-1**: Country codes standard
- **ISO 4217**: Currency codes standard
- **UN Statistics Division**: Official country names and classifications
- **DataHub**: https://datahub.io/core/country-codes

The data is embedded as a JSON resource (`countries.json`) in the assembly, ensuring it's always available without external dependencies.

---

## Dependencies

- Newtonsoft.Json (v13.0.4)

---

## Notes

- The `Countries` class uses a singleton pattern with thread-safe lazy initialization
- Country data is loaded from an embedded JSON resource on first access
- All country codes follow ISO 3166-1 standard
- Currency codes follow ISO 4217 standard
- Regional classifications are based on UN Statistics Division data
- The library supports filtering and querying using LINQ

---

## License

Copyright © Nephiora IT Solutions 2025

