using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public abstract class BaseNPCProcessWithBlackBoard<BlackBoardType> : BaseNPCProcess
        where BlackBoardType : class, new()
    {
        protected BlackBoardType BlackBoard;

        protected override void OnSetContext()
        {
            BlackBoard = Context.NoTypedBlackBoard as BlackBoardType;
        }
    }
}
