using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("go and shoot")]
    public class GoToPointAndShootNPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand(string name)
        {
            var command = new NPCCommand();
            command.Name = "go and shoot";
            command.AddParam(nameof(name), name);
            return command;
        }

        private void Main(string name)
        {
#if UNITY_EDITOR
            Log($"name = {name}");
#endif

            //var shooting = false;
            //var needShhoting = false;
            //var iSee = false;

            //INPCProcess shootingTask = null;
            
            //var targetObject = Context.GetLogicalObject("{: team='"+ BlackBoard.Team +"' :}");
//            var targetObject = Context.GetLogicalObject("{: name='John' :}");

//            var trigger = CreateTrigger(() => {
//                if (BlackBoard.VisibleObjects.Any(p => p == targetObject))
//                {
//                    return true;
//                }

//                return false;
//            }, 100);

//            trigger.OnFire += () => {
//#if UNITY_EDITOR
//                Log("I see!!! needShhoting = {needShhoting} shooting = {shooting}");
//#endif

//                iSee = true;

//                if(needShhoting)
//                {
//                    if(shooting)
//                    {
//                        shooting = false;
//                        var command = StopShootingNPCProcess.CreateCommand();
//                        ExecuteAsChild(command);
//                    }
//                }
//            };

//            trigger.OnResetCondition += ()=> {
//#if UNITY_EDITOR
//                Log($"I do not see!!! needShhoting = {needShhoting} shooting = {shooting}");
//#endif

//                iSee = false;

//                if (needShhoting)
//                {
//                    if (!shooting)
//                    {
//                        shooting = true;
//                        var command = StartShootingNPCProcess.CreateCommand();
//                        Execute(command);
                        
//                    }
//                }
//            };

            while (InfinityCondition)
            {
#if UNITY_EDITOR
                Log("---------------------------------------------------------------");
#endif

                var command = GoToPointNPCProcess.CreateCommand(name);
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
                //task.Dispose();//This is not cancel

#if UNITY_EDITOR
                Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
#endif

                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Stop;

                ExecuteBody(moveCommand);

                //needShhoting = true;

                //if(!iSee)
                //{
                    //shooting = true;
                    command = StartShootingNPCProcess.CreateCommand();
                    Execute(command);
                //}

#if UNITY_EDITOR
                Log(".................................................");
#endif

                command = RotateNPCProcess.CreateCommand(60f);
                task = ExecuteAsChild(command);
                mTask = task;

                Wait(task);
                Wait(5000);

                command = RotateNPCProcess.CreateCommand(-120f);
                task = ExecuteAsChild(command);
                mTask = task;

                Wait(task);
                Wait(5000);

                command = StopShootingNPCProcess.CreateCommand();
                ExecuteAsChild(command);

                //needShhoting = false;
                //shooting = false;

#if UNITY_EDITOR
                Log("***********************************************************");
#endif

                //command = GoToPointNPCProcess.CreateCommand(nameOfWaypoint);
                //task = Execute(command);

#if UNITY_EDITOR
                Log($"task.State = {task.State}");
                Log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
#endif

                //Wait(30000);
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
