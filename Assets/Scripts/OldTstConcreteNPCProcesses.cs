using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class OldTstRootProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstRootProcess(OldNPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log($"Begin OldTstRootProcess OnRun Status = {Status}");
#endif
            OldBaseNPCProcess tmpChildProcess = new OldTstInspectingProcess(Context);
            mChildProcesses.Add(tmpChildProcess);
            tmpChildProcess.RunAsync();

            tmpChildProcess = new OldTstGoToEnemyBaseProcess(Context);
            mChildProcesses.Add(tmpChildProcess);
            tmpChildProcess.RunAsync();

            WaitProsesses(mChildProcesses);

#if UNITY_EDITOR
            //Debug.Log("End OldTstRootProcess OnRun");
#endif
        }

        private List<OldBaseNPCProcess> mChildProcesses = new List<OldBaseNPCProcess>();
    }

    public class OldTstInspectingProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstInspectingProcess(OldNPCProcessesContext context)
            : base(context)
        {
            mRayScaner = context.GetInstance<INPCRayScaner>();
        }

        private INPCRayScaner mRayScaner;

        private bool ISawTrafficBarrierRed;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTstInspectingProcess OnRun");
#endif

            while (InfinityCycleCondition)
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
                //Debug.Log($"OldTstInspectingProcess OnRun items.Count = {items.Count}");
#endif

                var tmpItemsWithGameObjectsList = items.Where(p => p.GameObject != null).ToList();

#if UNITY_EDITOR
                //Debug.Log($"OldTstInspectingProcess OnRun tmpItemsWithGameObjectsList.Count = {tmpItemsWithGameObjectsList.Count}");
                foreach (var item in tmpItemsWithGameObjectsList)
                {
                    var gameObject = item.GameObject;

                    //Debug.Log($"OldTstInspectingProcess OnRun gameObject.Name = {gameObject.Name}");

                    if (gameObject.Name == "TrafficBarrierRed")
                    {
                        ISawTrafficBarrierRed = true;

                        var newProcess = new OldTstRunAwayProcess(Context);
                        newProcess.LocalPriority = OldBaseNPCProcessPriorities.BelowNormal;
                        newProcess.RunAsync();
                    }
                }
#endif

                Thread.Sleep(1000);
            }

#if UNITY_EDITOR
            //Debug.Log("End OldTstInspectingProcess OnRun");
#endif
        }
    }

    public class OldTstGoToEnemyBaseProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstGoToEnemyBaseProcess(OldNPCProcessesContext context)
            : base(context)
        {
            var tmpTimerInterval = new TimeSpan(0, 0, 1);
            //mTimer = new Timer(TimerCallback, null, tmpTimerInterval, tmpTimerInterval);
        }

        private Timer mTimer;

        private NPCMeshTask mTmpTask;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTstGoToEnemyBaseProcess OnRun");
#endif

            var targetWayPoint = WaypointsBus.GetByTag("enemy military base");
            
            if (targetWayPoint != null)
            {
                var moveCommand = new InternalHumanoidHStateCommand();
                moveCommand.State = InternalHumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                //Debug.Log($"OldTstGoToEnemyBaseProcess OnRun moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                mTmpTask = tmpTask;
#if UNITY_EDITOR
                //Debug.Log($"OldTstGoToEnemyBaseProcess OnRun tmpTask = {tmpTask}");
#endif

                WaitNPCMeshTask(tmpTask);
            }

#if UNITY_EDITOR
            //Debug.Log("End OldTstGoToEnemyBaseProcess OnRun");
