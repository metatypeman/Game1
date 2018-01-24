using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class NPCMeshTask : IObjectToString
    {
        public int TaskId { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(NPCMeshTask)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(NPCMeshTask)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(TaskId)} = {TaskId}");
            return sb.ToString();
        }
    }

    public class NPCThreadSafeMeshController
    {
        public NPCThreadSafeMeshController(IMoveHumanoidController movehumanoidController)
        {
            mMoveHumanoidController = movehumanoidController;
        }

        private IMoveHumanoidController mMoveHumanoidController;

        public NPCMeshTask Execute(IMoveHumanoidCommand command)
        {
            var commandsPackage = new MoveHumanoidCommandsPackage();
            commandsPackage.TaskId = command.TaskId;
            commandsPackage.Commands.Add(command);
            return Execute(commandsPackage);
        }

        public NPCMeshTask Execute(IMoveHumanoidCommandsPackage package)
        {
            var result = new NPCMeshTask();
            result.TaskId = package.TaskId;

#if UNITY_EDITOR
            Debug.Log($"NPCThreadSafeMeshController Execute package = {package}");
#endif

            mMoveHumanoidController.ExecuteAsync(package);

            return result;
        }
    }
}
