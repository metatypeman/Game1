using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    public class StubOfHumanoidBodyController: IHumanoidBodyController
    {
        private StatesOfHumanoidBodyController mStates = new StatesOfHumanoidBodyController();

        public StatesOfHumanoidBodyController States
        {
            get
            {
                return mStates;
            }
        }

        public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

        private object mLockObj = new object();
        private HumanoidTaskOfExecuting mTargetStateForExecuting;

        public HumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidBody targetState)
        {
#if DEBUG
            //Debug.Log($"EnemyController ExecuteAsync targetState = {targetState}");
#endif

            lock (mLockObj)
            {
                if (mTargetStateForExecuting != null)
                {
                    mTargetStateForExecuting.State = StateOfHumanoidTaskOfExecuting.Canceled;
                }

                var targetStateForExecuting = new HumanoidTaskOfExecuting();
                targetStateForExecuting.ProcessedState = targetState;

                mTargetStateForExecuting = targetStateForExecuting;

#if DEBUG
                targetStateForExecuting.State = StateOfHumanoidTaskOfExecuting.Executed;//tmp
                //Debug.Log($"EnemyController ExecuteAsync mTargetStateQueue.Count = {mTargetStateQueue.Count}");
#endif

                return targetStateForExecuting;
            }
        }
    }
}
