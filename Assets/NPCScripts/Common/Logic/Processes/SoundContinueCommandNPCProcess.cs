using MyNPCLib;
using MyNPCLib.LogicalSoundModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    public class SoundContinueCommandNPCProcess : CommonBaseNPCProcess
    {
        public void Main(LogicalSoundInfo logicalSoundInfo)
        {
            if (!BlackBoard.IsReadyForsoundCommandExecuting)
            {
                return;
            }

            Log($"NEXT logicalSoundInfo = {logicalSoundInfo}");

            var moveCommand = BlackBoard.LastCommand;

            if (moveCommand == null)
            {
                return;
            }

            ExecuteBody(moveCommand);
        }
    }
}
