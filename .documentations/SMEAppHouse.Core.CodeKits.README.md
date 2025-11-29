# SMEAppHouse.Core.CodeKits

## Overview

`SMEAppHouse.Core.CodeKits` is a comprehensive utility library providing reusable functions and procedures for .NET 8.0 applications. It includes file operations, cryptographic utilities, data manipulation helpers, extension methods, expression builders, and various helper classes.

**Target Framework**: .NET 8.0  
**Namespace**: `SMEAppHouse.Core.CodeKits`

---

## Public Classes and Utilities

### 1. CodeKit (Static Class)

Main utility class providing various helper methods for common programming tasks.

**Namespace**: `SMEAppHouse.Core.CodeKits`

#### Type Checking Methods

```csharp
public static bool IsNumericType(Type type)
```
Checks if a type is numeric (excluding strings, bools, enums, collections).

**Example:**
```csharp
bool isNumeric = CodeKit.IsNumericType(typeof(int)); // true
bool isString = CodeKit.IsNumericType(typeof(string)); // false
bool isDecimal = CodeKit.IsNumericType(typeof(decimal)); // true
```

#### Password Hashing

```csharp
public static string HashPassword(string password)
```
Hashes a password using SHA256 algorithm.

**Example:**
```csharp
string password = "MyPassword123";
string hashed = CodeKit.HashPassword(password);
// Returns SHA256 hash as hexadecimal string
```

#### Object Operations

```csharp
public static void CopyObjectProperties<TSource, TTarget>(TSource source, TTarget target)
    where TSource : class
    where TTarget : class
```
Copies all public, readable properties from source to target object.

**Example:**
```csharp
var source = new { Name = "John", Age = 30 };
var target = new Person();
CodeKit.CopyObjectProperties(source, target);
// target.Name = "John", target.Age = 30
```

```csharp
public static long GetObjectSize(object testObject)
```
Gets the approximate size of an object by serializing it to JSON.

**Example:**
```csharp
var obj = new { Name = "Test", Value = 123 };
long size = CodeKit.GetObjectSize(obj); // Returns size in bytes
```

```csharp
public static T ConvertToGeneric<T>(object obj)
```
Converts an object to a generic type.

**Example:**
```csharp
object value = "123";
int number = CodeKit.ConvertToGeneric<int>(value); // 123
```

#### Directory and File Operations

```csharp
public static bool DirectoryExists(string path, int millisecondsTimeout = 5000)
```
Checks if a directory exists with a timeout.

**Example:**
```csharp
bool exists = CodeKit.DirectoryExists(@"C:\MyFolder", 3000);
```

```csharp
public static bool FindApplicationProcess(string procName, bool killProc = false)
public static bool FindApplicationProcess(string procName, ref string exeName, bool killProc = false)
```
Finds a process by name, optionally killing it.

**Example:**
```csharp
bool found = CodeKit.FindApplicationProcess("notepad");
string exePath = string.Empty;
bool foundAndKilled = CodeKit.FindApplicationProcess("notepad", ref exePath, killProc: true);
```

#### String Operations

```csharp
public static string StringToBase64(string sOriginal)
public static string Base64ToString(string sBase64Str)
public static string ReverseString(string target)
public static string URLSafeString(string input)
public static string MakeSlug(string title)
public static string ExtractHexDigits(string input)
```

**Example:**
```csharp
string encoded = CodeKit.StringToBase64("Hello World");
string decoded = CodeKit.Base64ToString(encoded);
string reversed = CodeKit.ReverseString("Hello"); // "olleH"
string urlSafe = CodeKit.URLSafeString("Hello World!"); // "hello-world"
string slug = CodeKit.MakeSlug("My Article Title"); // "my-article-title"
string hex = CodeKit.ExtractHexDigits("#FF00AA"); // "FF00AA"
```

#### Random Number Generation

```csharp
public static int RandomNumber(int min, int max)
public static DateTime RandomDate(Random generator, DateTime rangeStart, DateTime rangeEnd)
public static TimeSpan RandomTime(int startHour, int endHour)
public static BigInteger RandomBigInteger()
```

