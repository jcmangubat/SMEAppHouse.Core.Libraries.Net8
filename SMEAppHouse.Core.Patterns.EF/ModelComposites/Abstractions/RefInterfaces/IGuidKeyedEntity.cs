using System;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposite.Abstractions.RefInterfaces
{
    public interface IGuidKeyedEntity : IEntityKeyed<Guid>
    {

    }
}