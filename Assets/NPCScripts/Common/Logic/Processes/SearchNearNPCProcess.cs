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
//    [NPCProcessName("search near")]
//    public class SearchNearNPCProcess : CommonBaseNPCProcess
//    {
//        private static readonly string COMMAND_NAME = "search near";
//        private static readonly float DefaultAngle = 90f;
//        private static readonly float DEFAULT_DELTA = 5f;

//        public static NPCCommand CreateCommand(BaseAbstractLogicalObject entity)
//        {
//            return CreateCommand(entity, DefaultAngle);
//        }

//        public static NPCCommand CreateCommand(BaseAbstractLogicalObject entity, float angle)
//        {
//            var command = new NPCCommand();
//            command.Name = COMMAND_NAME;
//            command.AddParam(nameof(angle), angle);
//            command.AddParam(nameof(entity), entity);
//            return command;
//        }

//        private void Main(BaseAbstractLogicalObject entity, float angle)
//        {
//#if UNITY_EDITOR
//            Log($"Begin angle = {angle}");
//            Log($"entity = {entity}");
//            Log($"Id = {Id}");
//            Log($"Rotate to Angle angle = {angle} Id = {Id}");
//#endif

//            Main(entity, angle, angle, DEFAULT_DELTA);

//#if UNITY_EDITOR
//            Log("End");
//#endif
//        }

//        private void Main(BaseAbstractLogicalObject entity, float leftAngle, float rightAngle, float delta)
//        {
//#if UNITY_EDITOR
//            Log($"entity = {entity}");
//            Log($"leftAngle = {leftAngle}");
//            Log($"rightAngle = {rightAngle}");
//            Log($"delta = {delta}");
//            Log($"Id = {Id}");
//#endif

//            var targetPosition = entity.GetValue<System.Numerics.Vector3?>("global position");

//#if UNITY_EDITOR
//            Log($"targetPosition = {targetPosition}");
//#endif

//            if (targetPosition.HasValue)
//            {
//                return;
//            }

//            leftAngle = Math.Abs(leftAngle);
//            rightAngle = Math.Abs(rightAngle);
//            delta = Math.Abs(delta);

//            if (delta == 0f)
//            {
//                return;
//            }

//#if UNITY_EDITOR
//            Log("NEXT");
//#endif

//            if (leftAngle > 0f)
//            {
//#if UNITY_EDITOR
//                Log("Begin left while");
//#endif

//                if (RotateTo(entity, leftAngle, delta))
//                {
//                    return;
//                }

//#if UNITY_EDITOR
//                Log("End left while");
//#endif

//            }

//            if (rightAngle > 0f)
//            {
//#if UNITY_EDITOR
//                Log("Begin right while");
//#endif
//                if (leftAngle > 0f)
//                {
//                    if (RotateTo(entity, -leftAngle, delta))
//                    {
//                        return;
//                    }
//                }


//                if (RotateTo(entity, -rightAngle, delta))
//                {
//                    return;
//                }

//#if UNITY_EDITOR
//                Log("End left while");
//#endif
//            }

//            State = StateOfNPCProcess.Faulted;

//#if UNITY_EDITOR
//            Log("End");
//#endif
//        }

//        private bool RotateTo(BaseAbstractLogicalObject entity, float angle, float delta)
//        {
//#if UNITY_EDITOR
//            Log($"entity = {entity}");
//            Log($"angle = {angle}");
//            Log($"delta = {delta}");
//            Log($"Id = {Id}");
//#endif

//            var anglesList = ListHelper.GetRange(0, angle, delta);

//#if UNITY_EDITOR
//            Log($"anglesList.Count = {anglesList.Count}");
//#endif

//            var cmdDelta = delta;

//            if (angle < 0f)
//            {
//                cmdDelta = -1f * cmdDelta;
//            }

//#if UNITY_EDITOR
//            Log($"cmdDelta = {cmdDelta}");
//#endif

//            var targetPosition = entity.GetValue<System.Numerics.Vector3?>("global position");

//#if UNITY_EDITOR
//            Log($"targetPosition = {targetPosition}");
//#endif
//            if(targetPosition.HasValue)
//            //if (BlackBoard.VisibleObjects.Any(p => p == entity))
//            {
//                return true;
//            }

//            foreach (var currentAngle in anglesList)
//            {
//#if UNITY_EDITOR
//                Log($"currentAngle = {currentAngle}");
//#endif

//                var tmpCommand = new HumanoidHStateCommand();
//                tmpCommand.State = HumanoidHState.Rotate;
//                tmpCommand.TargetPosition = new System.Numerics.Vector3(0, cmdDelta, 0);

//                var tmpTask = ExecuteBody(tmpCommand);
//                //tmpTask.OnStateChanged += TmpTask_OnStateChanged;
//                Wait(tmpTask);

//#if UNITY_EDITOR
//                Log($"BlackBoard.VisibleObjects.Any(p => p == entity) = {BlackBoard.VisibleObjects.Any(p => p == entity)}");
//#endif

//                var targetPosition_1 = entity.GetValue<System.Numerics.Vector3?>("global position");

//#if UNITY_EDITOR
//                Log($"targetPosition_1 = {targetPosition_1}");
//#endif
//                if(targetPosition_1.HasValue)
//                //if (BlackBoard.VisibleObjects.Any(p => p == entity))
//                {
//                    return true;
//                }
//            }

//#if UNITY_EDITOR
//            Log("End");
//#endif

//            return false;
//        }
//    }
//}
