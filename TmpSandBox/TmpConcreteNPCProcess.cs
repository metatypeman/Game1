using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TmpSandBox
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("test")]
    public class TmpConcreteNPCProcess: BaseNPCProcess
    {
        public TmpConcreteNPCProcess()
        {
            LogInstance.Log("Begin");
        }

        private void Main()
        {
            Log("Begin");
        }

        private void Main(int arg)
        {
            Log($"arg = {arg}");
        }

        public void Main(string arg)
        {
            Log($"arg = {arg}");
        }

        protected void Main(bool arg)
        {
            Log($"arg = {arg}");
        }
    }

    public class TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess()
        {
            Log("Begin");
        }

        public void Main()
        {
            Log("Begin");
        }

        public void Main(int someArgument, bool secondArgument = false)
        {
        }

        public void Main(int? someArgument)
        {
        }

        public void Main(object someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }

        public void Main(int someArgument, int secondArgument)
        {
        }

        public void Main(object someArgument, int secondArgument)
        {
        }

        public void Main(bool? someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewStandaloneInstance)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main(int someArgument = 12)
        {
            Log($"someArgument = {someArgument}");

            //Thread.Sleep(10000);

            Log($"End someArgument = {someArgument}");
        }
    }

    public class TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public void Main(int someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
    }
}
