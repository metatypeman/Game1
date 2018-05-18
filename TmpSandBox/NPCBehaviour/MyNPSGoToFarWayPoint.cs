using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewStandaloneInstance)]
    [NPCProcessName("go to far waypoint")]
    public class MyNPSGoToFarWayPoint : BaseNPCProcessWithBlackBoard<MyBlackBoard>
    {
        private void Main()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin Main");
            NLog.LogManager.GetCurrentClassLogger().Info($"Main BlackBoard.TstValue = {BlackBoard.TstValue}");

            var currnetProcessId = Id;

            NLog.LogManager.GetCurrentClassLogger().Info($"Main currnetProcessId = {currnetProcessId}");

            var bodyCommand = new HumanoidHStateCommand();
            bodyCommand.State = HumanoidHState.Run;

            var process = ExecuteBody(bodyCommand);

            Wait(process);

            var handCommand = new NPCCommand();
            handCommand.Name = "fire";

            process = ExecuteDefaultHand(handCommand);

            process.OnStateChanged += Process_OnStateChanged;

            Wait(process);

            TryAsCancel();

            var tmpResult = Context.DefaultHand.Get("FireMode");

            NLog.LogManager.GetCurrentClassLogger().Info($"Main tmpResult = {tmpResult}");

            tmpResult = GetDefaultHandProperty("FireMode");

            NLog.LogManager.GetCurrentClassLogger().Info($"Main tmpResult (2) = {tmpResult}");

            tmpResult = GetDefaultHandProperty<object>("FireMode");

            NLog.LogManager.GetCurrentClassLogger().Info($"Main tmpResult (3) = {tmpResult}");

            var tmpStrRes = GetDefaultHandProperty<string>("FireMode");

            NLog.LogManager.GetCurrentClassLogger().Info($"Main tmpStrRes = {tmpStrRes}");

            NLog.LogManager.GetCurrentClassLogger().Info("End Main");
        }

        private void Process_OnStateChanged(INPCProcess sender, StateOfNPCProcess state)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Process_OnStateChanged sender.Id = {sender.Id} state = {state}");
        }
    }
}
