using System.IO;
using System.Reflection;

namespace SMEAppHouse.Core.CodeKits.Tools;

public class HtmlResourceLoader
{
    public static string LoadHtmlResource(string resourceName, string resourcePrincipal="Resources")
    {
        // Get the assembly where the resource is embedded
        Assembly assembly = Assembly.GetCallingAssembly();

        // Construct the full resource name including the namespace
        var fullResourceName = $"{assembly.GetName().Name}.{resourcePrincipal}.{resourceName}";

        // Load the resource stream
        using Stream stream = assembly.GetManifestResourceStream(fullResourceName) 
            ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");

        // Read the content of the resource
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}