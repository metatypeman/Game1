using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class BaseNPCContextTests
    {
        [Fact]
        public void CreateEmptyContext_GotCreatedState()
        {
            var testedContext = new TestedNPCContext();

            Assert.Equal(StateOfNPCContext.Created, testedContext.State);
        }

        [Fact]
        public void DisposeEmptyContext_GotDestroyedState()
        {
            var testedContext = new TestedNPCContext();

            testedContext.Dispose();

            Assert.Equal(StateOfNPCContext.Destroyed, testedContext.State);
        }

        [Fact]
        public void DisposeTwiceEmptyContext_GotDestroyedState()
        {
            var testedContext = new TestedNPCContext();

            testedContext.Dispose();

            Assert.Equal(StateOfNPCContext.Destroyed, testedContext.State);

            testedContext.Dispose();

            Assert.Equal(StateOfNPCContext.Destroyed, testedContext.State);
        }

        [Fact]
        public void AddTypeOfProcess_SetNull_GotArgumentNullException()
        {
            var testedContext = new TestedNPCContext();

            var e = Assert.Throws<ArgumentNullException>(() => {
                testedContext.AddTypeOfProcess(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void AddTypeOfProcess_SetNull_OnDisposed_GotElementIsNotActiveException()
        {
            var testedContext = new TestedNPCContext();

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.AddTypeOfProcess(null);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_GotTypeIsNotNPCProcessException()
        {
            var testedContext = new TestedNPCContext();

            var type = GetType();

            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                testedContext.AddTypeOfProcess(type);
            });

            Assert.Equal(type, e.WrongType);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_ByGeneric_GotTypeIsNotNPCProcessException()
        {
            var testedContext = new TestedNPCContext();

            var type = typeof(BaseNPCContextTests);

            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                testedContext.AddTypeOfProcess<BaseNPCContextTests>();
            });

            Assert.Equal(type, e.WrongType);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_GotElementIsNotActiveException()
        {
            var testedContext = new TestedNPCContext();

            var type = GetType();

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.AddTypeOfProcess(type);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_ByGeneric_GotElementIsNotActiveException()
        {
            var testedContext = new TestedNPCContext();

            var type = typeof(BaseNPCContextTests);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.AddTypeOfProcess<BaseNPCContextTests>();
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_GotTrue()
        {
            var testedContext = new TestedNPCContext();

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = testedContext.AddTypeOfProcess(type);

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_ByGeneric_GotTrue()
        {
            var testedContext = new TestedNPCContext();

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess>();

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_GotTrueFirstlyAndFalseSecondly()
        {
            var testedContext = new TestedNPCContext();

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = testedContext.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var result_2 = testedContext.AddTypeOfProcess(type);

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_ByGeneric_GotTrueFirstlyAndFalseSecondly()
        {
            var testedContext = new TestedNPCContext();

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess>();

            Assert.Equal(true, result);

            var result_2 = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess>();

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddTwoDifferentTypesOfProcesesWithTheSameName_GotTrueFirstlyAndFalseSecondly()
        {
            var testedContext = new TestedNPCContext();

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            var result = testedContext.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var type_2 = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            var result_2 = testedContext.AddTypeOfProcess(type_2);

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddTwoDifferentTypesOfProcesesWithTheSameName_ByGeneric_GotTrueFirstlyAndFalseSecondly()
        {
            var testedContext = new TestedNPCContext();

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(true, result);

            var result_2 = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWithoutEntryPoints_GotTrue()
        {
            var testedContext = new TestedNPCContext();

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            var result = testedContext.AddTypeOfProcess(type);

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWithoutEntryPoints_ByGeneric_GotTrue()
        {
            var testedContext = new TestedNPCContext();

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess>();

            Assert.Equal(true, result);
        }

        [Fact]
        public void Bootstrap_ByEmpty_GotWorkingNPCContext()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Bootstrap_ByEmpty_OnDisposed_

        [Fact]
        public void Bootstrap_ByNull_GotWorkingNPCContext()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Bootstrap_ByNull_OnDisposed_
        [Fact]
        public void Bootstrap_ByNotRegisteredType_
        [Fact]
        public void Bootstrap_ByNotRegisteredType_OnDisposed_
        [Fact]
        public void Bootstrap_ByTypeWhatIsNotBasedOnBaseNPCProcess_
        [Fact]
        public void Bootstrap_ByTypeWhatIsNotBasedOnBaseNPCProcess_OnDisposed_
        [Fact]
        public void Bootstrap_ByTypeWithoutEntryPoints_
        [Fact]
        public void Bootstrap_ByTypeWithoutEntryPoints_OnDisposed_
        [Fact]
        public void Bootstrap_
        [Fact]
        public void Bootstrap_
    }
}
