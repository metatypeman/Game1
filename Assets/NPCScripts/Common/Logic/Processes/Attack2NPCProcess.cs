using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("attack2")]
    public class Attack2NPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "attack2";
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

            var nameOfWaypoint = "WayPoint_av_1";

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

            var ethan = Context.GetLogicalObject("{: name='John' :}");

#if UNITY_EDITOR
            Log($"ethan = {ethan}");
#endif

            var target = Context.GetLogicalObject("{: name='Ivan' :}");

#if UNITY_EDITOR
            Log($"target = {target}");
#endif

            var exceptEntities = new List<BaseAbstractLogicalObject>() { ethan };

            var trigger = CreateTrigger(() =>
            {
                if (BlackBoard.VisibleObjects.Any(p => p == target) && !BlackBoard.VisibleObjects.Any(p => p == ethan))
                {
                    return true;
                }

                return false;
            }, 100);

            trigger.OnFire += () => {
                if(BlackBoard.IsShooting)
                {
                    return;
                }

                var command = SafetyShootingNPCProcess.CreateCommand(target, exceptEntities);
                var task = Execute(command);
                Wait(task);
            };

            while (InfinityCondition)
            {
#if UNITY_EDITOR
                Log("---------------------------------------------------------------");
#endif

                var command = GoToPointNPCProcess.CreateCommand(nameOfWaypoint);
                var task = ExecuteAsChild(command);
                mTask = task;

#if UNITY_EDITOR
                Log($"task.GetHashCode() (45) = {task.GetHashCode()}");
#endif

                Wait(30000);

#if UNITY_EDITOR
                Log($"task.State = {task.State}");
#endif

                if (task.State == StateOfNPCProcess.RanToCompletion)
                {
#if UNITY_EDITOR
                    Log("task.State == StateOfNPCProcess.RanToCompletion break;");
#endif
                    return;
                }

                task.Cancel();

#if UNITY_EDITOR
                Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif

                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Stop;

                ExecuteBody(moveCommand);

                command = SearchAndShootNPCProcess.CreateCommand(target, exceptEntities);
                task = Execute(command);
                Wait(task);
            }
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
