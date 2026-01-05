# SMEAppHouse.Core.ScraperBox

## Overview

`SMEAppHouse.Core.ScraperBox` is a library for web scraping operations. It provides utilities for fetching web pages, parsing HTML, working with proxies, handling cookies, and various helper methods for web scraping tasks.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.ScraperBox`

---

## Public Classes and Utilities

### 1. Helper (Static Class)

Main utility class for web scraping operations.

**Namespace**: `SMEAppHouse.Core.ScraperBox`

#### URL and HTTP Operations

##### ResolveHttpUrl

```csharp
public static string ResolveHttpUrl(string url)
```

Resolves URLs that start with `//` to `http://`.

**Example:**
```csharp
var url = Helper.ResolveHttpUrl("//example.com/page"); // Returns "http://example.com/page"
```

##### ExtractDomainNameFromUrl

```csharp
public static string ExtractDomainNameFromUrl(string url, bool retainHttPrefix = false)
```

Extracts the domain name from a URL.

**Example:**
```csharp
var domain = Helper.ExtractDomainNameFromUrl("https://www.example.com/path/page");
// Returns "www.example.com" or "https://www.example.com" if retainHttPrefix is true
```

##### IsURLValid

```csharp
public static bool IsURLValid(string url, bool brute = false)
```

Validates if a URL is valid.

---

#### Page Document Retrieval

##### GetPageDocument (Multiple Overloads)

```csharp
public static string GetPageDocument(string site)
public static string GetPageDocument(string site, IWebProxy webProxy, ref string extraDataOnError)
public static string GetPageDocument(Uri site, ...)
public static string GetPageDocument(string sourceUrl, ref string extraDataOnError, IWebProxy webProxy = null, ...)
```

Retrieves HTML content from a web page with optional proxy support.

**Example:**
```csharp
// Simple fetch
var html = Helper.GetPageDocument("https://example.com");

// With proxy
var proxy = new WebProxy("127.0.0.1", 8080);
string errorData = null;
var html = Helper.GetPageDocument("https://example.com", proxy, ref errorData);
```

##### GetPageDocumentWithCookie

```csharp
public static string GetPageDocumentWithCookie(string url)
```

Retrieves page content with cookie support.

---

#### HTML Processing

##### Resolve

```csharp
public static string Resolve(string val, bool allTrim = false, params string[] otherElementsToClear)
```

Cleans and resolves HTML entities and encoded characters.

**Example:**
```csharp
var cleaned = Helper.Resolve("&amp;Hello%20World", allTrim: true);
// Returns "&Hello World"
```

##### CleanupHtmlStrains

```csharp
public static string CleanupHtmlStrains(string val, bool allTrim = false)
```

Removes HTML entities and unwanted characters.

##### RemoveHtmlComments

```csharp
public static string RemoveHtmlComments(string sourceHtml)
```

Removes HTML comments from source.

##### RemoveUnwantedTags

```csharp
public static string RemoveUnwantedTags(string data)
public static string RemoveUnwantedTags(string data, string[] acceptableTags)
```

Removes unwanted HTML tags, optionally keeping only specified tags.

---

#### HTML Node Operations

##### GetInnerText (Multiple Overloads)

```csharp
public static string GetInnerText(HtmlNode sourceNode, ...)
public static string GetInnerText(HtmlNode node, params string[] tagsToRemove)
public static string GetInnerText(HtmlNode node, bool removeCommentTags = true, params string[] tagsToRemove)
public static string GetInnerText(string sourceHtml, params string[] tagsToRemove)
public static string GetInnerText(string sourceHtml, bool removeCommentTags = true, params string[] tagsToRemove)
```

Extracts inner text from HTML nodes or strings.

**Example:**
```csharp
var htmlDoc = new HtmlDocument();
htmlDoc.LoadHtml("<div><p>Hello <b>World</b></p></div>");
var node = htmlDoc.DocumentNode.SelectSingleNode("//div");

var text = Helper.GetInnerText(node, "b"); // Returns "Hello World" (removes <b> tags)
```

##### GetNode

```csharp
public static HtmlNode GetNode(HtmlNode node, ...)
```

Gets a specific HTML node using XPath or other selectors.

