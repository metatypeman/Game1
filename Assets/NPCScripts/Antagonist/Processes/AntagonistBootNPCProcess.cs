using Assets.NPCScripts.Common.Logic.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Antagonist.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class AntagonistBootNPCProcess : CommonBaseNPCProcess
    {
        protected override void Awake()
        {
#if UNITY_EDITOR
            Log("Begin :)");
#endif

            //GoToTargetWayPoint("FarWaypoint");
        }

        private void Main()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif

            var command = TakeRifleFromBagPackNPCProcess.CreateCommand();
            var task = Execute(command);
            Wait(task);

            Wait(100000);

#if UNITY_EDITOR
            Log("TakeRifleFromBagPackNPCProcess");
#endif

            command = SimpleAimNPCProcess.CreateCommand();
            task = Execute(command);
            Wait(task);

#if UNITY_EDITOR
            Log("SimpleAimNPCProcess");
#endif

            command = StartShootingNPCProcess.CreateCommand();
            Execute(command);

            var nameOfWaypoint = "Cube_3";

#if UNITY_EDITOR
            Log("StartShootingNPCProcess");
#endif

            command = GoToPointNPCProcess.CreateCommand(nameOfWaypoint);
            task = Execute(command);
            Wait(task);

#if UNITY_EDITOR
            Log("GoToPointNPCProcess");
#endif
            task.OnRanToCompletionChanged += (INPCProcess sender) =>
            {
                command = StopShootingNPCProcess.CreateCommand();
                Execute(command);

#if UNITY_EDITOR
                Log("StopShootingNPCProcess");
#endif
            };
        }
    }
}
