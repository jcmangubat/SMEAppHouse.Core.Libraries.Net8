namespace SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces
{
    public interface IEntityKeyedArchivable<TPk> : 
        IEntityKeyed<TPk>, IEntityArchivable
        where TPk : struct
    {
    }
}
