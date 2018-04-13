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
            NLog.LogManager.GetCurrentClassLogger().Info("Begin Awake");

            NLog.LogManager.GetCurrentClassLogger().Info($"Awake BlackBoard.TstValue = {BlackBoard.TstValue}");

            var trigger = CreateTrigger(() => {
                if (BlackBoard.TstValue == 12)
                {
                    return true;
                }

                return false;
            });

            trigger.OnFire += Trigger_OnFire;
            trigger.OnResetCondition += Trigger_OnResetCondition;

            NLog.LogManager.GetCurrentClassLogger().Info("End Awake");
        }
        private void Trigger_OnFire()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Trigger_OnFire");

            Thread.Sleep(1000);

            BlackBoard.TstValue = 15;
        }

        private void Trigger_OnResetCondition()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Trigger_OnResetCondition");
        }

        private void Main()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin Main");

            NLog.LogManager.GetCurrentClassLogger().Info($"Main BlackBoard.TstValue = {BlackBoard.TstValue}");

            BlackBoard.TstValue = 12;

            var command = new NPCCommand();
            command.Name = "go to far waypoint";

            var childProcess = ExecuteAsChild(command);

            Wait(childProcess);

            NLog.LogManager.GetCurrentClassLogger().Info("Main End Wait(childProcess)");

            NLog.LogManager.GetCurrentClassLogger().Info("End Main");
        }
    }
}
