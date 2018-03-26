using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class StorageOfNPCProcessesTests
    {
        [Fact]
        public void AddTypeOfProcess_SetNull_GotArgumentNullException()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddTypeOfProcess_SetNull_OnDisposed_GotElementIsNotActiveException()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_GotTypeIsNotNPCProcessException()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_GotElementIsNotActiveException()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_GotTrue()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_GotTrueFirstlyAndFalseSecondly()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddTypeOfProcess_AddTwoDifferentTypesOfProcesesWithTheSameName_GotTrueFirstlyAndFalseSecondly()
        {
            throw new NotImplementedException();
        }
    }
}