**Example:**
```csharp
int random = CodeKit.RandomNumber(1, 100);
var generator = new Random();
DateTime randomDate = CodeKit.RandomDate(generator, DateTime.Now, DateTime.Now.AddDays(30));
TimeSpan randomTime = CodeKit.RandomTime(9, 17); // Random time between 9 AM and 5 PM
BigInteger bigInt = CodeKit.RandomBigInteger(); // From GUID
```

#### Paging Utilities

```csharp
public static int CalculateNumberOfPages(int totalNumberOfItems, int pageSize)
```
Calculates the number of pages required for paging.

**Example:**
```csharp
int totalItems = 100;
int pageSize = 20;
int pages = CodeKit.CalculateNumberOfPages(totalItems, pageSize); // 5
```

#### Rounding

```csharp
public static double Round(double x, int numerator, int denominator)
```
Rounds a floating point value to a custom precision (e.g., 0.05, 0.25).

**Example:**
```csharp
double rounded = CodeKit.Round(12.1436, 5, 100); // 12.15 (precision = 0.05)
```

#### Delay and Async Operations

```csharp
public static Task Delay(double milliseconds)
public static void Delay2(double amountOfTime, Rules.TimeIntervalTypesEnum timeGranule = Rules.TimeIntervalTypesEnum.MilliSeconds)
public static Task Delay3(int ms, Action doThis)
public static async Task ForEachWithDelay<T>(this ICollection<T> items, Func<T, Task> action, double interval)
```

**Example:**
```csharp
await CodeKit.Delay(1000); // Delay 1 second
CodeKit.Delay2(5000, Rules.TimeIntervalTypesEnum.MilliSeconds); // Synchronous delay

var items = new List<int> { 1, 2, 3, 4, 5 };
await items.ForEachWithDelay(async item => {
    await ProcessItem(item);
}, 1000); // Process each item with 1 second delay
```

#### Stream Operations

```csharp
public static Stream StringToStream(string s)
```
Converts a string to a MemoryStream.

**Example:**
```csharp
Stream stream = CodeKit.StringToStream("Hello World");
```

#### String Matching

```csharp
public static bool IsEntryFound(string entry, params string[] test)
public static bool IsEntryFound(string entry, bool caseDownAll, params string[] test)
```
Checks if an entry is found in a test array.

**Example:**
```csharp
bool found = CodeKit.IsEntryFound("test", "one", "two", "test"); // true
bool foundCaseInsensitive = CodeKit.IsEntryFound("TEST", true, "one", "two", "test"); // true
```

#### Email Validation

```csharp
public static bool IsValidEmail(string email)
```
Validates an email address.

**Example:**
```csharp
bool isValid = CodeKit.IsValidEmail("user@example.com"); // true
bool isInvalid = CodeKit.IsValidEmail("invalid-email"); // false
```

#### URL Extraction

```csharp
public static List<string> GetAllUrLsInText(string text)
```
Extracts all URLs from a text string.

**Example:**
```csharp
string text = "Visit https://example.com and http://test.com";
List<string> urls = CodeKit.GetAllUrLsInText(text);
// Returns: ["https://example.com", "http://test.com"]
```

#### Threading Operations

```csharp
public static void SpawnAndWait(params Action[] actions)
public static void SpawnAndWait(IEnumerable<Action> actions)
```
Spawns multiple actions in parallel and waits for all to complete.

**Example:**
```csharp
CodeKit.SpawnAndWait(
    () => DoTask1(),
    () => DoTask2(),
    () => DoTask3()
);
```

#### GUID Operations

```csharp
public static string Short(this Guid guid)
public static string Shorten(this Guid guid)
public static string GenerateIdentityCode(this Guid guid, int length = 6)
```

**Example:**
```csharp
Guid guid = Guid.NewGuid();
string shortGuid = guid.Short(); // Base64 encoded, URL-safe
string shortened = guid.Shorten(); // Alternative shortening
string identityCode = guid.GenerateIdentityCode(6); // 6-character alphanumeric code
```

