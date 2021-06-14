//using Assets.NPCScripts.Antagonist.Processes;
//using Assets.NPCScripts.Common.Logic.Processes;
//using MyNPCLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.NPCScripts.PixKeeper.Processes
//{
//    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
//    public class PixKeeperBootNPCProcess: CommonBaseNPCProcess
//    {
//        protected override void Awake()
//        {
//#if UNITY_EDITOR
//            Log("Begin :)");
//#endif

//            //GoToTargetWayPoint("RedWaypoint");
//        }

//        private void Main()
//        {
//#if UNITY_EDITOR
//            Log("Begin");
//#endif

//            //var rifle = Context.GetLogicalObject("{: name='M4A1 Sopmod' :}");

//#if UNITY_EDITOR
//            //Log($"rifle !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! = {rifle}");
//#endif

//            //var tmpB = Context.GetLogicalObject("{: name='TrafficBarrierHazards (1)' :}");

//            //var tmpP = tmpB.GetValue<System.Numerics.Vector3?>("global position");

//#if UNITY_EDITOR
//            //Log($"tmpP = {tmpP}");
//#endif

//            //if (rifle == null)
//            //{
//            //    return;
//            //}

//            //var command = TakeFromSurfaceNPCProcess.CreateCommand(rifle);
//            //var task = Execute(command);
//            //Wait(task);

//            //command = SimpleAimNPCProcess.CreateCommand();
//            //task = Execute(command);
//            //Wait(task);

//            ////command = ShootingNPCProcess.CreateCommand();
//            //command = StartShootingNPCProcess.CreateCommand();
//            //task = Execute(command);

//            //Wait(10000);
//            //task.Cancel();

//            //Wait(10000);

//            //var command = AntagonistAttack1NPCProcess.CreateCommand();
//            //Execute(command);
//        }
//    }
//}
