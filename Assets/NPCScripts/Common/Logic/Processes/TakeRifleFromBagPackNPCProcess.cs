using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("take rifle from bagpack")]
    public class TakeRifleFromBagPackNPCProcess: CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "take rifle from bagpack";
            return command;
        }

        private void Main()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif

            var rifle = BlackBoard.EntityOfRifle;

            if(rifle == null)
            {
                return;
            }

#if UNITY_EDITOR
            Log("NEXT");
#endif

            var command = TakeFromSurfaceNPCProcess.CreateCommand(rifle);
            Execute(command);
        }
    }
}
