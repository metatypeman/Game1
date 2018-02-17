using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            mRayScaner = context.GetInstance<INPCRayScaner>();
        }

        private INPCRayScaner mRayScaner;

        private bool ISawTrafficBarrierRed;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TstInspectingProcess OnRun");
#endif

            while(InfinityCycleCondition)
            {
                if(ISawTrafficBarrierRed)
                {
                    break;
                }

                var items = mRayScaner.VisibleObjects;

                if(items.Count == 0)
                {
                    return;
                }

#if UNITY_EDITOR
                Debug.Log($"TstInspectingProcess OnRun items.Count = {items.Count}");
#endif

                var tmpItemsWithGameObjectsList = items.Where(p => p.GameObject != null).ToList();

#if UNITY_EDITOR
                Debug.Log($"TstInspectingProcess OnRun tmpItemsWithGameObjectsList.Count = {tmpItemsWithGameObjectsList.Count}");
                foreach(var item in tmpItemsWithGameObjectsList)
                {
                    var gameObject = item.GameObject;

                    Debug.Log($"TstInspectingProcess OnRun gameObject.Name = {gameObject.Name}");

                    if(gameObject.Name == "TrafficBarrierRed")
                    {
                        ISawTrafficBarrierRed = true;

                        var newProcess = new TstRunAwayProcess(Context);
                        newProcess.LocalPriority = BaseNPCProcessPriorities.BelowNormal;
                        newProcess.RunAsync();
                    }
                }
#endif

                Thread.Sleep(1000);
            }

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
            var tmpTimerInterval = new TimeSpan(0, 0, 1);
            mTimer = new Timer(TimerCallback, null, tmpTimerInterval, tmpTimerInterval);
        }

        private Timer mTimer;

        private NPCMeshTask mTmpTask;

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
                Debug.Log($"TstGoToEnemyBaseProcess OnRun moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                mTmpTask = tmpTask;
#if UNITY_EDITOR
                Debug.Log($"TstGoToEnemyBaseProcess OnRun tmpTask = {tmpTask}");
#endif

                WaitNPCMeshTask(tmpTask);
            }

#if UNITY_EDITOR
            Debug.Log("End TstGoToEnemyBaseProcess OnRun");
