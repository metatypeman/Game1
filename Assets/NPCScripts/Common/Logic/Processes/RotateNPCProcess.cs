using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("rotate")]
    public class RotateNPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand(float angle)
        {
            var command = new NPCCommand();
            command.Name = "rotate";
            command.AddParam(nameof(angle), angle);
            return command;
        }

        private void Main(float angle)
        {
#if UNITY_EDITOR
            Log($"Begin angle = {angle}");
#endif

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.Rotate;
            tmpCommand.TargetPosition = new System.Numerics.Vector3(0, angle, 0);

            var tmpTask = ExecuteBody(tmpCommand);
            Wait(tmpTask);

#if UNITY_EDITOR
            Log($"End angle = {angle}");
#endif
        }
    }
}