##### GetNodeByInnerHtml

```csharp
public static HtmlNode GetNodeByInnerHtml(HtmlNode node, ...)
```

Finds a node by its inner HTML content.

##### GetNodeByAttribute

```csharp
public static HtmlNode GetNodeByAttribute(HtmlNode node, ...)
```

Finds a node by attribute value.

##### GetNodeCollection

```csharp
public static IEnumerable<HtmlNode> GetNodeCollection(HtmlNode node, ...)
public static IEnumerable<HtmlNode> GetNodeCollection(HtmlNode node, params string[] element)
```

Gets a collection of HTML nodes.

---

#### Query String Operations

##### EncodeQueryStringSegment

```csharp
public static string EncodeQueryStringSegment(string query)
```

Encodes query string segments.

**Example:**
```csharp
var encoded = Helper.EncodeQueryStringSegment("hello world & test");
// Returns "hello%20world%20%26%20test"
```

---

#### Proxy Operations

##### FindProxyCountryFromPartial

```csharp
public static Rules.WorldCountriesEnum FindProxyCountryFromPartial(string countryNamePartial)
```

Finds a country enum value from a partial country name.

**Example:**
```csharp
var country = Helper.FindProxyCountryFromPartial("united"); // Returns WorldCountriesEnum.UNITED_STATES
```

---

### 2. Models

#### IPProxy

Represents an IP proxy server.

**Namespace**: `SMEAppHouse.Core.ScraperBox.Models`

**Properties:**
- `Guid Id` - Unique identifier
- `string ProviderId` - Proxy provider ID
- `string IPAddress` - Proxy IP address
- `int PortNo` - Proxy port number
- `Rules.WorldCountriesEnum Country` - Country of proxy
- `IPProxyRules.ProxyAnonymityLevelsEnum AnonymityLevel` - Anonymity level
- `IPProxyRules.ProxyProtocolsEnum Protocol` - Protocol (HTTP, HTTPS, SOCKS)
- `DateTime LastChecked` - Last validation time
- `int ResponseRate` - Response rate percentage
- `int SpeedRate` - Speed in milliseconds
- `TimeSpan SpeedTimeSpan` - Speed as TimeSpan
- `string ISP` - Internet Service Provider
- `string City` - City location
- `IPProxyRules.ProxySpeedsEnum Speed` - Speed category
- `IPProxyRules.ProxyConnectionSpeedsEnum ConnectionTime` - Connection speed
- `Guid CheckerTokenId` - Checker token identifier
- `CheckStatusEnum CheckStatus` - Current check status
- `Tuple<string, string> Credential` - Username/password credentials

**Methods:**

##### ToWebProxy

```csharp
public IWebProxy ToWebProxy()
```

Converts to `IWebProxy` for use with HTTP clients.

##### ToNetworkCredential

```csharp
public NetworkCredential ToNetworkCredential()
```

Converts credentials to `NetworkCredential`.

##### AsTuple

```csharp
public Tuple<string, string> AsTuple()
```

Returns IP and port as a tuple.

##### GetLastValidationElapsedTime

```csharp
public TimeSpan GetLastValidationElapsedTime()
```

Gets time elapsed since last validation.

**CheckStatusEnum:**
- `NotChecked`
- `Checking`
- `Checked`
- `CheckedInvalid`

**Example:**
```csharp
var proxy = new IPProxy
{
    IPAddress = "192.168.1.1",
    PortNo = 8080,
    Country = Rules.WorldCountriesEnum.UNITED_STATES,
    AnonymityLevel = IPProxyRules.ProxyAnonymityLevelsEnum.Elite,
    Protocol = IPProxyRules.ProxyProtocolsEnum.HTTP,
    Credential = new Tuple<string, string>("username", "password")
};

// Use with HTTP client
var webProxy = proxy.ToWebProxy();
var credential = proxy.ToNetworkCredential();
```

---

#### PageInstruction

Represents pagination instructions for URL construction.

**Namespace**: `SMEAppHouse.Core.ScraperBox.Models`

**Properties:**
- `char PadCharacter` - Character used for padding
- `int PadLength` - Length of padding
- `PaddingDirectionsEnum PaddingDirection` - Direction of padding (Left or Right)

