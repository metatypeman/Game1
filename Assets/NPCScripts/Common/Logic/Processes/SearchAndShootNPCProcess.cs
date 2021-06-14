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
//    [NPCProcessName("search and shoot")]
//    public class SearchAndShootNPCProcess : CommonBaseNPCProcess
//    {
//        private static readonly string COMMAND_NAME = "search and shoot";
//        private static readonly float DefaultAngle = 90f;

//        public static NPCCommand CreateCommand(BaseAbstractLogicalObject entity)
//        {
//            return CreateCommand(entity, new List<BaseAbstractLogicalObject>(), DefaultAngle);
//        }

//        public static NPCCommand CreateCommand(BaseAbstractLogicalObject target, List<BaseAbstractLogicalObject> exceptEntities)
//        {
//            return CreateCommand(target, exceptEntities, DefaultAngle);
//        }

//        public static NPCCommand CreateCommand(BaseAbstractLogicalObject target, List<BaseAbstractLogicalObject> exceptEntities, float angle)
//        {
//            var command = new NPCCommand();
//            command.Name = COMMAND_NAME;
//            command.AddParam(nameof(angle), angle);
//            command.AddParam(nameof(target), target);
//            command.AddParam(nameof(exceptEntities), exceptEntities);
//            return command;
//        }

//        private void Main(BaseAbstractLogicalObject target, List<BaseAbstractLogicalObject> exceptEntities, float angle)
//        {
//#if UNITY_EDITOR
//            Log($"target = {target}");
//            Log($"exceptEntities.Count = {exceptEntities.Count}");
//            Log($"angle = {angle}");
//#endif

//            var command = SearchNearNPCProcess.CreateCommand(target);
//            var task = Execute(command);
//            Wait(task);

//#if UNITY_EDITOR
//            Log($"task.State = {task.State}");
//#endif

//            if (task.State == StateOfNPCProcess.RanToCompletion)
//            {
//                command = SafetyShootingNPCProcess.CreateCommand(target, exceptEntities);
//                task = Execute(command);
//                Wait(task);
//            }
//        }
//    }
//}
