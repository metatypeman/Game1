using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseNPCProcessWithBlackBoard<T> : BaseNPCProcess where T: class
    {
        protected BaseNPCProcessWithBlackBoard()
        {
        }

        protected BaseNPCProcessWithBlackBoard(NPCProcessesContext context)
            : base(context)
        {
#if UNITY_EDITOR
            Debug.Log("BaseNPCProcessWithBlackBoard constructor");
#endif
        }

        protected override void OnChangeContext()
        {
#if UNITY_EDITOR
            Debug.Log("Begin BaseNPCProcessWithBlackBoard OnChangeContext");
#endif

            var blackBoard = Context.GetInstance<T>();

            lock(mBlackBoardLockObj)
            {
                mBlackBoard = blackBoard;
            }

#if UNITY_EDITOR
            Debug.Log($"End BaseNPCProcessWithBlackBoard OnChangeContext (blackBoard == null) = {blackBoard == null}");
#endif
        }

        private object mBlackBoardLockObj = new object();
        private T mBlackBoard = default(T);

        protected T BlackBoard
        {
            get
            {
                lock (mBlackBoardLockObj)
                {
                    return mBlackBoard;
                }            
            }
        }
    }
}
