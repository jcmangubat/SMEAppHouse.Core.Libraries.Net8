# SMEAppHouse.Core.SeleniumExt

## Overview

`SMEAppHouse.Core.SeleniumExt` is a library for handling Selenium functionalities. It provides extension methods for Selenium WebDriver to simplify common operations like waiting for elements, taking full-page screenshots, and working with element attributes.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.SeleniumExt`

---

## Public Classes and Utilities

### Extensions (Static Class)

Extension methods for `IWebDriver` and `Screenshot` classes.

**Namespace**: `SMEAppHouse.Core.SeleniumExt`

---

#### Wait Methods

##### WaitUntilElementIsPresent

```csharp
public static bool WaitUntilElementIsPresent(this IWebDriver driver, By by, int timeoutInSeconds = 10)
```

Waits until an element is present on the page.

**Example:**
```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SMEAppHouse.Core.SeleniumExt;

var driver = new ChromeDriver();
driver.Navigate().GoToUrl("https://example.com");

// Wait for element to appear (max 10 seconds)
bool found = driver.WaitUntilElementIsPresent(By.Id("myElement"));

if (found)
{
    var element = driver.FindElement(By.Id("myElement"));
    element.Click();
}
```

##### WaitUntilElementIsNotPresent

```csharp
public static bool WaitUntilElementIsNotPresent(this IWebDriver driver, By by, int timeoutInSeconds = 10)
```

Waits until an element is no longer present on the page.

**Example:**
```csharp
// Wait for loading spinner to disappear
bool disappeared = driver.WaitUntilElementIsNotPresent(By.ClassName("spinner"), timeoutInSeconds: 5);
```

##### WaitUntilElementInnerTextContains

```csharp
public static bool WaitUntilElementInnerTextContains(this IWebDriver driver, By by, string valuePartial, int timeoutInSeconds = 10)
```

Waits until an element's inner text contains the specified value.

**Example:**
```csharp
// Wait for status message to appear
bool contains = driver.WaitUntilElementInnerTextContains(
    By.Id("status"), 
    "Success", 
    timeoutInSeconds: 15
);
```

##### WaitUntilElementWithInnerValueExist

```csharp
public static bool WaitUntilElementWithInnerValueExist(this IWebDriver driver, By by, string valuePartial, int timeoutInSeconds = 10)
```

Waits until an element with the specified inner HTML value exists.

**Example:**
```csharp
bool exists = driver.WaitUntilElementWithInnerValueExist(
    By.TagName("div"), 
    "Loading complete"
);
```

##### WaitUntilElementWithAttributeValueExist

```csharp
public static bool WaitUntilElementWithAttributeValueExist(this IWebDriver driver, By by, string attribute, string valuePartial, int timeoutInSeconds = 10)
```

Waits until an element with the specified attribute value exists.

**Example:**
```csharp
// Wait for element with data-status="ready"
bool exists = driver.WaitUntilElementWithAttributeValueExist(
    By.TagName("div"), 
    "data-status", 
    "ready"
);
```

---

#### Element Attribute Methods

##### GetElementAttributeValue

```csharp
public static string GetElementAttributeValue(this IWebDriver driver, IWebElement element, string attribute)
```

Gets an element's attribute value using JavaScript.

**Example:**
```csharp
var element = driver.FindElement(By.Id("myButton"));
string dataId = driver.GetElementAttributeValue(element, "data-id");
string className = driver.GetElementAttributeValue(element, "class");
```

---

#### Screenshot Methods

##### GetEntireScreenshot

```csharp
public static Image GetEntireScreenshot(this IWebDriver driver)
```

Takes a full-page screenshot by stitching together multiple viewport screenshots.

**Example:**
```csharp
using System.Drawing;
using System.Drawing.Imaging;

var driver = new ChromeDriver();
driver.Navigate().GoToUrl("https://example.com");

// Take full-page screenshot
Image fullPageScreenshot = driver.GetEntireScreenshot();

// Save to file
fullPageScreenshot.Save("fullpage.png", ImageFormat.Png);
```

##### ToImage

```csharp
public static Image ToImage(this Screenshot screenshot)
```

Converts a Selenium `Screenshot` to a .NET `Image`.

**Example:**
```csharp
var screenshot = driver.TakeScreenshot();
Image image = screenshot.ToImage();
image.Save("screenshot.png", ImageFormat.Png);
```

---

## Complete Usage Examples

### Example 1: Basic Element Waiting

```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SMEAppHouse.Core.SeleniumExt;

var driver = new ChromeDriver();

