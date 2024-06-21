using System;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces
{
    public interface IEntityArchivable
    {
        bool? IsArchived { get; set; }
        DateTime? DateArchived { get; set; }
        string ReasonArchived { get; set; }
    }
}