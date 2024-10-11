using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.DtoModelAbstraction;

public class GuidKeyedAuditableModel : IKeyedAuditableEntity<Guid>
{
    public required Guid Id { get; set; }
    public bool? IsActive { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime? DateModified { get; set; } = DateTime.Now;
    public bool? IsArchived { get; set; }
    public DateTime? DateArchived { get; set; }
    public string? ReasonArchived { get; set; }
}