#### Utility Methods

```csharp
public static TOut FuncInvoke<TOut>(Func<TOut> func)
public static int GetHashCodeSafe<T>(T target)
public static string GetPropertyName<T>(T item) where T : class
public static string GetCurrentMethod()
public static bool CheckForInternetConnection()
public static void Swap<T>(IList<T> list, int indexA, int indexB)
public static bool IsArrayOf<T>(this Type type)
public static bool AreStringsAnagrams(string a, string b)
public static string GetMonthFromPartial(string partialName)
public static KeyValuePair<string, object> KeyValOf(string key, object value)
public static bool IsNullOrZero(this int? nullableInt)
```

**Example:**
```csharp
var result = CodeKit.FuncInvoke(() => CalculateValue());
int hash = CodeKit.GetHashCodeSafe<string>("test");
string propName = CodeKit.GetPropertyName(new { Name = "Test" }); // "Name"
string methodName = CodeKit.GetCurrentMethod(); // Gets calling method name
bool hasInternet = CodeKit.CheckForInternetConnection();

var list = new List<int> { 1, 2, 3, 4 };
CodeKit.Swap(list, 0, 3); // [4, 2, 3, 1]

bool isAnagram = CodeKit.AreStringsAnagrams("listen", "silent"); // true
string month = CodeKit.GetMonthFromPartial("Jan"); // "January"
```

---

### 2. FileHelper (Static Class)

Utility class for file operations including reading, writing, logging, and hashing.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers`

#### File Writing Methods

```csharp
public static void WriteToFile(string filePath, string data, bool append = true)
public static void WriteToFile2(string filePath, string data)
public static bool WriteContentToFile(string contentPathUri, string content, string encryptionKey = null)
```

**Example:**
```csharp
FileHelper.WriteToFile("log.txt", "Log entry", append: true);
FileHelper.WriteToFile("data.txt", "New content", append: false);
FileHelper.WriteContentToFile("secret.txt", "Sensitive data", "MyEncryptionKey");
```

#### File Reading Methods

```csharp
public static string[] ReadFromFileEachLine(string textFile)
public static IEnumerable<string> ReadFromFileEachLine2(string textFile)
public static IEnumerable<T> ReadFromFileEachLine3<T>(string textFile, Func<string, T> parserAction)
public static IEnumerable<T> ReadFromFileEachLine3<T>(string[] stringData, Func<string, T> parserAction)
public static IEnumerable<string> ReadFromFileEachLine4(string textfile)
public static bool ReadContentFromFile(string contentPathUri, out string content, string encryptionKey = null)
```

**Example:**
```csharp
string[] lines = FileHelper.ReadFromFileEachLine("data.txt");
IEnumerable<string> lines2 = FileHelper.ReadFromFileEachLine2("data.txt");

// Parse lines to objects
IEnumerable<int> numbers = FileHelper.ReadFromFileEachLine3("numbers.txt", 
    line => int.Parse(line));

// Read encrypted content
bool success = FileHelper.ReadContentFromFile("secret.txt", out string content, "MyEncryptionKey");
```

#### Filename Formatting

```csharp
public static string FormatShortDateForfilename(DateTime date, bool includeTime = false)
public static string FormatShortDateForfilename(DateTime date, string prefix, string suffix, string extension, bool includeTime = false)
```

**Example:**
```csharp
string filename = FileHelper.FormatShortDateForfilename(DateTime.Now, "log", "backup", "txt", includeTime: true);
// Result: "log-2025-01-15_02-30-45-PM-backup.txt"

