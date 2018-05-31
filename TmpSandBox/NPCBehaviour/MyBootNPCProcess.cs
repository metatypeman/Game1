using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TmpSandBox.NPCBehaviour
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class MyBootNPCProcess: BaseNPCProcessWithBlackBoard<MyBlackBoard>
    {
        protected override void Awake()
        {
            Log("Begin");

            Log($"BlackBoard.TstValue = {BlackBoard.TstValue}");

            var trigger = CreateTrigger(() => {
                if (BlackBoard.TstValue == 12)
                {
                    return true;
                }

                return false;
            });

            trigger.OnFire += Trigger_OnFire;
            trigger.OnResetCondition += Trigger_OnResetCondition;

            Log("End");
        }

        private void Trigger_OnFire()
        {
            Log("Begin");

            Thread.Sleep(1000);

            BlackBoard.TstValue = 15;
        }

        private void Trigger_OnResetCondition()
        {
            Log("Begin");
        }

        private void Main()
        {
            Log("Begin");
            Log($"Id = {Id}");

            var currnetProcessId = Id;

            Log($"currnetProcessId = {currnetProcessId}");

            Log($"BlackBoard.TstValue = {BlackBoard.TstValue}");

            BlackBoard.TstValue = 12;

            var command = new NPCCommand();
            command.Name = "go to far waypoint";
            command.KindOfLinkingToInitiator = KindOfLinkingToInitiator.Child;

            var childProcess = ExecuteAsChild(command);

            childProcess.OnStateChanged += ChildProcess_OnStateChanged;

            Wait(childProcess);

            Log("End Wait(childProcess)");

            Log("End");
        }

        private void ChildProcess_OnStateChanged(INPCProcess sender, StateOfNPCProcess state)
        {
            Log($"sender.Id = {sender.Id} state = {state}");
        }
    }
}
