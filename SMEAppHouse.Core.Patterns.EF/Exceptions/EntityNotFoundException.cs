#nullable disable

using System;

namespace SMEAppHouse.Core.Common.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception
        where TEntity : class, new()
    {
        public EntityNotFoundException() 
            : base(message: $"Record of the {typeof(TEntity).Name} entity is not found.")
        {
        }
    }
}