string simple = FileHelper.FormatShortDateForfilename(DateTime.Now); // "20250115"
```

#### File Hashing

```csharp
public static string GetFileMD5HashChecksum(string filePath)
```
Calculates MD5 checksum for a file.

**Example:**
```csharp
string hash = FileHelper.GetFileMD5HashChecksum("document.pdf");
```

#### File Information

```csharp
public static long GetFileSize(string filePath)
public static bool IsFilePathAccessible(string filepath)
```

**Example:**
```csharp
long size = FileHelper.GetFileSize("document.pdf");
bool accessible = FileHelper.IsFilePathAccessible("document.pdf");
```

#### Logging

```csharp
public static void LogMessage(string logFile, string LogMessage)
```
Logs a message to a file.

**Example:**
```csharp
FileHelper.LogMessage("app.log", "Application started at " + DateTime.Now);
```

#### Directory Operations

```csharp
public static void Recurse(DirectoryInfo directory, bool asReadOnly = false)
```
Recursively sets read-only attribute for all files in a directory.

**Example:**
```csharp
var dir = new DirectoryInfo(@"C:\MyFolder");
FileHelper.Recurse(dir, asReadOnly: true);
```

---

### 3. Cryptor (Static Class)

Cryptographic utilities for encryption, decryption, and hashing.

**Namespace**: `SMEAppHouse.Core.CodeKits.Encryptions`

#### AES Encryption/Decryption

```csharp
public static string EncryptStringAES(string plainText, string sharedSecretKey)
public static string DecryptStringAES(string cipherText, string sharedSecretKey)
```
Encrypts/decrypts strings using AES algorithm with a shared secret key.

**Example:**
```csharp
string secret = "MySecretKey123";
string plainText = "Sensitive Data";
string encrypted = Cryptor.EncryptStringAES(plainText, secret);
string decrypted = Cryptor.DecryptStringAES(encrypted, secret);
```

#### MD5/TripleDES Encryption

```csharp
public static string EncryptStringMD5(string plainText, string sharedSecretKey)
public static string DecryptStringMD5(string cipherText, string sharedSecretKey)
```
Encrypts/decrypts using MD5-hashed TripleDES.

**Example:**
```csharp
string encrypted = Cryptor.EncryptStringMD5("Data", "Key");
string decrypted = Cryptor.DecryptStringMD5(encrypted, "Key");
```

#### Salted Hash Generation

```csharp
public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
```
Generates a salted hash using SHA256.

**Example:**
```csharp
byte[] data = Encoding.UTF8.GetBytes("password");
byte[] salt = Encoding.UTF8.GetBytes("somesalt");
byte[] hash = Cryptor.GenerateSaltedHash(data, salt);
```

---

### 4. JsonHelper (Static Class)

JSON serialization, deserialization, and manipulation utilities.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers`

#### JSON Formatting

```csharp
public static string FormatJson(string str)
public static string FixJsonString(string jsonStr)
public static string NormalizeJson(string json)
```
Formats and normalizes JSON strings.

**Example:**
```csharp
string json = "{\"name\":\"John\",\"age\":30}";
string formatted = JsonHelper.FormatJson(json); // Pretty-printed JSON
```

#### JSON File Operations

```csharp
public static string ReadJsonFileAsString(string jsonFile)
public static JObject ReadJson(string jsonFile)
public static TObject ReadJson<TObject>(string jsonFilePath) where TObject : class
```

**Example:**
```csharp
string jsonContent = JsonHelper.ReadJsonFileAsString("data.json");
JObject jsonObj = JsonHelper.ReadJson("data.json");
MyClass obj = JsonHelper.ReadJson<MyClass>("data.json");
```

#### JSON Deserialization

```csharp
public static dynamic DeserializeJson(string jsonData)
public static T Deserialize<T>(string json)
public static string Serialize<T>(T obj)
```

**Example:**
```csharp
dynamic obj = JsonHelper.DeserializeJson("{\"name\":\"John\"}");
MyClass obj2 = JsonHelper.Deserialize<MyClass>("{\"name\":\"John\"}");
string json = JsonHelper.Serialize(myObject);
```

#### JToken Reading Methods

