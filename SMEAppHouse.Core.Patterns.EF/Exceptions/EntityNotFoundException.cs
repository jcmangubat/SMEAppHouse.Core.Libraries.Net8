namespace SMEAppHouse.Core.Patterns.EF.Exceptions;

/// <summary>
/// Exception thrown when an entity of the specified type is not found in the database.
/// </summary>
/// <typeparam name="TEntity">The type of entity that was not found.</typeparam>
public class EntityNotFoundException<TEntity> : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class
    /// with a default message indicating the entity type was not found.
    /// </summary>
    public EntityNotFoundException()
        : base($"Entity of type {typeof(TEntity).Name} not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class
    /// with a message indicating the entity with the specified identifier was not found.
    /// </summary>
    /// <param name="id">The identifier of the entity that was not found.</param>
    public EntityNotFoundException(object id)
        : base($"Entity of type {typeof(TEntity).Name} with ID '{id}' not found.")
    {
    }
}