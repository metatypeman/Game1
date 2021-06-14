//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
//using MyNPCLib;

//namespace Assets.Scripts
//{
//    public class TestedBaseNPCProcess: BaseNPCProcessWithBlackBoard<TestedBlackBoard>
//    {
//        protected void GoToTargetWayPoint(string nameOfThisWaypoint, bool withWaiting = true)
//        {
//#if UNITY_EDITOR
//            //Log($"Begin nameOfThisWaypoint = {nameOfThisWaypoint} withWaiting = {withWaiting}");
//#endif
//            var targetWayPoint = Context.GetLogicalObject($"{{: name='{nameOfThisWaypoint}'&class='waypoint' :}}");
//            //var targetWayPoint = Context.GetLogicalObject($"{{: name='{nameOfThisWaypoint}':}}");

//#if UNITY_EDITOR
//            Log($"targetWayPoint = {targetWayPoint}");
//#endif

//            if (targetWayPoint != null)
//            {
//                var moveCommand = new HumanoidHStateCommand();
//                moveCommand.State = HumanoidHState.Walk;
//                moveCommand.TargetPosition = targetWayPoint.GetValue<Vector3?>("global position");

//#if UNITY_EDITOR
//                //Log($"moveCommand = {moveCommand}");
//#endif
//                var tmpTask = ExecuteBody(moveCommand);
//                //mTmpTask = tmpTask;
//#if UNITY_EDITOR
//                //Log($"tmpTask = {tmpTask}");
//#endif

//                if (withWaiting)
//                {
//                    Wait(tmpTask);
//                }
//            }

//#if UNITY_EDITOR
//            //Log("End");
//#endif
//        }
//    }
//}
