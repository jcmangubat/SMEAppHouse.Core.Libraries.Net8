using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace SMEAppHouse.Core.Patterns.EF.Helpers;

public static class Utilities
{
    /// <summary>
    /// Updates the properties of the destination object with values from the source object.
    /// Only properties with matching names and types are copied.
    /// </summary>
    /// <typeparam name="TEntityDest">The type of the destination object.</typeparam>
    /// <typeparam name="TEntitySource">The type of the source object.</typeparam>
    /// <param name="destinationObject">The destination object to update.</param>
    /// <param name="sourceObject">The source object to copy values from.</param>
    /// <exception cref="ArgumentNullException">Thrown when destinationObject or sourceObject is null.</exception>
    public static void UpdateValuesFrom<TEntityDest, TEntitySource>(this TEntityDest destinationObject, TEntitySource sourceObject)
        where TEntitySource : class
        where TEntityDest : class
    {
        if (destinationObject == null)
            throw new ArgumentNullException(nameof(destinationObject));
        if (sourceObject == null)
            throw new ArgumentNullException(nameof(sourceObject));

        var sourceProperties = typeof(TEntitySource).GetProperties();
        // Cache destination properties in a dictionary for O(1) lookup instead of O(n) FirstOrDefault
        var destinationPropertiesDict = typeof(TEntityDest).GetProperties()
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var sourceProperty in sourceProperties)
        {
            if (!destinationPropertiesDict.TryGetValue(sourceProperty.Name, out var destinationProperty))
                continue;

            if (destinationProperty.PropertyType != sourceProperty.PropertyType)
                continue;

            var sourceValue = sourceProperty.GetValue(sourceObject);

            // Check if the property is nullable and the source value is null
            if (Nullable.GetUnderlyingType(destinationProperty.PropertyType) != null && sourceValue == null)
                continue; // Skip updating the destination property

            destinationProperty.SetValue(destinationObject, sourceValue);
        }
    }

    /// <summary>
    /// Reads a SQL script that is embedded as a resource named after the migration class.
    /// </summary>
    /// <typeparam name="MigrationType">The migration type the SQL file script is attached to.</typeparam>
    /// <param name="migrationType">The migration instance.</param>
    /// <param name="upOrDownScaleFilePrefix">Optional parameter providing a strategy to distinguish between UP or DOWN SQL scripts.</param>
    /// <returns>The content of the SQL file, or an empty string if not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when migrationType is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the migration attribute is not found or invalid.</exception>
    public static string ReadSql<MigrationType>(this MigrationType migrationType, string upOrDownScaleFilePrefix = "")
        where MigrationType : Migration
    {
        if (migrationType == null)
            throw new ArgumentNullException(nameof(migrationType));

        var type = typeof(MigrationType);
        var customAttribute = type.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(MigrationAttribute));
        
        if (customAttribute == null || customAttribute.ConstructorArguments.Count == 0)
            throw new InvalidOperationException($"MigrationAttribute not found on type {type.Name}.");

        var migrationAttributeName = customAttribute.ConstructorArguments[0].Value?.ToString();
        if (string.IsNullOrEmpty(migrationAttributeName))
            throw new InvalidOperationException($"MigrationAttribute value is null or empty on type {type.Name}.");

        if (!string.IsNullOrEmpty(upOrDownScaleFilePrefix))
        {
            migrationAttributeName += upOrDownScaleFilePrefix;
        }

        var assembly = Assembly.GetExecutingAssembly();
        var sqlFile = assembly.GetManifestResourceNames().FirstOrDefault(scriptFile => scriptFile.Contains(migrationAttributeName));
        if (string.IsNullOrEmpty(sqlFile))
            return string.Empty;

        using var stream = assembly.GetManifestResourceStream(sqlFile);
        if (stream == null)
            return string.Empty;

        using StreamReader reader = new(stream);
        var sqlScript = reader.ReadToEnd();

        return sqlScript;
    }
}
