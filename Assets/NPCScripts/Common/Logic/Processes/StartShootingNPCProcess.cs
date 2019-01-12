using Assets.Scripts;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("start shooting")]
    public class StartShootingNPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "start shooting";
            return command;
        }

        private void Main()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif

            var fireMode = GetDefaultHandProperty<FireMode?>("FireMode");

#if UNITY_EDITOR
            Log($"fireMode = {fireMode}");
#endif

            if (fireMode.HasValue)
            {
                var fireModeValue = fireMode.Value;

                switch (fireModeValue)
                {
                    case FireMode.Single:
                        SingleShoting();
                        break;

                    case FireMode.Multiple:
                        MultipleShooting();
                        break;
                }
            }

#if UNITY_EDITOR
            Log("End");
#endif
        }

        private void MultipleShooting()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif

            var command = new NPCCommand();
            command.Name = "shoot on";

            ExecuteDefaultHand(command);

#if UNITY_EDITOR
            Log("End");
#endif
        }

        private void SingleShoting()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif

#if UNITY_EDITOR
            Log("End ");
#endif
        }
    }
}
