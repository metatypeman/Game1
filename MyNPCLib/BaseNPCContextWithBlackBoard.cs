using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseNPCContextWithBlackBoard<BlackBoardType>: BaseNPCContext 
        where BlackBoardType: BaseBlackBoard, new()
    {
        public BaseNPCContextWithBlackBoard(IEntityLogger entityLogger, IEntityDictionary entityDictionary = null, NPCProcessInfoCache npcProcessInfoCache = null, INPCHostContext npcHostContext = null)
            : base(entityLogger, entityDictionary, npcProcessInfoCache, npcHostContext)
        {
            mBlackBoard = new BlackBoardType();
            mBlackBoard.EntityLogger = entityLogger;
            mBlackBoard.Context = this;
        }

        private BlackBoardType mBlackBoard;

        public BlackBoardType BlackBoard
        {
            get
            {
                return mBlackBoard;
            }
        }

        public override object NoTypedBlackBoard => mBlackBoard;

        protected override void OnBootsrap()
        {
            base.OnBootsrap();
            mBlackBoard.Bootstrap();
        }
    }
}