```csharp
public static string ReadString(JToken token)
public static bool ReadBoolean(JToken token)
public static bool? ReadNullableBoolean(JToken token)
public static int ReadInteger(JToken token)
public static int? ReadNullableInteger(JToken token)
public static long ReadLong(JToken token)
public static long? ReadNullableLong(JToken token)
public static double ReadFloat(JToken token)
public static double? ReadNullableFloat(JToken token)
public static DateTime ReadDate(JToken token)
public static DateTime? ReadNullableDate(JToken token)
public static object ReadObject(JToken token)
public static T ReadStronglyTypedObject<T>(JToken token) where T : class
public static T GetValue<T>(this JToken jToken, string key, T defaultValue = default(T))
```

**Example:**
```csharp
JObject json = JObject.Parse("{\"name\":\"John\",\"age\":30}");
string name = JsonHelper.ReadString(json["name"]);
int age = JsonHelper.ReadInteger(json["age"]);
string name2 = json.GetValue<string>("name", "Default");
```

#### Array and Dictionary Reading

```csharp
public static T[] ReadArray<T>(JToken token, ValueReader<T> reader)
public static Dictionary<string, T> ReadDictionary<T>(JToken token)
```

**Example:**
```csharp
JArray array = JArray.Parse("[1,2,3,4,5]");
int[] numbers = JsonHelper.ReadArray(array, JsonHelper.ReadInteger);
```

---

### 5. RetryCodeKit (Static Class)

Retry logic utilities for handling transient failures.

**Namespace**: `SMEAppHouse.Core.CodeKits.Tools`

#### Retry Methods

```csharp
public static async Task RetryOnExceptionAsync(int times, TimeSpan delay, Func<Task> operation)
public static async Task RetryOnExceptionAsync<TException>(int times, TimeSpan delay, Func<Task> operation) where TException : Exception
public static T Do<T>(Func<T> func, TimeSpan retryInterval, int retryCount = 3, bool ignoreFinalException = false, Action<RetryLogWarningEvent> retryLogWarn = null)
public static void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
public static bool Retry(Func<bool> retryAction, int iteration = 5, int iterationTimeout = 5000)
public static bool Retry(Func<bool> retryAction, ref Exception finalExceptionThrown, int maxretry = 5, int iterationTimeout = 5000, Action<RetryLogWarningEvent> retryLogWarn = null)
```

**Example:**
```csharp
// Retry async operation
await RetryCodeKit.RetryOnExceptionAsync(3, TimeSpan.FromSeconds(1), async () => {
    await SomeUnreliableOperation();
});

// Retry with specific exception type
await RetryCodeKit.RetryOnExceptionAsync<HttpRequestException>(
    3, TimeSpan.FromSeconds(2), async () => {
        await HttpCall();
    });

// Retry with return value
var result = RetryCodeKit.Do(() => {
    return GetData();
}, TimeSpan.FromSeconds(1), retryCount: 5);

// Retry with logging
RetryCodeKit.Do(() => {
    ProcessData();
}, TimeSpan.FromSeconds(1), retryCount: 3, retryLogWarn: (event) => {
    Console.WriteLine(event.WarningMessage);
});
```

#### Loop and Conditional Retry

```csharp
public static bool DoWhileError(Action action, int iterationLimit = 3, int iterationTimeout = 3000)
public static bool DoWhileError(Action action, DoWhileErrorCallbackDelegate exceptionOccuredEvent, int iterationLimit = 3, int iterationTimeout = 3000, Action<RetryLogWarningEvent> retryLogWarn = null)
public static bool LoopRetry(Func<bool> retryAction, Func<bool> successQualifier, int iterationLimit = 0, int iterationTimeout = 5000)
public static void LoopAction(Func<bool> loopAction, bool exitWhenActionIsTrue = true, int iterationLimit = 0, int iterationTimeout = 5000)
```

**Example:**
```csharp
bool success = RetryCodeKit.DoWhileError(() => {
    ProcessData();
}, iterationLimit: 5, iterationTimeout: 2000);

RetryCodeKit.LoopRetry(
    retryAction: () => CheckStatus(),
    successQualifier: () => IsComplete(),
    iterationLimit: 10,
    iterationTimeout: 1000
);
```