**PaddingDirectionsEnum:**
- `ToLeft` - Pad to the left
- `ToRight` - Pad to the right

**Extension Method:**

##### PageNo

```csharp
public static string PageNo(this PageInstruction pgInstruction, int pageNo)
```

Formats a page number according to the instruction.

**Example:**
```csharp
var instruction = new PageInstruction
{
    PadCharacter = '0',
    PadLength = 3,
    PaddingDirection = PageInstruction.PaddingDirectionsEnum.ToLeft
};

var pageNumber = instruction.PageNo(5); // Returns "005"
```

---

#### UserAgents

Type-safe enum pattern for user agent strings.

**Namespace**: `SMEAppHouse.Core.ScraperBox.Models`

**Static Properties:**
- `UserAgents Mozilla22`
- `UserAgents FireFox36`
- `UserAgents FireFox33`
- `UserAgents Chrome41022280`
- `UserAgents InternetExplorer8`

**Methods:**

##### GetFakeUserAgent

```csharp
public static FakeUserAgent GetFakeUserAgent(UserAgents userAgent)
```

Gets the fake user agent string.

**Example:**
```csharp
var userAgent = UserAgents.GetFakeUserAgent(UserAgents.Chrome41022280);
// Returns "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36..."
```

---

#### AuthenticationMethod

Type-safe enum for authentication methods.

**Namespace**: `SMEAppHouse.Core.ScraperBox.Models`

**Static Properties:**
- `AuthenticationMethod FORMS`
- `AuthenticationMethod WINDOWSAUTHENTICATION`
- `AuthenticationMethod SINGLESIGNON`

---

### 3. Rules and Enums

### 2. ContentGenerator (Class)

Queue-based HTML content generator that processes URLs from a queue using ProcessAgentViaTask.

**Namespace**: `SMEAppHouse.Core.ScraperBox`

**Implements**: `IContentGenerator`, `ProcessAgentViaTask`

#### Properties

```csharp
public List<HtmlTarget> Sources { get; set; }
public bool UseProxyWhenAvailable { get; set; }
public bool IsBusy { get; set; }
```

#### Events

```csharp
public event EventHandlers.StartingEventHandler OnStarting;
public event EventHandlers.DoneEventHandler OnDone;
public event EventHandlers.OperationExceptionEventHandler OnOperationException;
```

#### Methods

##### FeedSource

```csharp
public void FeedSource(HtmlTarget target)
```

Adds a target URL to the processing queue.

**Example:**
```csharp
var generator = new ContentGenerator();
generator.FeedSource(new HtmlTarget 
{ 
    Url = "https://example.com/page1",
    PageNo = 1 
});
```

##### GetContent

```csharp
public string GetContent(string pgUrl)
```

Gets content from a URL with optional proxy support.

**Example:**
```csharp
var generator = new ContentGenerator(() => myProxyProvider.GetProxy());
var content = generator.GetContent("https://example.com");
```

#### Usage Example

```csharp
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;

// Create generator with proxy provider
var generator = new ContentGenerator(() => proxyManager.GetFreeProxy());
generator.UseProxyWhenAvailable = true;

// Subscribe to events
generator.OnStarting += (sender, e) => 
{
    Console.WriteLine($"Starting: {e.Target.Url}");
};

generator.OnDone += (sender, e) => 
{
    Console.WriteLine($"Done: {e.Target.Url} in {e.ElapsedTime.TotalSeconds}s");
    // Process e.Target.Content
};

generator.OnOperationException += (sender, e) => 
{
    Console.WriteLine($"Error: {e.Exception.Message}");
};

// Feed URLs to process
generator.FeedSource(new HtmlTarget { Url = "https://example.com/page1", PageNo = 1 });
generator.FeedSource(new HtmlTarget { Url = "https://example.com/page2", PageNo = 2 });

// Activate and start processing
await generator.Activate();
generator.Resume();
```

### 3. Models

#### HtmlSource

Source configuration for content generation with URL patterns and page ranges.

**Namespace**: `SMEAppHouse.Core.ScraperBox.Models`