#endif
        }

        private void TimerCallback(object state)
        {
            if(mTmpTask == null)
            {
                return;
            }

#if UNITY_EDITOR
            //Debug.Log($"TstGoToEnemyBaseProcess TimerCallback mTmpTask = {mTmpTask} Status = {Status}");
#endif          
        }

        protected override void OnDispose()
        {
            mTimer?.Dispose();
        }
    }

    public class OldTstRunAwayProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstRunAwayProcess(OldNPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTstRunAwayProcess OnRun");
#endif

            var targetName = "Cube_1";

            GoToTargetWayPoint(targetName);

#if UNITY_EDITOR
            //Debug.Log("End OldTstRunAwayProcess OnRun");
#endif
        }

        private void GoToTargetWayPoint(string nameOfThisWaypoint, bool withWaiting = true)
        {
#if UNITY_EDITOR
            //Debug.Log($"OldTstRunAwayProcess Begin GoToTargetWayPoint nameOfThisWaypoint = {nameOfThisWaypoint} withWaiting = {withWaiting}");
#endif

            var targetWayPoint = WaypointsBus.GetByName(nameOfThisWaypoint);

            if (targetWayPoint != null)
            {
                var moveCommand = new InternalHumanoidHStateCommand();
                moveCommand.State = InternalHumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAwayProcess GoToTargetWayPoint moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                //mTmpTask = tmpTask;
#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAwayProcess GoToTargetWayPoint tmpTask = {tmpTask}");
#endif

                if (withWaiting)
                {
                    WaitNPCMeshTask(tmpTask);
                }
            }

#if UNITY_EDITOR
            //Debug.Log("End OldTstRunAwayProcess GoToTargetWayPoint");
#endif
        }
    }

    public class OldTstRunAtOurBaseProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstRunAtOurBaseProcess(OldNPCProcessesContext context)
            : base(context)
        {
            var tmpTimerInterval = new TimeSpan(0, 0, 1);
            //mTimer = new Timer(TimerCallback, null, tmpTimerInterval, tmpTimerInterval);
        }

        private Timer mTimer;

        private NPCMeshTask mTmpTask;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTstRunAtOurBaseProcess OnRun");
#endif

            for (var n = 1; n <= 3; n++)
            {
                var targetName = $"Cube_{n}";

#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAtOurBaseProcess OnRun targetName = {targetName}");
#endif

                GoToTargetWayPoint(targetName);

#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAtOurBaseProcess OnRun goal '{targetName}' had achieved!!!!");
#endif
            }

            var targetName1 = "Cube_1";
            var targetName2 = "Cube_2";

            GoToTargetWayPoint(targetName1, false);

            Thread.Sleep(1000);

            GoToTargetWayPoint(targetName2);

            //"Cube_1"

#if UNITY_EDITOR
            //Debug.Log("End OldTstRunAtOurBaseProcess OnRun");
#endif
        }

        private void GoToTargetWayPoint(string nameOfThisWaypoint, bool withWaiting = true)
        {
#if UNITY_EDITOR
            //Debug.Log($"OldTstRunAtOurBaseProcess Begin GoToTargetWayPoint nameOfThisWaypoint = {nameOfThisWaypoint} withWaiting = {withWaiting}");
#endif

            var targetWayPoint = WaypointsBus.GetByName(nameOfThisWaypoint);

            if (targetWayPoint != null)
            {
                var moveCommand = new InternalHumanoidHStateCommand();
                moveCommand.State = InternalHumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAtOurBaseProcess GoToTargetWayPoint moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                mTmpTask = tmpTask;
#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAtOurBaseProcess GoToTargetWayPoint tmpTask = {tmpTask}");
#endif

                if (withWaiting)
                {
                    WaitNPCMeshTask(tmpTask);
                }        
            }

#if UNITY_EDITOR
            //Debug.Log("End OldTstRunAtOurBaseProcess GoToTargetWayPoint");
#endif
        }

        private void TimerCallback(object state)
        {
            if (mTmpTask == null)
            {
                return;
            }

#if UNITY_EDITOR
            //Debug.Log($"OldTstRunAtOurBaseProcess TimerCallback mTmpTask = {mTmpTask} Status = {Status}");
#endif          
        }

        protected override void OnDispose()
        {
            mTimer?.Dispose();
        }
    }

    public class OldTstSimpleAimProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstSimpleAimProcess(OldNPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTstSimpleAimProcess OnRun");
#endif

            var tmpCommand = new InternalHumanoidHandsActionStateCommand();
            tmpCommand.State = InternalHumanoidHandsActionState.StrongAim;

            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            //Debug.Log("End OldTstSimpleAimProcess OnRun");
#endif
        }

        private void GoToTargetWayPoint(string nameOfThisWaypoint, bool withWaiting = true)
        {
#if UNITY_EDITOR
            //Debug.Log($"OldTstRunAwayProcess Begin GoToTargetWayPoint nameOfThisWaypoint = {nameOfThisWaypoint} withWaiting = {withWaiting}");
#endif

            var targetWayPoint = WaypointsBus.GetByName(nameOfThisWaypoint);

            if (targetWayPoint != null)
            {
                var moveCommand = new InternalHumanoidHStateCommand();
                moveCommand.State = InternalHumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAwayProcess GoToTargetWayPoint moveCommand = {moveCommand}");
#endif
                var tmpTask = Execute(moveCommand);
                //mTmpTask = tmpTask;
#if UNITY_EDITOR
                //Debug.Log($"OldTstRunAwayProcess GoToTargetWayPoint tmpTask = {tmpTask}");
#endif

                if (withWaiting)
                {
                    WaitNPCMeshTask(tmpTask);
                }
            }

#if UNITY_EDITOR
            //Debug.Log("End OldTstRunAwayProcess GoToTargetWayPoint");
#endif
        }
    }

    public class OldTSTFireToEthanProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTSTFireToEthanProcess(OldNPCProcessesContext context, Vector3 targetPosition)
            : base(context)
        {
            mTargetPosition = targetPosition;
        }

        private Vector3 mTargetPosition;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log($"Begin OldTSTFireToEthanProcess OnRun mTargetPosition = {mTargetPosition}");
