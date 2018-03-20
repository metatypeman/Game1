using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTests
{
    public class NPCProcessInfoFactoryTests
    {
        /*public class TestedNPCProcessInfoWithoutEntryPointsAndWithoutAttributes : BaseNPCProcess
        {
        }*/

        /*public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributes : BaseNPCProcess
        {
            public void Main()
            {
            }
        }*/

        /*public class TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributes : BaseNPCProcess
        {
            public void Main()
            {
            }

            public void Main(int someArgument)
            {
            }
        }*/

        public class TestedNPCProcessInfoWithThreeEntryPointsAndWithoutAttributes : BaseNPCProcess
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

        public class TestedNPCProcessInfoWithFourEntryPointsAndWithoutAttributes : BaseNPCProcess
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

        public class TestedNPCProcessInfoWithFiveEntryPointsAndWithoutAttributes : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
        {
        }

        [NPCProcessName("SomeName")]
        public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }
        }

        [NPCProcessName("SomeName")]
        public class TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }

            public void Main(int someArgument)
            {
            }
        }

        [NPCProcessName("SomeName")]
        public class TestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithoutEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
        {
        }

        [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutNameAndWithStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }
        }

        [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public class TestedNPCProcessInfoWithTwoEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }

            public void Main(int someArgument)
            {
            }
        }

        [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public class TestedNPCProcessInfoThreeEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithFourEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithFiveEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
        }

        [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        [NPCProcessName("SomeName")]
        public class TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }
        }

        [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        [NPCProcessName("SomeName")]
        public class TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
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
        public class TestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
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
