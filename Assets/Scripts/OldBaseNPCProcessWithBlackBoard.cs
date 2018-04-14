using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class OldBaseNPCProcessWithBlackBoard<T> : OldBaseNPCProcess where T: class
    {
        protected OldBaseNPCProcessWithBlackBoard()
        {
        }

        protected OldBaseNPCProcessWithBlackBoard(OldNPCProcessesContext context)
            : base(context)
        {
#if UNITY_EDITOR
            //Debug.Log("OldBaseNPCProcessWithBlackBoard constructor");
#endif
        }

        protected override void OnChangeContext()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldBaseNPCProcessWithBlackBoard OnChangeContext");
#endif

            var blackBoard = Context.GetInstance<T>();

            lock(mBlackBoardLockObj)
            {
                mBlackBoard = blackBoard;
            }

#if UNITY_EDITOR
            //Debug.Log($"End OldBaseNPCProcessWithBlackBoard OnChangeContext (blackBoard == null) = {blackBoard == null}");
#endif
        }

        private object mBlackBoardLockObj = new object();
        private T mBlackBoard = default(T);

        public T BlackBoard
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
