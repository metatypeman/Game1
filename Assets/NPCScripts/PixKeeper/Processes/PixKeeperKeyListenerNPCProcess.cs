using Assets.NPCScripts.Antagonist.Processes;
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
                case KeyCode.J:
                    GoToFarWayPoint();
                    break;
            }
        }

        private void GoToFarWayPoint()
        {
            //            var rifle = Context.GetLogicalObject("{: name='M4A1 Sopmod' :}");

            //#if UNITY_EDITOR
            //            Log($"rifle !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! = {rifle}");
            //#endif

            //            //var tmpB = Context.GetLogicalObject("{: name='TrafficBarrierHazards (1)' :}");

            //            //var tmpP = tmpB.GetValue<System.Numerics.Vector3?>("global position");

            //#if UNITY_EDITOR
            //            //Log($"tmpP = {tmpP}");
            //#endif

            //            if (rifle == null)
            //            {
            //                return;
            //            }

            //            var command = TakeFromSurfaceNPCProcess.CreateCommand(rifle);
            //            var task = Execute(command);
            //            Wait(task);

            //            command = SimpleAimNPCProcess.CreateCommand();
            //            task = Execute(command);
            //            Wait(task);

            //NProcessGoToTargetWaypoint("Cube_2");
            //NProcessGoToTargetWaypoint("WayPoint_av_1");

            //NProcessGoToTargetWaypoint("Cube_2");

            //var command = AntagonistAttack1NPCProcess.CreateCommand();
            var command = Attack2NPCProcess.CreateCommand();
            Execute(command);
        }

        private void NProcessGoToTargetWaypoint(string nameOfWaypoint)
        {
#if UNITY_EDITOR
            Log($"nameOfWaypoint = {nameOfWaypoint}");
#endif

            var command = GoToPointAndShootNPCProcess.CreateCommand(nameOfWaypoint);
            var task = ExecuteAsChild(command);
            //mTask = task;
            Wait(task);

            //            var targetWayPoint = Context.GetLogicalObject("{: name='" + nameOfWaypoint + "'&class='place' :}");

            //#if UNITY_EDITOR
            //            Log($"(targetWayPoint == null) = {targetWayPoint == null}");
            //#endif

            //            if (targetWayPoint == null)
            //            {
            //                return;
            //            }

            //            var targetPosition = targetWayPoint.GetValue<System.Numerics.Vector3?>("global position");

            //#if UNITY_EDITOR
            //            Log($"targetPosition = {targetPosition}");
            //#endif

            //            var command = GoToPointNPCProcess.CreateCommand(targetPosition.Value);
            //            var task = Execute(command);
            //            Wait(task);
        }
    }
}
