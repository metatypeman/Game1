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
            Log($"point = {name}");
#endif

            var targetWayPoint = Context.GetLogicalObject("{: name='" + name + "'&class='place' :}");

#if UNITY_EDITOR
            Log($"(targetWayPoint == null) = {targetWayPoint == null}");
#endif

            if (targetWayPoint == null)
            {
                return;
            }

            var targetPosition = targetWayPoint.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Log($"targetPosition = {targetPosition}");
#endif

            var command = CreateCommand(targetPosition.Value);
            var task = Execute(command);

            Wait(task);

#if UNITY_EDITOR
            Log("End");
#endif
        }

        private void Main(System.Numerics.Vector3 point)
        {
#if UNITY_EDITOR
            Log($"point = {point}");
#endif

            var startPosition = Context.SelfLogicalObject.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Log($"startPosition = {startPosition}");
#endif

            var route = Context.GetRouteForPosition(startPosition.Value, point);

            while (route.Status == StatusOfRoute.Processed)
            {
#if UNITY_EDITOR
                Log($"route = {route}");
#endif

                if (route.NextPoints.Count == 0)
                {
                    break;
                }

                var pointInfo = route.NextPoints.First();

#if UNITY_EDITOR
                Log($"pointInfo = {pointInfo}");
#endif

                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.TargetPosition = pointInfo.Position;

                var tmpTask = ExecuteBody(moveCommand);
                Wait(tmpTask);

#if UNITY_EDITOR
                Log("End Moving");
#endif
                route = Context.GetRouteForPosition(pointInfo);

#if UNITY_EDITOR
                Log($"next route = {route}");
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
