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
            Wait(task);

            var nameOfWaypoint = "Cube_3";

            while (true)
            {
#if UNITY_EDITOR
                Log("---------------------------------------------------------------");
#endif

                command = GoToPointNPCProcess.CreateCommand(nameOfWaypoint);
                task = Execute(command);

                Wait(1000);

#if UNITY_EDITOR
                Log($"task.State = {task.State}");
#endif

                if (task.State != StateOfNPCProcess.Running)
                {
                    break;
                }

                //task.Dispose();//This is not cancel

#if UNITY_EDITOR
                Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif

                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Stop;

                ExecuteBody(moveCommand);

#if UNITY_EDITOR
                Log(".................................................");
#endif

                Wait(1000);
            }
        }
    }
}