#### Try and Ignore

```csharp
public static bool TryAndIgnore(Action tryAction)
public static bool TryAndIgnore(Action tryAction, ref Exception exception)
```

**Example:**
```csharp
bool succeeded = RetryCodeKit.TryAndIgnore(() => {
    RiskyOperation();
});

Exception ex = null;
bool succeeded2 = RetryCodeKit.TryAndIgnore(() => {
    RiskyOperation();
}, ref ex);
```

---

### 6. ExpressionBuilder (Class)

Builds LINQ expressions dynamically for filtering and querying.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers`

#### Operators

```csharp
public enum OperatorComparer
{
    Contains,
    StartsWith,
    EndsWith,
    Equals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqualTo,
    NotEqual
}
```

#### Methods

```csharp
public static Expression<Func<T, bool>> BuildPredicate<T>(object test, OperatorComparer comparer, params string[] properties)
public static Expression<Func<T, bool>> GetExpression<T>(List<Filter> filters)
```

**Example:**
```csharp
var builder = new ExpressionBuilder();
var expr = ExpressionBuilder.BuildPredicate<Person>(
    "John", 
    ExpressionBuilder.OperatorComparer.Equals, 
    "Name"
);
var filtered = persons.Where(expr.Compile());

// Using filters
var filters = new List<Filter>
{
    new Filter { PropertyName = "Age", Value = 30, Comparer = OperatorComparer.GreaterThan },
    new Filter { PropertyName = "Name", Value = "John", Comparer = OperatorComparer.Contains }
};
var expression = ExpressionBuilder.GetExpression<Person>(filters);
var results = persons.Where(expression.Compile());
```

---

### 7. ExpressionsHelper (Static Class)

Helper methods for working with LINQ expressions.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers.Expressions`

#### Methods

```csharp
public static Expression<Func<TSource, bool>> Equal<TSource>(Expression<Func<TSource, object>> propertySelector, object value)
public static Expression<Func<TSource, bool>> NotEqual<TSource>(Expression<Func<TSource, object>> propertySelector, object value)
// ... and more comparison methods
```

**Example:**
```csharp
var expr = ExpressionsHelper.Equal<Person>(p => p.Name, "John");
var filtered = persons.Where(expr.Compile());
```

---

### 8. SerializableDictionary<TKey, TValue> (Class)

A dictionary that can be serialized to and deserialized from XML.

**Namespace**: `SMEAppHouse.Core.CodeKits.Data`

**Example:**
```csharp
var dict = new SerializableDictionary<string, int>
{
    { "One", 1 },
    { "Two", 2 },
    { "Three", 3 }
};

// Serialize to XML
var serializer = new XmlSerializer(typeof(SerializableDictionary<string, int>));
using var writer = new StringWriter();
serializer.Serialize(writer, dict);
string xml = writer.ToString();

// Deserialize from XML
using var reader = new StringReader(xml);
var deserialized = (SerializableDictionary<string, int>)serializer.Deserialize(reader);
```

---

### 9. CountryInfo (Class)

Represents country information with ID, code, name, and currency.

**Namespace**: `SMEAppHouse.Core.CodeKits.Geo`

#### Properties

- `int Id` - Country ID
- `string Code` - Country code (ISO)
- `string Name` - Country name
- `string Currency` - Currency name

**Example:**
```csharp
var country = new CountryInfo(1, "US", "United States", "USD");
Console.WriteLine($"{country.Name} ({country.Code}): {country.Currency}");
```

---

### 10. GeoKit (Class)

Provides access to country information.

**Namespace**: `SMEAppHouse.Core.CodeKits.Geo`

#### Properties

```csharp
public static List<CountryInfo> CountryInfos { get; }
```

**Example:**
```csharp
var countries = GeoKit.CountryInfos;
var usa = countries.FirstOrDefault(c => c.Code == "US");
```

---

### 11. DateTimeSpan (Struct)