#endif

            var tmpCommand = new InternalHumanoidHStateCommand();
            tmpCommand.State = InternalHumanoidHState.AimAt;
            tmpCommand.TargetPosition = mTargetPosition;
            //var tmpCommand = new HumanoidHandsActionStateCommand();
            //tmpCommand.State = HumanoidHandsActionState.StrongAim;

            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            //Debug.Log("End OldTSTFireToEthanProcess OnRun");
#endif
        }
    }

    public class OldTSTRotateProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTSTRotateProcess(OldNPCProcessesContext context, float angle)
            : base(context)
        {
            mAngle = angle;
        }

        private float mAngle;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log($"Begin OldTSTRotateProcess OnRun mAngle = {mAngle}");
#endif

            var tmpCommand = new InternalHumanoidHStateCommand();
            tmpCommand.State = InternalHumanoidHState.Rotate;
            tmpCommand.TargetPosition = new Vector3(0, mAngle, 0);
            //var tmpCommand = new HumanoidHandsActionStateCommand();
            //tmpCommand.State = HumanoidHandsActionState.StrongAim;

            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            //Debug.Log("End OldTSTFireToEthanProcess OnRun");
#endif
        }
    }

    public class OldTSTRotateHeadProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTSTRotateHeadProcess(OldNPCProcessesContext context, float angle)
            : base(context)
        {
            mAngle = angle;
        }

        private float mAngle;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log($"Begin OldTSTRotateHeadProcess OnRun mAngle = {mAngle}");
#endif
            var nameOfThisWaypoint = "Cube_1";
            var targetWayPoint = WaypointsBus.GetByName(nameOfThisWaypoint);

#if UNITY_EDITOR
            //Debug.Log($"Begin OldTSTRotateHeadProcess OnRun targetWayPoint.Position = {targetWayPoint.Position}");
#endif

            var tmpCommand = new InternalHumanoidHeadStateCommand();
            //tmpCommand.State = HumanoidHeadState.LookAt;
            tmpCommand.State = InternalHumanoidHeadState.Rotate;
            //tmpCommand.TargetPosition = targetWayPoint.Position; // new Vector3(0, mAngle, 0);
            tmpCommand.TargetPosition = new Vector3(0, mAngle, 0);
            //var tmpCommand = new HumanoidHandsActionStateCommand();
            //tmpCommand.State = HumanoidHandsActionState.StrongAim;

            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            //Debug.Log("End OldTSTRotateHeadProcess OnRun");
