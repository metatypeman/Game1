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
            Log("Begin");
            Log($"BlackBoard.TstValue = {BlackBoard.TstValue}");

            var currnetProcessId = Id;

            Log($"currnetProcessId = {currnetProcessId}");

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

            Log($"tmpResult = {tmpResult}");

            tmpResult = GetDefaultHandProperty("FireMode");

            Log($"tmpResult (2) = {tmpResult}");

            tmpResult = GetDefaultHandProperty<object>("FireMode");

            Log($"tmpResult (3) = {tmpResult}");

            var tmpStrRes = GetDefaultHandProperty<string>("FireMode");

            Log($"tmpStrRes = {tmpStrRes}");

            Log("End");
        }

        private void Process_OnStateChanged(INPCProcess sender, StateOfNPCProcess state)
        {
            Log($"sender.Id = {sender.Id} state = {state}");
        }
    }
}