try
{
    driver.Navigate().GoToUrl("https://example.com");
    
    // Wait for page to load
    if (driver.WaitUntilElementIsPresent(By.TagName("body")))
    {
        // Wait for specific element
        if (driver.WaitUntilElementIsPresent(By.Id("submitButton"), timeoutInSeconds: 15))
        {
            var button = driver.FindElement(By.Id("submitButton"));
            button.Click();
            
            // Wait for element to disappear after click
            driver.WaitUntilElementIsNotPresent(By.Id("submitButton"));
        }
    }
}
finally
{
    driver.Quit();
}
```

---

### Example 2: Dynamic Content Waiting

```csharp
public class DynamicPageTest
{
    public void TestDynamicContent(IWebDriver driver)
    {
        driver.Navigate().GoToUrl("https://example.com/dynamic");
        
        // Wait for loading indicator to disappear
        driver.WaitUntilElementIsNotPresent(By.ClassName("loading"), timeoutInSeconds: 30);
        
        // Wait for content to appear with specific text
        bool contentLoaded = driver.WaitUntilElementInnerTextContains(
            By.Id("content"), 
            "Data loaded successfully"
        );
        
        if (contentLoaded)
        {
            var content = driver.FindElement(By.Id("content"));
            Console.WriteLine($"Content: {content.Text}");
        }
    }
}
```

---

### Example 3: Full-Page Screenshot

```csharp
using System.Drawing;
using System.Drawing.Imaging;
using OpenQA.Selenium.Chrome;
using SMEAppHouse.Core.SeleniumExt;

public class ScreenshotService
{
    public void CaptureFullPage(string url, string outputPath)
    {
        var driver = new ChromeDriver();
        
        try
        {
            driver.Navigate().GoToUrl(url);
            
            // Wait for page to fully load
            driver.WaitUntilElementIsPresent(By.TagName("body"));
            Thread.Sleep(2000); // Additional wait for dynamic content
            
            // Capture full page
            Image fullPage = driver.GetEntireScreenshot();
            fullPage.Save(outputPath, ImageFormat.Png);
            
            Console.WriteLine($"Screenshot saved to {outputPath}");
        }
        finally
        {
            driver.Quit();
        }
    }
}

// Usage
var service = new ScreenshotService();
service.CaptureFullPage("https://example.com", "fullpage.png");
```

---

### Example 4: Attribute-Based Element Finding

```csharp
public class AttributeBasedTest
{
    public void TestWithAttributes(IWebDriver driver)
    {
        driver.Navigate().GoToUrl("https://example.com");
        
        // Wait for element with specific data attribute
        bool ready = driver.WaitUntilElementWithAttributeValueExist(
            By.ClassName("widget"), 
            "data-ready", 
            "true"
        );
        
        if (ready)
        {
            var widgets = driver.FindElements(By.ClassName("widget"));
            foreach (var widget in widgets)
            {
                string dataId = driver.GetElementAttributeValue(widget, "data-id");
                string status = driver.GetElementAttributeValue(widget, "data-status");
                
                Console.WriteLine($"Widget {dataId}: {status}");
            }
        }
    }
}
```

---

### Example 5: Form Submission with Waiting

```csharp
public class FormTest
{
    public void SubmitForm(IWebDriver driver)
    {
        driver.Navigate().GoToUrl("https://example.com/form");
        
        // Wait for form to load
        driver.WaitUntilElementIsPresent(By.Id("form"));
        
        // Fill form
        driver.FindElement(By.Id("name")).SendKeys("John Doe");
        driver.FindElement(By.Id("email")).SendKeys("john@example.com");
        
        // Submit
        driver.FindElement(By.Id("submit")).Click();
        
        // Wait for success message
        bool success = driver.WaitUntilElementInnerTextContains(
            By.ClassName("message"), 
            "Thank you"
        );
        
        if (success)
        {
            var message = driver.FindElement(By.ClassName("message"));
            Console.WriteLine($"Success: {message.Text}");
        }
    }
}
```

---

### Example 6: Screenshot Comparison

```csharp
using System.Drawing;
using OpenQA.Selenium.Chrome;
using SMEAppHouse.Core.SeleniumExt;

public class VisualTest
{
    public void CompareScreenshots(string url1, string url2)
    {
        var driver = new ChromeDriver();
        
        try
        {
            // Capture first page
            driver.Navigate().GoToUrl(url1);
            Image screenshot1 = driver.GetEntireScreenshot();
            screenshot1.Save("page1.png", ImageFormat.Png);
            
            // Capture second page
            driver.Navigate().GoToUrl(url2);
            Image screenshot2 = driver.GetEntireScreenshot();
            screenshot2.Save("page2.png", ImageFormat.Png);
            
            // Compare (simplified - you'd use image comparison library)
            Console.WriteLine("Screenshots captured for comparison");
        }
        finally
        {
            driver.Quit();
        }
    }
}
```

---

## Key Features

1. **Element Waiting**: Multiple wait methods for different scenarios
2. **Full-Page Screenshots**: Automatic stitching of viewport screenshots
3. **Attribute Access**: JavaScript-based attribute retrieval
4. **Text Matching**: Wait for elements with specific text content
5. **Timeout Control**: Configurable timeout for all wait operations
6. **Image Conversion**: Convert Selenium screenshots to .NET Image objects

---

## Dependencies

- Selenium.WebDriver (v4.35.0)
- Selenium.Support (v4.35.0)
- System.Drawing.Common (v9.0.9)

---

## Notes

- All wait methods use `WebDriverWait` internally
- Default timeout is 10 seconds for all wait operations
- `GetEntireScreenshot` automatically handles scrolling and stitching
- Attribute methods use JavaScript execution for reliable access
- Text matching is case-insensitive
- Full-page screenshots work best with headless browsers
- Remember to call `driver.Quit()` to clean up resources

---

## License

Copyright © Nephiora IT Solutions 2025
