namespace SMEAppHouse.Core.Patterns.EF.SettingsModel;

public class DbMigrationInformation : IDbMigrationInformation
{
    public required string MigrationTblName { get; set; }
    public string DbSchema { get; set; } = "dbo";
}
