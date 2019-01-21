using Assets.NPCScripts.Common.Logic.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.PixKeeper.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class PixKeeperBootNPCProcess: CommonBaseNPCProcess
    {
        protected override void Awake()
        {
#if UNITY_EDITOR
            Log("Begin :)");
#endif

            //GoToTargetWayPoint("RedWaypoint");
        }

        private void Main()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif    
        }
    }
}
