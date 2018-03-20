using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("test")]
    public class TmpConcreteNPCProcess: BaseNPCProcess
    {
        public TmpConcreteNPCProcess()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("TmpConcreteNPCProcess");
        }

        private void Main()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Main");
        }

        private void Main(int arg)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Main arg = {arg}");
        }

        public void Main(string arg)
        {

        }

        protected void Main(bool arg)
        {

        }
    }

    public class TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }

        public void Main(int someArgument, int secondArgument)
        {
        }
    }
}
