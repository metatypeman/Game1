using Assets.NPCScripts.Common.Logic.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Antagonist.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("attack1")]
    public class AntaginistAttack1NPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "attack1";
            return command;
        }

        private void Main()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif

            var command = TakeRifleFromBagPackNPCProcess.CreateCommand();
            var task = Execute(command);
            Wait(task);

            Wait(1000);

#if UNITY_EDITOR
            Log("TakeRifleFromBagPackNPCProcess");
#endif

            command = SimpleAimNPCProcess.CreateCommand();
            task = Execute(command);

#if UNITY_EDITOR
            Log($"task.GetHashCode() (23) = {task.GetHashCode()}");
#endif

            Wait(task);

            var nameOfWaypoint = "Cube_3";

            RunAndShoot(nameOfWaypoint);

            nameOfWaypoint = "Cube_2";

            RunAndShoot(nameOfWaypoint);

            nameOfWaypoint = "Cube_1";

            RunAndShoot(nameOfWaypoint);

            nameOfWaypoint = "WayPoint_av_1";

            RunAndShoot(nameOfWaypoint);
        }

        private void RunAndShoot(string nameOfWaypoint)
        {
#if UNITY_EDITOR
            Log($"nameOfWaypoint = {nameOfWaypoint}");
#endif

            var command = GoToPointAndShootNPCProcess.CreateCommand(nameOfWaypoint);
            var task = ExecuteAsChild(command);
            mTask = task;
            Wait(task);
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
    }
}
