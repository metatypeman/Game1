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
        public void Main()
        {
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
}
