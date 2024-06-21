namespace SMEAppHouse.Core.Patterns.EF.SettingsModel
{
    public class DbMigrationInformation : IDbMigrationInformation
    {
        public string MigrationTblName { get; set; }
        public string DbSchema { get; set; } = "dbo";
    }
}
