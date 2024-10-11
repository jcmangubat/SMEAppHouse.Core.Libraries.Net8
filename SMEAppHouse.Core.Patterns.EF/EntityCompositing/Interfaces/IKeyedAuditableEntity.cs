namespace SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces
{
    public interface IKeyedAuditableEntity<TPk> :
        IKeyedEntity<TPk>, IAuditableEntity
        where TPk : struct
    {
    }
}
