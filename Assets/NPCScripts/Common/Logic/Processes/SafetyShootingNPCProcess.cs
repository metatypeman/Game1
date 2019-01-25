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
    [NPCProcessName("safety shooting")]
    public class SafetyShootingNPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand(BaseAbstractLogicalObject target, List<BaseAbstractLogicalObject> exceptEntities)
        {
            var command = new NPCCommand();
            command.Name = "safety shooting";
            command.AddParam(nameof(target), target);
            command.AddParam(nameof(exceptEntities), exceptEntities);
            return command;
        }

        private void Main(BaseAbstractLogicalObject target, List<BaseAbstractLogicalObject> exceptEntities)
        {
#if UNITY_EDITOR
            Log($"target = {target}");
            Log($"exceptEntities.Count = {exceptEntities.Count}");
#endif

            var targetPosition = target.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Log($"targetPosition = {targetPosition}");
#endif

            if (!targetPosition.HasValue)
            {
                State = StateOfNPCProcess.Faulted;
                return;
            }

            var isDied = target.GetValue<bool?>("died");

#if UNITY_EDITOR
            Log($"isDied = {isDied}");
#endif

            if(isDied == true)
            {
                State = StateOfNPCProcess.Faulted;
                return;
            }

            if (exceptEntities.Count > 0)
            {
                if (Context.VisibleObjects.Any(p => exceptEntities.Contains(p)))
                {
#if UNITY_EDITOR
                    Log("Context.VisibleObjects.Any(p => exceptEntities.Contains(p)) !!!!!!!!!!");
#endif

                    State = StateOfNPCProcess.Faulted;
                    return;
                }
            }

            BlackBoard.IsShooting = true;

            var moveCommand = new HumanoidHStateCommand();
            moveCommand.State = HumanoidHState.Stop;

            ExecuteBody(moveCommand);

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.AimAt;
            tmpCommand.TargetPosition = targetPosition;

            var tmpTask = ExecuteBody(tmpCommand);

            Wait(tmpTask);

            var command = ShootingWithRotationNPCProcess.CreateCommand();
            var task = Execute(command);

            Wait(task);

            BlackBoard.IsShooting = false;

#if UNITY_EDITOR
            Log($"task.State (2) = {task.State}");
#endif
        }
    }
}
