namespace SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces
{
    public interface IEntityKeyed<TPk> : IEntity
        where TPk : struct
    {
        TPk Id { get; set; }
    }
}
