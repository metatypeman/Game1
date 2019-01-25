using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("shooting with rotation")]
    public class ShootingWithRotationNPCProcess : CommonBaseNPCProcess
    {
        private static readonly string COMMAND_NAME = "shooting with rotation";
        private static readonly int DefaultCountOfIterations = 3;
        private static readonly float DefaultAngle = 10f;

        public static NPCCommand CreateCommand()
        {
            return CreateCommand(DefaultCountOfIterations, DefaultAngle);
        }

        public static NPCCommand CreateCommand(int? countOfIterations, float angle)
        {
            var command = new NPCCommand();
            command.Name = COMMAND_NAME;
            command.AddParam(nameof(countOfIterations), countOfIterations);
            command.AddParam(nameof(angle), angle);
            return command;
        }

        private void Main(int? countOfIterations, float angle)
        {
#if UNITY_EDITOR
            Log($"countOfIterations = {countOfIterations} angle = {angle}");
#endif

            var n = 0;

            var positiveDelta = angle * 2;
            var negativeDelta = -2 * angle;

            var command = ShootingNPCProcess.CreateCommand();
            var task = Execute(command);
            mShootingTask = task;

            command = RotateNPCProcess.CreateCommand(angle);
            task = ExecuteAsChild(command);
            mRotationTask = task;

            Wait(task);

            while (InfinityCondition)
            {
                n++;

                if(countOfIterations.HasValue && n > countOfIterations.Value)
                {
                    break;
                }

#if UNITY_EDITOR
                Log($"n = {n}");
#endif

                command = RotateNPCProcess.CreateCommand(negativeDelta);
                task = ExecuteAsChild(command);
                mRotationTask = task;

                Wait(task);

                command = RotateNPCProcess.CreateCommand(positiveDelta);
                task = ExecuteAsChild(command);
                mRotationTask = task;

                Wait(task);
            }

            command = RotateNPCProcess.CreateCommand(-1 * angle);
            task = ExecuteAsChild(command);
            mRotationTask = task;

            mShootingTask.Cancel();

#if UNITY_EDITOR
            Log("End");
#endif
        }

        private INPCProcess mRotationTask;
        private INPCProcess mShootingTask;

        protected override void CancelOfProcessChanged()
        {
#if UNITY_EDITOR
            Log($"Begin");
#endif
            mRotationTask?.Cancel();
            mShootingTask?.Cancel();

            base.CancelOfProcessChanged();
        }
    }
}
