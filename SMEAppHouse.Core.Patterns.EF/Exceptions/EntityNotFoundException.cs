#nullable disable

namespace SMEAppHouse.Core.Patterns.EF.Exceptions;

public class EntityNotFoundException<TEntity> : Exception
{
    public EntityNotFoundException()
        : base($"Entity of type {typeof(TEntity).Name} not found.")
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}