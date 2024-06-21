using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;
using System;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposite.Abstractions.RefInterfaces
{
    public interface IGuidKeyedEntityAuditable : IEntityKeyedArchivable<Guid>
    {

    }
}