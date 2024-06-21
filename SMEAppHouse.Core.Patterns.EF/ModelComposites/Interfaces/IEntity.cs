using System;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces
{
    public interface IEntity
    {
        bool? IsActive { get; set; }

        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}