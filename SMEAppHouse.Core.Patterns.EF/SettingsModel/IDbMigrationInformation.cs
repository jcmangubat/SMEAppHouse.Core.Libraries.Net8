namespace SMEAppHouse.Core.Patterns.EF.SettingsModel
{
    public interface IDbMigrationInformation
    {
        string MigrationTblName { get; set; }
        string DbSchema { get; set; }
    }
}