**Properties:**
- `string Id` - Source identifier
- `string Name` - Source name
- `string UrlPattern` - URL pattern with placeholders
- `int PageNoMin` - Minimum page number
- `int PageNoMax` - Maximum page number
- `bool Ignore` - Whether to ignore this source
- `int ContentSize` - Expected content size
- `string UrlPatternDecoded` - Decoded URL pattern

**Example:**
```csharp
var source = new HtmlSource
{
    Id = "example",
    Name = "Example Site",
    UrlPattern = "https://example.com/page?p={0}",
    PageNoMin = 1,
    PageNoMax = 100
};
```

#### HtmlTarget

Target URL to be processed by ContentGenerator.

**Namespace**: `SMEAppHouse.Core.ScraperBox.Models`

**Properties:**
- `int PageNo` - Page number
- `string Url` - Target URL
- `string Content` - Retrieved content (set after processing)
- `bool PageIsInvalid` - Whether the page is invalid
- `HtmlSource Source` - Associated source configuration

**Example:**
```csharp
var target = new HtmlTarget
{
    PageNo = 1,
    Url = "https://example.com/page1",
    Source = source
};
```

### 4. Interfaces

#### IContentGenerator

Interface for content generation with queue-based processing.

**Namespace**: `SMEAppHouse.Core.ScraperBox.Interfaces`

**Properties:**
- `bool IsBusy`
- `bool UseProxyWhenAvailable`
- `List<HtmlTarget> Sources`

**Methods:**
- `void FeedSource(HtmlTarget target)`

**Events:**
- `OnStarting`, `OnDone`, `OnOperationException`

---

#### HttpOpsRules

HTTP operation rules and constants.

**Namespace**: `SMEAppHouse.Core.ScraperBox`

**HttpMethodConsts Enum:**
- `GET`, `POST`, `PUT`, `HEAD`, `TRACE`, `DELETE`, `SEARCH`, `CONNECT`, `PROPFIND`, `PROPPATCH`, `PATCH`, `MKCOL`, `COPY`, `MOVE`, `LOCK`, `UNLOCK`, `OPTIONS`

**ContentTypeConsts Enum:**
- `Xml`
- `Json`

---

#### IPProxyRules

IP proxy rules and enums.

**Namespace**: `SMEAppHouse.Core.ScraperBox`

**ProxyAnonymityLevelsEnum:**
- `Elite` - Highly anonymous (Level 1)
- `Anonymous` - Anonymous (Level 2)
- `Transparent` - Transparent (Level 3)

**ProxySpeedsEnum:**
- `Slow`
- `Medium`
- `Fast`

**ProxyConnectionSpeedsEnum:**
- `Slow`
- `Medium`
- `Fast`

**ProxyProtocolsEnum:**
- `HTTP`
- `HTTPS`
- `SOCKS4_5`

---

## Complete Usage Examples

### Example 1: Basic Web Scraping

```csharp
using SMEAppHouse.Core.ScraperBox;
using HtmlAgilityPack;

// Fetch a web page
var html = Helper.GetPageDocument("https://example.com");

// Parse HTML
var doc = new HtmlDocument();
doc.LoadHtml(html);

// Extract data
var title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText;
var links = doc.DocumentNode.SelectNodes("//a[@href]")
    .Select(a => a.GetAttributeValue("href", ""))
    .ToList();
```

---

### Example 2: Scraping with Proxy

```csharp
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;

// Create proxy
var proxy = new IPProxy
{
    IPAddress = "192.168.1.1",
    PortNo = 8080,
    Protocol = IPProxyRules.ProxyProtocolsEnum.HTTP,
    AnonymityLevel = IPProxyRules.ProxyAnonymityLevelsEnum.Elite,
    Credential = new Tuple<string, string>("user", "pass")
};

// Fetch with proxy
var webProxy = proxy.ToWebProxy();
string errorData = null;
var html = Helper.GetPageDocument("https://example.com", webProxy, ref errorData);

if (!string.IsNullOrEmpty(errorData))
{
    Console.WriteLine($"Error: {errorData}");
}
```

---

### Example 3: HTML Processing

