using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContextWithBlackBoard<T>: BaseNPCContext
    {
        public BaseNPCContextWithBlackBoard(IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null)
            : base(entityDictionary, npcProcessInfoCache)
        {
        }


    }
}
