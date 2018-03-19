using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTests
{
    public class NPCProcessInfoFactoryTests
    {
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithoutAttributes()
        {
        }

        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributes
        {
            public void Main()
            {
            }
        }

        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributes
        {
            public void Main()
            {
            }

            public void Main(int someArgument)
            {
            }
        }

        public void CreateTestedNPCProcessInfoWithThreeEntryPointsAndWithoutAttributes
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

        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithoutAttributes : BaseNPCProcess
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

        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithoutAttributes : BaseNPCProcess
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

        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
        {
        }

        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }
        }

        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }

            public void Main(int someArgument)
            {
            }
        }

        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
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

        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
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

        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithoutStartupMode : BaseNPCProcess
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

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
        {
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutNameAndWithStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }

            public void Main(int someArgument)
            {
            }
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public void CreateTestedNPCProcessInfoThreeEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
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

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
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

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithoutNameAndWithStartupMode : BaseNPCProcess
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

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
            public void Main()
            {
            }

            public void Main(int someArgument)
            {
            }
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
        }

        //[NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
        //[NPCProcessName("SomeName")]
        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithStartupMode : BaseNPCProcess
        {
        }
    }
}
