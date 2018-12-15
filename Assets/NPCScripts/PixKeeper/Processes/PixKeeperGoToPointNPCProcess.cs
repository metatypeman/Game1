using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.PixKeeper.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("go to point")]
    public class PixKeeperGoToPointNPCProcess : PixKeeperBaseNPCProcess
    {
        public static NPCCommand CreateCommand(System.Numerics.Vector3 point)
        {
            var command = new NPCCommand();
            command.Name = "go to point";
            command.AddParam(nameof(point), point);
            return command;
        }

        private void Main(System.Numerics.Vector3 point)
        {
#if UNITY_EDITOR
            Log($"point = {point}");
#endif

            var moveCommand = new HumanoidHStateCommand();
            moveCommand.State = HumanoidHState.Walk;
            moveCommand.TargetPosition = point;

            var tmpTask = ExecuteBody(moveCommand);

            Wait(tmpTask);

#if UNITY_EDITOR
            Log("End");
#endif
        }
    }
}
