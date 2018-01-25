using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TstRootProcess: BaseNPCProcess
    {
        public TstRootProcess(NPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TstRootProcess OnRun Status = {Status}");
#endif
            BaseNPCProcess tmpChildProcess = new TstInspectingProcess(Context);
            mChildProcesses.Add(tmpChildProcess);
            tmpChildProcess.RunAsync();

            tmpChildProcess = new TstGoToEnemyBaseProcess(Context);
            mChildProcesses.Add(tmpChildProcess);
            tmpChildProcess.RunAsync();

            WaitProsesses(mChildProcesses);

#if UNITY_EDITOR
            Debug.Log("End TstRootProcess OnRun");
#endif
        }

        private List<BaseNPCProcess> mChildProcesses = new List<BaseNPCProcess>();
    }

    public class TstInspectingProcess : BaseNPCProcess
    {
        public TstInspectingProcess(NPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TstInspectingProcess OnRun");
#endif

#if UNITY_EDITOR
            Debug.Log("End TstInspectingProcess OnRun");
#endif
        }
    }

    public class TstGoToEnemyBaseProcess : BaseNPCProcess
    {
        public TstGoToEnemyBaseProcess(NPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TstGoToEnemyBaseProcess OnRun");
#endif

            var targetWayPoint = WaypointsBus.GetByTag("enemy military base");

            if (targetWayPoint != null)
            {
                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                Debug.Log($"TstGoToEnemyBaseProcess moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);

#if UNITY_EDITOR
                Debug.Log($"TstGoToEnemyBaseProcess tmpTask = {tmpTask}");
#endif
                //mEnemyController.Execute(moveCommand);
            }

#if UNITY_EDITOR
            Debug.Log("End TstGoToEnemyBaseProcess OnRun");
#endif
        }
    }

    public class TstRunAwayProcess : BaseNPCProcess
    {
        public TstRunAwayProcess(NPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TstRunAwayProcess OnRun");
#endif


#if UNITY_EDITOR
            Debug.Log("End TstRunAwayProcess OnRun");
#endif
        }
    }
}
