namespace SMEAppHouse.Core.Patterns.EF.SettingsModel;

/// <summary>
/// Default implementation of <see cref="IDbMigrationInformation"/> that provides
/// migration table and schema information for Entity Framework migrations.
/// </summary>
public class DbMigrationInformation : IDbMigrationInformation
{
    /// <summary>
    /// Gets or sets the name of the migrations history table.
    /// </summary>
    public required string MigrationTblName { get; set; }

    /// <summary>
    /// Gets or sets the database schema for the migrations history table.
    /// Defaults to "dbo" if not specified.
    /// </summary>
    public string DbSchema { get; set; } = "dbo";
}
