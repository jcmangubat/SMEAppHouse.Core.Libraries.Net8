namespace SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces
{
    public interface IKeyedEntity<TPk> : IEntity
        where TPk : struct
    {
        TPk Id { get; set; }
    }
}
