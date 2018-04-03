using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContextWithResources<T>: BaseNPCContextWithBlackBoard<T>
    {
        public BaseNPCContextWithResources(IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null)
            : base(entityDictionary, npcProcessInfoCache)
        {
        }
    }
}
