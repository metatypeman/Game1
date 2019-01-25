using Assets.NPCScripts.Common.Logic.Processes;
using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Antagonist.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("attack1")]
    public class AntagonistAttack1NPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "attack1";
            return command;
        }

        private void Main()
        {
#if UNITY_EDITOR
            Log("Begin");
#endif

            var command = TakeRifleFromBagPackNPCProcess.CreateCommand();
            var task = Execute(command);
            Wait(task);

            Wait(1000);

#if UNITY_EDITOR
            Log("TakeRifleFromBagPackNPCProcess");
#endif

            command = SimpleAimNPCProcess.CreateCommand();
            task = Execute(command);

#if UNITY_EDITOR
            Log($"task.GetHashCode() (23) = {task.GetHashCode()}");
#endif

            Wait(task);

//            var ethan = Context.GetLogicalObject("{: name='John' :}");

//#if UNITY_EDITOR
//            Log($"ethan = {ethan}");
//#endif

//            var target = Context.GetLogicalObject("{: name='Ivan' :}");

//#if UNITY_EDITOR
//            Log($"target = {target}");
//#endif

//            command = SearchAndShootNPCProcess.CreateCommand(target, new List<BaseAbstractLogicalObject>() { ethan });
//            task = Execute(command);
//            Wait(task);

//            command = SearchNearNPCProcess.CreateCommand(target);
//            task = Execute(command);
//            Wait(task);

//#if UNITY_EDITOR
//            Log($"task.State = {task.State}");
//#endif

//            if(task.State == StateOfNPCProcess.RanToCompletion)
//            {
//                var targetPosition = target.GetValue<System.Numerics.Vector3?>("global position");

//#if UNITY_EDITOR
//                Log($"targetPosition = {targetPosition}");
//#endif

//                var ethanPosition = ethan.GetValue<System.Numerics.Vector3?>("global position");

//#if UNITY_EDITOR
//                Log($"ethanPosition = {ethanPosition}");
//                Log($"Context.VisibleObjects.Any(p => p == ethan) = {Context.VisibleObjects.Any(p => p == ethan)}");
//#endif

//                if(!(ethanPosition.HasValue || Context.VisibleObjects.Any(p => p == ethan)))
//                {
//                    if (targetPosition.HasValue)
//                    {
//                        var tmpCommand = new HumanoidHStateCommand();
//                        tmpCommand.State = HumanoidHState.AimAt;
//                        tmpCommand.TargetPosition = targetPosition;

//                        var tmpTask = ExecuteBody(tmpCommand);

//                        Wait(tmpTask);

//                        command = ShootingWithRotationNPCProcess.CreateCommand(/*3, 70*/);
//                        task = Execute(command);

//                        Wait(task);

//#if UNITY_EDITOR
//                        Log($"task.State (2) = {task.State}");
//#endif
//                    }
//                }
//            }


            //            command = ShootingNPCProcess.CreateCommand();
            //            //command = StartShootingNPCProcess.CreateCommand();
            //            task = Execute(command);
            //            //task.OnCanceledChanged += (INPCProcess sender) => {
            //            //    command = new NPCCommand();
            //            //    command.Name = "shoot off";

            //            //    ExecuteDefaultHand(command);
            //            //};

            //#if UNITY_EDITOR
            //            Log("qwetr");
            //#endif

            //            Wait(10000);
            //            task.Cancel();

            var nameOfWaypoint = "Cube_3";

            RunAndShoot(nameOfWaypoint);

            nameOfWaypoint = "Cube_2";

            RunAndShoot(nameOfWaypoint);

            nameOfWaypoint = "Cube_1";

            RunAndShoot(nameOfWaypoint);

            nameOfWaypoint = "WayPoint_av_1";

            RunAndShoot(nameOfWaypoint);

#if UNITY_EDITOR
            Log("End");
#endif
        }

        private void RunAndShoot(string nameOfWaypoint)
        {
#if UNITY_EDITOR
            Log($"nameOfWaypoint = {nameOfWaypoint}");
#endif

            var command = GoToPointAndShootNPCProcess.CreateCommand(nameOfWaypoint);
            var task = ExecuteAsChild(command);
            mTask = task;
            Wait(task);
        }

        private INPCProcess mTask;

        protected override void CancelOfProcessChanged()
        {
#if UNITY_EDITOR
            Log($"CancelOfProcessChanged mTask?.GetHashCode() = {mTask?.GetHashCode()}");
#endif
            mTask?.Cancel();
            //mTask.Dispose();//This is not cancel

            base.CancelOfProcessChanged();
        }
    }
}