Represents a time span between two dates with detailed breakdown.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers`

#### Properties

- `int Years`
- `int Months`
- `int Days`
- `int Hours`
- `int Minutes`
- `int Seconds`
- `int Milliseconds`

**Example:**
```csharp
DateTime start = new DateTime(2020, 1, 1);
DateTime end = new DateTime(2025, 12, 31);
DateTimeSpan span = DateTimeSpan.CalculateDifference(start, end);
Console.WriteLine($"{span.Years} years, {span.Months} months, {span.Days} days");
```

---

### 12. HtmlResourceLoader (Class)

Loads HTML resources from URLs or files.

**Namespace**: `SMEAppHouse.Core.CodeKits.Tools`

**Example:**
```csharp
var loader = new HtmlResourceLoader();
string html = loader.LoadFromUrl("https://example.com");
```

---

### 13. User32Interop (Static Class)

Windows API interop utilities.

**Namespace**: `SMEAppHouse.Core.CodeKits.Tools`

---

## Extension Methods

### String Extensions

**Namespace**: `SMEAppHouse.Core.CodeKits.Extensions`

#### Methods

```csharp
public static string Left(this string param, int length)
public static string Right(this string param, int length)
public static string Mid(this string param, int startIndex, int length)
public static string Mid(string param, int startIndex)
public static string ToCamelCase(this string phrase)
public static string FromCamelCase(string camelCase)
public static string SplitQuotedLine(this string value, char separator, bool quotes)
```

**Example:**
```csharp
string text = "Hello World";
string left = text.Left(5); // "Hello"
string right = text.Right(5); // "World"
string mid = text.Mid(6, 5); // "World"
string camelCase = "hello world".ToCamelCase(); // "HelloWorld"
string fromCamel = StringExt.FromCamelCase("HelloWorld"); // "Hello World"
```

---

### DateTime Extensions

**Namespace**: `SMEAppHouse.Core.CodeKits.Extensions`

#### Methods

```csharp
public static DateTime FirstDayOfThisMonth()
public static DateTime EndOfLastMonth()
public static DateTime FirstDayOfPreviousMonths(int months = 1)
public static int CalculateElapsedTime(DateTime since, DateTime now, Rules.TimeIntervalTypesEnum dateTimeScale)
public static bool IsSameDay(this DateTime datetime1, DateTime datetime2)
public static bool IsInExactTime(DateTime xTime, DateTime[] qeuedTimes)
public static bool IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
public static bool IsDateBetween(DateTime target, DateTime dateFrom, DateTime dateTo)
```

**Example:**
```csharp
DateTime firstDay = DateTimeExt.FirstDayOfThisMonth();
DateTime lastMonth = DateTimeExt.EndOfLastMonth();
bool sameDay = DateTime.Now.IsSameDay(DateTime.Today);
bool inRange = DateTimeExt.IsTimeOfDayBetween(DateTime.Now, TimeSpan.FromHours(9), TimeSpan.FromHours(17));
```

---

### Enum Extensions

**Namespace**: `SMEAppHouse.Core.CodeKits.Extensions`

**Example:**
```csharp
public enum Status { Active, Inactive }
Status status = Status.Active;
string description = status.GetDescription(); // If Description attribute exists
```

---

### Collection Extensions

**Namespace**: `SMEAppHouse.Core.CodeKits.Extensions`

**Example:**
```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
bool isEmpty = numbers.IsEmpty();
var chunked = numbers.Chunk(2);
```

---

## Helper Classes

### AsyncHelper

Helper for async operations.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers`

**Example:**
```csharp
AsyncHelper.RunSync(async () => {
    await SomeAsyncOperation();
});
```

### ExceptionHelpers

Exception handling utilities.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers`

**Example:**
```csharp
string messages = exception.GetExceptionMessages();
```

### ShortCodes

Generates short codes.

**Namespace**: `SMEAppHouse.Core.CodeKits.Helpers`

---

## Complete Usage Examples

### Example 1: File Operations with Encryption

```csharp
using SMEAppHouse.Core.CodeKits;
using SMEAppHouse.Core.CodeKits.Helpers;
using SMEAppHouse.Core.CodeKits.Encryptions;

