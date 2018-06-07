using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseNPCProcessWithBlackBoard<BlackBoardType> : BaseNPCProcess
        where BlackBoardType : class, new()
    {
        protected BlackBoardType BlackBoard;

        protected override void OnSetContext()
        {
            BlackBoard = Context.NoTypedBlackBoard as BlackBoardType;

            Task.Run(() => {
                try
                {
                    Awake();
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        protected virtual void Awake()
        {
        }
    }
}