#endif
        }

        private void TimerCallback(object state)
        {
            if(mTmpTask == null)
            {
                return;
            }

#if UNITY_EDITOR
            Debug.Log($"TstGoToEnemyBaseProcess TimerCallback mTmpTask = {mTmpTask} Status = {Status}");
#endif          
        }

        protected override void OnDispose()
        {
            mTimer.Dispose();
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

            var targetName = "Cube_1";

            GoToTargetWayPoint(targetName);

#if UNITY_EDITOR
            Debug.Log("End TstRunAwayProcess OnRun");
#endif
        }

        private void GoToTargetWayPoint(string nameOfThisWaypoint, bool withWaiting = true)
        {
#if UNITY_EDITOR
            Debug.Log($"TstRunAwayProcess Begin GoToTargetWayPoint nameOfThisWaypoint = {nameOfThisWaypoint} withWaiting = {withWaiting}");
#endif

            var targetWayPoint = WaypointsBus.GetByName(nameOfThisWaypoint);

            if (targetWayPoint != null)
            {
                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                Debug.Log($"TstRunAwayProcess GoToTargetWayPoint moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                //mTmpTask = tmpTask;
#if UNITY_EDITOR
                Debug.Log($"TstRunAwayProcess GoToTargetWayPoint tmpTask = {tmpTask}");
#endif

                if (withWaiting)
                {
                    WaitNPCMeshTask(tmpTask);
                }
            }

#if UNITY_EDITOR
            Debug.Log("End TstRunAwayProcess GoToTargetWayPoint");
#endif
        }
    }

    public class TstRunAtOurBaseProcess: BaseNPCProcess
    {
        public TstRunAtOurBaseProcess(NPCProcessesContext context)
            : base(context)
        {
            var tmpTimerInterval = new TimeSpan(0, 0, 1);
            mTimer = new Timer(TimerCallback, null, tmpTimerInterval, tmpTimerInterval);
        }

        private Timer mTimer;

        private NPCMeshTask mTmpTask;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TstRunAtOurBaseProcess OnRun");
#endif

            for(var n = 1; n <= 3; n++)
            {
                var targetName = $"Cube_{n}";

#if UNITY_EDITOR
                Debug.Log($"TstRunAtOurBaseProcess OnRun targetName = {targetName}");
#endif

                GoToTargetWayPoint(targetName);

#if UNITY_EDITOR
                Debug.Log($"TstRunAtOurBaseProcess OnRun goal '{targetName}' had achieved!!!!");
#endif
            }

            var targetName1 = "Cube_1";
            var targetName2 = "Cube_2";

            GoToTargetWayPoint(targetName1, false);

            Thread.Sleep(1000);

            GoToTargetWayPoint(targetName2);

            //"Cube_1"

#if UNITY_EDITOR
            Debug.Log("End TstRunAtOurBaseProcess OnRun");
#endif
        }

        private void GoToTargetWayPoint(string nameOfThisWaypoint, bool withWaiting = true)
        {
#if UNITY_EDITOR
            Debug.Log($"TstRunAtOurBaseProcess Begin GoToTargetWayPoint nameOfThisWaypoint = {nameOfThisWaypoint} withWaiting = {withWaiting}");
#endif

            var targetWayPoint = WaypointsBus.GetByName(nameOfThisWaypoint);

            if (targetWayPoint != null)
            {
                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                Debug.Log($"TstRunAtOurBaseProcess GoToTargetWayPoint moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                mTmpTask = tmpTask;
#if UNITY_EDITOR
                Debug.Log($"TstRunAtOurBaseProcess GoToTargetWayPoint tmpTask = {tmpTask}");
#endif

                if(withWaiting)
                {
                    WaitNPCMeshTask(tmpTask);
                }        
            }

#if UNITY_EDITOR
            Debug.Log("End TstRunAtOurBaseProcess GoToTargetWayPoint");
#endif
        }

        private void TimerCallback(object state)
        {
            if (mTmpTask == null)
            {
                return;
            }

#if UNITY_EDITOR
            Debug.Log($"TstRunAtOurBaseProcess TimerCallback mTmpTask = {mTmpTask} Status = {Status}");
#endif          
        }

        protected override void OnDispose()
        {
            mTimer.Dispose();
        }
    }

    public class TstSimpleAimProcess : BaseNPCProcess
    {
        public TstSimpleAimProcess(NPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TstSimpleAimProcess OnRun");
#endif

            var tmpCommand = new HumanoidHandsActionStateCommand();
            tmpCommand.State = HumanoidHandsActionState.StrongAim;

            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            Debug.Log("End TstSimpleAimProcess OnRun");
#endif
        }

        private void GoToTargetWayPoint(string nameOfThisWaypoint, bool withWaiting = true)
        {
#if UNITY_EDITOR
            Debug.Log($"TstRunAwayProcess Begin GoToTargetWayPoint nameOfThisWaypoint = {nameOfThisWaypoint} withWaiting = {withWaiting}");
#endif

            var targetWayPoint = WaypointsBus.GetByName(nameOfThisWaypoint);

            if (targetWayPoint != null)
            {
                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                Debug.Log($"TstRunAwayProcess GoToTargetWayPoint moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                //mTmpTask = tmpTask;
#if UNITY_EDITOR
                Debug.Log($"TstRunAwayProcess GoToTargetWayPoint tmpTask = {tmpTask}");
#endif

                if (withWaiting)
                {
                    WaitNPCMeshTask(tmpTask);
                }
            }

#if UNITY_EDITOR
            Debug.Log("End TstRunAwayProcess GoToTargetWayPoint");
#endif
        }
    }

    public class TSTFireToEthanProcess : BaseNPCProcess
    {
        public TSTFireToEthanProcess(NPCProcessesContext context, Vector3 targetPosition)
            : base(context)
        {
            mTargetPosition = targetPosition;
        }

        private Vector3 mTargetPosition;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TSTFireToEthanProcess OnRun mTargetPosition = {mTargetPosition}");
#endif

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.AimAt;
            tmpCommand.TargetPosition = mTargetPosition;
            //var tmpCommand = new HumanoidHandsActionStateCommand();
            //tmpCommand.State = HumanoidHandsActionState.StrongAim;

            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            Debug.Log("End TSTFireToEthanProcess OnRun");
#endif
        }
    }

    public class TSTRotateProcess : BaseNPCProcess
    {
        public TSTRotateProcess(NPCProcessesContext context, float angle)
            : base(context)
        {
            mAngle = angle;
        }

        private float mAngle;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TSTRotateProcess OnRun mAngle = {mAngle}");
#endif

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.Rotate;
            tmpCommand.TargetPosition = new Vector3(0, mAngle, 0);
            //var tmpCommand = new HumanoidHandsActionStateCommand();
            //tmpCommand.State = HumanoidHandsActionState.StrongAim;

            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            Debug.Log("End TSTFireToEthanProcess OnRun");
#endif
        }
    }
}