// Write encrypted data to file
string data = "Sensitive information";
string secretKey = "MySecretKey123";
string encrypted = Cryptor.EncryptStringAES(data, secretKey);
FileHelper.WriteContentToFile("encrypted.txt", encrypted, secretKey);

// Read and decrypt
bool success = FileHelper.ReadContentFromFile("encrypted.txt", out string decrypted, secretKey);
Console.WriteLine(decrypted); // "Sensitive information"
```

### Example 2: Object Property Copying

```csharp
using SMEAppHouse.Core.CodeKits;

public class Source
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class Target
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}

var source = new Source { Name = "John", Age = 30 };
var target = new Target { Address = "123 Main St" };

CodeKit.CopyObjectProperties(source, target);
// target.Name = "John", target.Age = 30, target.Address = "123 Main St"
```

### Example 3: Retry Logic

```csharp
using SMEAppHouse.Core.CodeKits.Tools;

// Retry an operation with exponential backoff
var result = RetryCodeKit.Do(() => {
    return CallExternalAPI();
}, TimeSpan.FromSeconds(2), retryCount: 5, retryLogWarn: (event) => {
    Logger.Warning(event.WarningMessage);
});

// Retry async operation
await RetryCodeKit.RetryOnExceptionAsync<HttpRequestException>(
    3, 
    TimeSpan.FromSeconds(1), 
    async () => {
        await HttpClient.GetAsync("https://api.example.com/data");
    }
);
```

### Example 4: JSON Operations

```csharp
using SMEAppHouse.Core.CodeKits.Helpers;
using Newtonsoft.Json.Linq;

// Read JSON file
JObject json = JsonHelper.ReadJson("config.json");
string value = JsonHelper.ReadString(json["setting"]);

// Deserialize to object
MyConfig config = JsonHelper.ReadJson<MyConfig>("config.json");

// Serialize object
string jsonString = JsonHelper.Serialize(myObject);

// Format JSON
string prettyJson = JsonHelper.FormatJson(compactJson);
```

### Example 5: Expression Building

```csharp
using SMEAppHouse.Core.CodeKits.Helpers;

// Build dynamic filter expression
var filters = new List<Filter>
{
    new Filter 
    { 
        PropertyName = "Age", 
        Value = 25, 
        Comparer = ExpressionBuilder.OperatorComparer.GreaterThan 
    },
    new Filter 
    { 
        PropertyName = "Name", 
        Value = "John", 
        Comparer = ExpressionBuilder.OperatorComparer.Contains 
    }
};

var expression = ExpressionBuilder.GetExpression<Person>(filters);
var filteredPersons = persons.Where(expression.Compile()).ToList();
```

### Example 6: String and GUID Operations

```csharp
using SMEAppHouse.Core.CodeKits;

// String operations
string base64 = CodeKit.StringToBase64("Hello");
string decoded = CodeKit.Base64ToString(base64);
string slug = CodeKit.MakeSlug("My Article Title!"); // "my-article-title"

// GUID operations
Guid guid = Guid.NewGuid();
string shortGuid = guid.Short(); // URL-safe short GUID
string identityCode = guid.GenerateIdentityCode(8); // 8-character code

// Email validation
bool isValid = CodeKit.IsValidEmail("user@example.com");

// URL extraction
string text = "Visit https://example.com for more info";
List<string> urls = CodeKit.GetAllUrLsInText(text);
```

---

## Dependencies

- Microsoft.CSharp (v4.7.0)
- Newtonsoft.Json (v13.0.4)

---

## Notes

- All static utility classes are thread-safe where applicable
- File operations use proper resource disposal
- Cryptographic operations use industry-standard algorithms (AES, MD5, SHA256)
- Extension methods follow .NET naming conventions
- Retry logic includes exponential backoff
- JSON operations support both Newtonsoft.Json and System.Text.Json where applicable

---

## License

Copyright © Nephiora IT Solutions 2025

