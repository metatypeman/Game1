using MyNPCLib;
using MyNPCLib.NavigationSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("go to point")]
    public class GoToPointNPCProcess: CommonBaseNPCProcess
    {
        private static string COMMAND_NAME = "go to point";

        public static NPCCommand CreateCommand(string name)
        {
            var command = new NPCCommand();
            command.Name = COMMAND_NAME;
            command.AddParam(nameof(name), name);
            return command;
        }

        public static NPCCommand CreateCommand(System.Numerics.Vector3 point)
        {
            var command = new NPCCommand();
            command.Name = COMMAND_NAME;
            command.AddParam(nameof(point), point);
            return command;
        }

        private void Main(string name)
        {
#if UNITY_EDITOR
            Log($"name = {name} Id = {Id}");
#endif

            var targetWayPoint = Context.GetLogicalObject("{: name='" + name + "'&class='place' :}");

#if UNITY_EDITOR
            Log($"(targetWayPoint == null) = {targetWayPoint == null}");
#endif

            if (targetWayPoint == null)
            {
                State = StateOfNPCProcess.Faulted;
                return;
            }

            var targetPosition = targetWayPoint.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Log($"targetPosition = {targetPosition}");
#endif

            var command = CreateCommand(targetPosition.Value);
            var task = ExecuteAsChild(command);
            //var task = Execute(command);
            mTask = task;

#if UNITY_EDITOR
            Log($"targetPosition task.GetHashCode() (1) = {task.GetHashCode()}");
#endif

            Wait(task);

#if UNITY_EDITOR
            Log($"task.State (1)= {task.State} task.GetHashCode() (1) = {task.GetHashCode()}");
#endif

            //State = task.State;

#if UNITY_EDITOR
            Log("End");
#endif
        }

        private INPCProcess mTask;

        protected override void CancelOfProcessChanged()
        {
#if UNITY_EDITOR
            Log($"CancelOfProcessChanged mTask?.GetHashCode() = {mTask?.GetHashCode()}");
#endif
            mTask?.Cancel();
            //mTask.Dispose();//This is not cancel

            base.CancelOfProcessChanged();
        }

        private void Main(System.Numerics.Vector3 point)
        {
#if UNITY_EDITOR
            Log($"point = {point} Id = {Id}");
#endif

            var startPosition = Context.SelfLogicalObject.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Log($"startPosition = {startPosition}");
#endif

            if(!startPosition.HasValue)
            {
                return;
            }

            var route = Context.GetRouteForPosition(startPosition.Value, point);

#if UNITY_EDITOR
            Log($"route (1) = {route}");
#endif

            if (route.Status == StatusOfRoute.Impossible)
            {
                //var moveCommand = new HumanoidHStateCommand();
                //moveCommand.State = HumanoidHState.Walk;
                //moveCommand.TargetPosition = point;
                State = StateOfNPCProcess.Faulted;
                //var tmpTask = ExecuteBody(moveCommand);
                //mTask = tmpTask;
                //Wait(tmpTask);

                return;
            }

            if(route.Status == StatusOfRoute.Finished)
            {
                return;
            }

            while (route.Status == StatusOfRoute.Processed && InfinityCondition)
            {
#if UNITY_EDITOR
                Log($"InfinityCondition = {InfinityCondition}");
                Log($"route = {route} GetHashCode() = {GetHashCode()}");
#endif

                if (route.NextPoints.Count == 0)
                {
                    return;
                }

                var pointInfo = route.NextPoints.First();

#if UNITY_EDITOR
                Log($"pointInfo = {pointInfo}");
#endif

                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.InitiatingProcessId = Id;
                moveCommand.TargetPosition = pointInfo.Position;

                var task = ExecuteBody(moveCommand);
                mTask = task;

#if UNITY_EDITOR
                Log($"targetPosition task.GetHashCode() (2) = {task.GetHashCode()}");
#endif

                Wait(task);

#if UNITY_EDITOR
                Log($"task.State (2) = {task.State} task.GetHashCode() (2) = {task.GetHashCode()}");
                Log("End Moving");
#endif

                if (task.State != StateOfNPCProcess.RanToCompletion)
                {
                    State = task.State;

#if UNITY_EDITOR
                    Log("task.State != StateOfNPCProcess.RanToCompletion !!!!! TTRTTTTTT");
#endif

                    return;
                }

                //startPosition = Context.SelfLogicalObject.GetValue<System.Numerics.Vector3?>("global position");

                //if (!startPosition.HasValue)
                //{
                //    return;
                //}

                //route = Context.GetRouteForPosition(startPosition.Value, point);
                route = Context.GetRouteForPosition(pointInfo);

#if UNITY_EDITOR
                Log($"next route = {route}  GetHashCode() = {GetHashCode()}");
#endif
                //break;
            }
            //var moveCommand = new HumanoidHStateCommand();
            //moveCommand.State = HumanoidHState.Walk;
            //moveCommand.TargetPosition = point;

            //var tmpTask = ExecuteBody(moveCommand);

            //Wait(tmpTask);

#if UNITY_EDITOR
            Log("End");
#endif
        }
    }
}
