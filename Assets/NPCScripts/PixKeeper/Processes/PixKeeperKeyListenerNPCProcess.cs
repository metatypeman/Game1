using Assets.NPCScripts.Common.Logic.Processes;
using Assets.Scripts;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NPCScripts.PixKeeper.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("key press")]
    public class PixKeeperKeyListenerNPCProcess: CommonBaseNPCProcess
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

            switch (key)
            {
                case KeyCode.G:
                    GoToFarWayPoint();
                    break;
            }
        }

        private void GoToFarWayPoint()
        {
            NProcessGoToTargetWaypoint("Cube_2");
        }

        private void NProcessGoToTargetWaypoint(string nameOfWaypoint)
        {
#if UNITY_EDITOR
            Log($"nameOfWaypoint = {nameOfWaypoint}");
#endif

            var targetWayPoint = Context.GetLogicalObject("{: name='" + nameOfWaypoint + "'&class='place' :}");

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

            var command = GoToPointNPCProcess.CreateCommand(targetPosition.Value);
            Execute(command);
        }
    }
}