#endif
        }
    }

    public class OldTSTHeadToForvardProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTSTHeadToForvardProcess(OldNPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTSTHeadToForvardProcess OnRun");
#endif
            var tmpCommand = new InternalHumanoidHeadStateCommand();
            tmpCommand.State = InternalHumanoidHeadState.LookingForward;
            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            //Debug.Log("End OldTSTHeadToForvardProcess OnRun");
#endif
        }
    }

    public class OldTSTMoveProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTSTMoveProcess(OldNPCProcessesContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTSTMoveProcess OnRun");
#endif

            var tmpCommand = new InternalHumanoidHStateCommand();
            tmpCommand.State = InternalHumanoidHState.Move;
            tmpCommand.TargetPosition = new Vector3(0, 0, 1);
            var tmpTask = Execute(tmpCommand);

#if UNITY_EDITOR
            //Debug.Log("End OldTSTMoveProcess OnRun");
#endif
        }
    }

    public class OldTSTTakeFromSurfaceProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTSTTakeFromSurfaceProcess(OldNPCProcessesContext context, int instanceId)
            : base(context)
        {
            mInstanceId = instanceId;
        }

        private int mInstanceId;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTSTTakeFromSurfaceProcess OnRun");
#endif

            var tmpCommand = new InternalHumanoidThingsCommand();
            tmpCommand.State = InternalKindOfHumanoidThingsCommand.Take;
            tmpCommand.InstanceId = mInstanceId;
            var tmpTask = Execute(tmpCommand);

            tmpTask.OnStateChangedToRanToCompletion += () => {
#if UNITY_EDITOR
                //Debug.Log("OldTSTTakeFromSurfaceProcess OnRun tmpTask.OnStateChangedToRanToCompletion");
#endif

                var targetGameObj = MyGameObjectsBus.GetObject(mInstanceId);
                var gun = targetGameObj.GetInstance<IRapidFireGun>();
                BlackBoard.RapidFireGunProxy.Instance = gun;
            };

#if UNITY_EDITOR
            //Debug.Log("End OldTSTTakeFromSurfaceProcess OnRun");
#endif
        }
    }

    public class OldTstHideRifleToBagPackProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstHideRifleToBagPackProcess(OldNPCProcessesContext context, int instanceId)
            : base(context)
        {
            mInstanceId = instanceId;
        }

        private int mInstanceId;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTstHideRifleToBagPackProcess OnRun");
#endif

            BlackBoard.RapidFireGunProxy.Instance = null;
            var tmpCommand = new InternalHumanoidThingsCommand();
            tmpCommand.State = InternalKindOfHumanoidThingsCommand.PutToBagpack;
            tmpCommand.InstanceId = mInstanceId;
            var tmpTask = Execute(tmpCommand);

            tmpTask.OnStateChangedToRanToCompletion += () => {
#if UNITY_EDITOR
                //Debug.Log("OldTstHideRifleToBagPackProcess OnRun tmpTask.OnStateChangedToRanToCompletion");
#endif

            };

#if UNITY_EDITOR
            //Debug.Log("End OldTstHideRifleToBagPackProcess OnRun");
#endif
        }
    }

    public class OldTstThrowOutToSurfaceRifleToSurfaceProcess : OldTstBaseConcreteNPCProcessWithBlackBoard
    {
        public OldTstThrowOutToSurfaceRifleToSurfaceProcess(OldNPCProcessesContext context, int instanceId)
            : base(context)
        {
            mInstanceId = instanceId;
        }

        private int mInstanceId;

        protected override void OnRun()
        {
#if UNITY_EDITOR
            //Debug.Log("Begin OldTstThrowOutToSurfaceRifleToSurfaceProcess OnRun");
#endif

            BlackBoard.RapidFireGunProxy.Instance = null;
            var tmpCommand = new InternalHumanoidThingsCommand();
            tmpCommand.State = InternalKindOfHumanoidThingsCommand.ThrowOutToSurface;
            tmpCommand.InstanceId = mInstanceId;
            var tmpTask = Execute(tmpCommand);

            tmpTask.OnStateChangedToRanToCompletion += () => {
#if UNITY_EDITOR
                //Debug.Log("OldTstThrowOutToSurfaceRifleToSurfaceProcess OnRun tmpTask.OnStateChangedToRanToCompletion");
#endif
            };

#if UNITY_EDITOR
            //Debug.Log("End OldTstThrowOutToSurfaceRifleToSurfaceProcess OnRun");
#endif
        }
    }
}
