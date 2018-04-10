using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContextWithBlackBoard<BlackBoardType>: BaseNPCContext
        where BlackBoardType: class, new()
    {
        public BaseNPCContextWithBlackBoard(IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null, INPCHostContext npcHostContext = null)
            : base(entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            mBlackBoard = new BlackBoardType();
        }

        private BlackBoardType mBlackBoard;

        public override object NoTypedBlackBoard => mBlackBoard;
    }
}
