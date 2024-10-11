namespace SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces
{
    public interface IAuditableEntity
    {
        bool? IsArchived { get; set; }
        DateTime? DateArchived { get; set; }
        string ReasonArchived { get; set; }
    }
}