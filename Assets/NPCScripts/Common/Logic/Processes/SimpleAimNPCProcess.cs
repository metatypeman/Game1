//using MyNPCLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.NPCScripts.Common.Logic.Processes
//{
//    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
//    [NPCProcessName("simple aim")]
//    public class SimpleAimNPCProcess : CommonBaseNPCProcess
//    {
//        public static NPCCommand CreateCommand()
//        {
//            var command = new NPCCommand();
//            command.Name = "simple aim";
//            return command;
//        }

//        private void Main()
//        {
//#if UNITY_EDITOR
//            Log("Begin");
//#endif

//            var tmpCommand = new HumanoidHandsActionStateCommand();
//            tmpCommand.State = HumanoidHandsActionState.StrongAim;

//            var tmpTask = ExecuteBody(tmpCommand);

//#if UNITY_EDITOR
//            Log("End");
//#endif
//        }
//    }
//}
