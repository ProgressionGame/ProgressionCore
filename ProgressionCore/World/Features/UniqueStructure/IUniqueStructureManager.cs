using System.Collections;
using System.Collections.Generic;

namespace Progression.Engine.Core.World.Features.UniqueStructure
{
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IUniqueStructureManager<out TStructure>: IEnumerable<TStructure>, IUniqueStructureManager where TStructure : IUniqueStructure
    {
        new TStructure Get(int index);
    }
    
    public interface IUniqueStructureManager : IEnumerable
    {
        IUniqueStructure Get(int index);
        void Lock();
    }
}