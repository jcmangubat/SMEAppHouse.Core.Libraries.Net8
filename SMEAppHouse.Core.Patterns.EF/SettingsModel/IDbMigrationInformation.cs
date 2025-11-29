namespace SMEAppHouse.Core.Patterns.EF.SettingsModel
{
    /// <summary>
    /// Interface for providing migration table and schema information for Entity Framework migrations.
    /// </summary>
    public interface IDbMigrationInformation
    {
        /// <summary>
        /// Gets or sets the name of the migrations history table.
        /// </summary>
        string MigrationTblName { get; set; }

        /// <summary>
        /// Gets or sets the database schema for the migrations history table.
        /// </summary>
        string DbSchema { get; set; }
    }
}
