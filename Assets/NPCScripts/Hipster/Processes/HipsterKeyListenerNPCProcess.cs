using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NPCScripts.Hipster.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("key press")]
    public class HipsterKeyListenerNPCProcess: HipsterBaseNPCProcess
    {
        public static NPCCommand CreateCommand(KeyCode key)
        {
            var command = new NPCCommand();
            command.Name = "key press";
            command.AddParam(nameof(key), key);
            return command;
        }

        private void Main(KeyCode key)
        {
#if UNITY_EDITOR
            Log($"key = {key}");
#endif

            //switch (key)
            //{
            //    case KeyCode.F:
            //        ProcessGoToRedWaypoint();
            //        break;

            //    case KeyCode.G:
            //        ProcessGoToGreenWaypoint();
            //        break;

            //    case KeyCode.H:
            //        ProcessGoToBlueWaypoint();
            //        break;

            //    case KeyCode.J:
            //        ProcessGoToYellowWaypoint();
            //        break;

            //    case KeyCode.K:
            //        ProcessStop();
            //        break;

            //    case KeyCode.L:
            //        ProcessContinue();
            //        break;
            //}
        }

        private void ProcessGoToRedWaypoint()
        {
            ProcessGoToTargetWaypoint("RedWaypoint");
        }

        private void ProcessGoToGreenWaypoint()
        {
            ProcessGoToTargetWaypoint("GreenWaypoint");
        }

        private void ProcessGoToBlueWaypoint()
        {
            ProcessGoToTargetWaypoint("BlueWaypoint");
        }

        private void ProcessGoToYellowWaypoint()
        {
            ProcessGoToTargetWaypoint("YellowWaypoint");
        }

        private void ProcessGoToTargetWaypoint(string nameOfWaypoint)
        {
            var targetWayPoint = Context.GetLogicalObject("{: name='" + nameOfWaypoint + "'&class='place' :}");

            if (targetWayPoint == null)
            {
                return;
            }

            var moveCommand = new HumanoidHStateCommand();
            moveCommand.State = HumanoidHState.Walk;
            moveCommand.TargetPosition = targetWayPoint.GetValue<System.Numerics.Vector3?>("global position");

            BlackBoard.LastCommand = moveCommand;

#if UNITY_EDITOR
            Log($"moveCommand = {moveCommand}");
#endif

            /*var childProcess = */ExecuteBody(moveCommand);
            //Wait(childProcess);
        }

        private void ProcessStop()
        {
            //Log("Begin");

            var moveCommand = new HumanoidHStateCommand();
            moveCommand.State = HumanoidHState.Stop;

            ExecuteBody(moveCommand);
        }

        private void ProcessContinue()
        {
            var moveCommand = BlackBoard.LastCommand;

            if (moveCommand == null)
            {
                return;
            }

            ExecuteBody(moveCommand);
        }
    }
}