```csharp
using SMEAppHouse.Core.ScraperBox;
using HtmlAgilityPack;

var html = "<div><p>Hello <b>World</b> &amp; Friends</p><!-- comment --></div>";

// Clean HTML
var cleaned = Helper.CleanupHtmlStrains(html, allTrim: true);

// Remove comments
var noComments = Helper.RemoveHtmlComments(html);

// Remove unwanted tags
var textOnly = Helper.RemoveUnwantedTags(html, new[] { "p" }); // Keep only <p> tags

// Get inner text
var doc = new HtmlDocument();
doc.LoadHtml(html);
var node = doc.DocumentNode.SelectSingleNode("//div");
var innerText = Helper.GetInnerText(node, "b"); // Removes <b> tags
```

---

### Example 4: Pagination

```csharp
using SMEAppHouse.Core.ScraperBox.Models;

var instruction = new PageInstruction
{
    PadCharacter = '0',
    PadLength = 3,
    PaddingDirection = PageInstruction.PaddingDirectionsEnum.ToLeft
};

// Generate paginated URLs
for (int i = 1; i <= 10; i++)
{
    var pageNumber = instruction.PageNo(i); // "001", "002", ..., "010"
    var url = $"https://example.com/page/{pageNumber}";
    Console.WriteLine(url);
}
```

---

### Example 5: Node Collection Extraction

```csharp
using SMEAppHouse.Core.ScraperBox;
using HtmlAgilityPack;

var html = Helper.GetPageDocument("https://example.com/products");
var doc = new HtmlDocument();
doc.LoadHtml(html);

// Get all product nodes
var products = Helper.GetNodeCollection(doc.DocumentNode, "div", "class", "product");

foreach (var product in products)
{
    var name = Helper.GetInnerText(product, "h2");
    var price = Helper.GetInnerText(product, "span", "class", "price");
    Console.WriteLine($"{name}: {price}");
}
```

---

### Example 6: URL Processing

```csharp
using SMEAppHouse.Core.ScraperBox;

// Resolve relative URLs
var url1 = Helper.ResolveHttpUrl("//example.com/page"); // "http://example.com/page"
var url2 = Helper.ResolveHttpUrl("https://example.com/page"); // "https://example.com/page"

// Extract domain
var domain = Helper.ExtractDomainNameFromUrl("https://www.example.com/path/page");
// Returns "www.example.com"

// Validate URL
bool isValid = Helper.IsURLValid("https://example.com");

// Encode query string
var encoded = Helper.EncodeQueryStringSegment("search query & filter");
// Returns "search%20query%20%26%20filter"
```

---

### Example 7: User Agent Usage

```csharp
using SMEAppHouse.Core.ScraperBox.Models;
using System.Net.Http;

var userAgent = UserAgents.GetFakeUserAgent(UserAgents.Chrome41022280);

var client = new HttpClient();
client.DefaultRequestHeaders.Add("User-Agent", userAgent.UserAgent);

var response = await client.GetAsync("https://example.com");
```

---

## Key Features

1. **Web Page Fetching**: Multiple methods for retrieving web page content
2. **Proxy Support**: Full support for HTTP proxies with authentication
3. **HTML Parsing**: Integration with HtmlAgilityPack for DOM manipulation
4. **HTML Cleaning**: Utilities for cleaning and processing HTML
5. **Node Operations**: Methods for finding and extracting HTML nodes
6. **Pagination Support**: Helper for constructing paginated URLs
7. **User Agent Management**: Predefined user agent strings
8. **URL Processing**: Utilities for URL manipulation and validation

---

## Dependencies

- HtmlAgilityPack (v1.12.3)
- ScrapySharp (v3.0.0)
- SMEAppHouse.Core.CodeKits

---

## Notes

- Uses HtmlAgilityPack for HTML parsing
- Proxy support includes authentication via credentials
- All HTML operations are case-sensitive for tag names
- XPath expressions are supported for node selection
- User agent strings are predefined for common browsers
- PageInstruction format: `'{padChar}-{padLength}-{direction}'` where direction is 0 (left) or 1 (right)
- Proxy protocols support HTTP, HTTPS, and SOCKS4/5
- Anonymity levels indicate how well the proxy hides your IP

---

## License

Copyright © Nephiora IT Solutions 2025
