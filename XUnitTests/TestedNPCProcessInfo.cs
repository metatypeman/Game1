using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTests
{
    public class TestedNPCProcessInfoWithoutEntryPointsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
    }

    public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }
    }

    public class TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public void Main(int someArgument)
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
    }

    public class TestedNPCProcessInfoWithThreeEntryPointsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }
    }

    public class TestedNPCProcessInfoWithFourEntryPointsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        private void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        protected void Main(bool someArgument, int secondArgument)
        {
        }
    }

    public class TestedNPCProcessInfoWithFiveEntryPointsAndWithoutAttributesNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }

        public void Main(int someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess : BaseNPCProcess
    {
    }

    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }
    }

    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }
    }

    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithoutStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }
    }

    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithoutStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithoutStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }

        public void Main(int someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedNPCProcessInfoWithoutEntryPointsAndWithoutNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedNPCProcessInfoWithTwoEntryPointsAndWithoutNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedNPCProcessInfoThreeEntryPointsAndWithoutNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedNPCProcessInfoWithFourEntryPointsAndWithoutNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedNPCProcessInfoWithFiveEntryPointsAndWithoutNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }

        public void Main(int someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public bool IsCalledMain { get; private set; }

        public void Main()
        {
            IsCalledMain = true;
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main(int someArgument = 12)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main(int someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOnlyMethodWithOneArgumentAndWithTypeObjectWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main(object someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeStringAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main(string someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNpcProcessInfoWithSeveralEntryPointsWithOneArgumentAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main(object someArgument)
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(int? someArgument)
        {
        }

        public void Main(string someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOnlyMethodWithTwoArgumentsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public bool IsCalledMain_bool_int { get; private set; }
        public bool? BoolValue { get; private set; }
        public int? IntValue { get; private set; }

        public void Main(bool someArgument, int secondArgument)
        {
            IsCalledMain_bool_int = true;
            BoolValue = someArgument;
            IntValue = secondArgument;
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithStartupModeNPCProcess : BaseNPCProcess
    {
        public void Main()
        {
        }

        public void Main(int someArgument)
        {
        }

        public void Main(bool someArgument)
        {
        }

        public void Main(bool someArgument, int secondArgument)
        {
        }

        public void Main(int someArgument, int secondArgument)
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcessSingleton : BaseNPCProcess
    {
        public void Main()
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcessNewInstance : BaseNPCProcess
    {
        public void Main()
        {
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewStandaloneInstance)]
    [NPCProcessName("SomeName")]
    public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcessNewStandaloneInstance : BaseNPCProcess
    {
        public void Main()
        {
        }
    }
}
