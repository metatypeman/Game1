//using MyNPCLib;
//using MyNPCLib.Logical;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.NPCScripts.Common.Logic.Processes
//{
//    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
//    [NPCProcessName("take from surface")]
//    public class TakeFromSurfaceNPCProcess: CommonBaseNPCProcess
//    {
//        public static NPCCommand CreateCommand(BaseAbstractLogicalObject thing)
//        {
//            var command = new NPCCommand();
//            command.Name = "take from surface";
//            command.AddParam(nameof(thing), thing);
//            return command;
//        }

//        private void Main(BaseAbstractLogicalObject thing)
//        {
//#if UNITY_EDITOR
//            Log($"Begin thing = {thing}");
//#endif

//            var tmpCommand = new HumanoidThingsCommand();
//            tmpCommand.State = KindOfHumanoidThingsCommand.Take;
//            tmpCommand.Thing = thing;
//            var tmpTask = ExecuteBody(tmpCommand);

//            tmpTask.OnRanToCompletionChanged += (INPCProcess sender) => {
//#if UNITY_EDITOR
//                //Log("tmpTask.OnStateChangedToRanToCompletion");
//#endif

//                BlackBoard.EntityOfRifle = thing;
//            };

//#if UNITY_EDITOR
//            Log($"End thing = {thing}");
//#endif
//        }
//    }
//}
