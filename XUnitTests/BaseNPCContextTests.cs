using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTests.Helpers;

namespace XUnitTests
{
    public class BaseNPCContextTests
    {
        [Fact]
        public void CreateEmptyContext_GotCreatedState()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            Assert.Equal(StateOfNPCContext.Created, testedContext.State);
        }

        [Fact]
        public void DisposeEmptyContext_GotDestroyedState()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Equal(StateOfNPCContext.Destroyed, testedContext.State);
        }

        [Fact]
        public void DisposeTwiceEmptyContext_GotDestroyedState()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Equal(StateOfNPCContext.Destroyed, testedContext.State);

            testedContext.Dispose();

            Assert.Equal(StateOfNPCContext.Destroyed, testedContext.State);
        }

        [Fact]
        public void AddTypeOfProcess_SetNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var e = Assert.Throws<ArgumentNullException>(() => {
                testedContext.AddTypeOfProcess(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void AddTypeOfProcess_SetNull_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.AddTypeOfProcess(null);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_GotTypeIsNotNPCProcessException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = GetType();

            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                testedContext.AddTypeOfProcess(type);
            });

            Assert.Equal(type, e.WrongType);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_ByGeneric_GotTypeIsNotNPCProcessException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(BaseNPCContextTests);

            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                testedContext.AddTypeOfProcess<BaseNPCContextTests>();
            });

            Assert.Equal(type, e.WrongType);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = GetType();

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.AddTypeOfProcess(type);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_ByGeneric_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(BaseNPCContextTests);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.AddTypeOfProcess<BaseNPCContextTests>();
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = testedContext.AddTypeOfProcess(type);

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_ByGeneric_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess>();

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_GotTrueFirstlyAndFalseSecondly()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = testedContext.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var result_2 = testedContext.AddTypeOfProcess(type);

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_ByGeneric_GotTrueFirstlyAndFalseSecondly()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess>();

            Assert.Equal(true, result);

            var result_2 = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess>();

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddTwoDifferentTypesOfProcesesWithTheSameName_GotTrueFirstlyAndFalseSecondly()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

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
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(true, result);

            var result_2 = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWithoutEntryPoints_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            var result = testedContext.AddTypeOfProcess(type);

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWithoutEntryPoints_ByGeneric_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var result = testedContext.AddTypeOfProcess<TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess>();

            Assert.Equal(true, result);
        }

        [Fact]
        public void Bootstrap_ByEmpty_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByEmpty_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap();
            });
        }

        [Fact]
        public void Bootstrap_ByNull_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap(null);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByNull_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap(null);
            });
        }

        [Fact]
        public void Bootstrap_ByNotRegisteredType_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            testedContext.Bootstrap(type);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByNotRegisteredType_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByNotRegisteredType_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap(type);
            });
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByNotRegisteredType_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();
            });
        }

        [Fact]
        public void Bootstrap_ByTypeWhatIsNotBasedOnBaseNPCProcess_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(BaseNPCContextTests);

            testedContext.Bootstrap(type);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeWhatIsNotBasedOnBaseNPCProcess_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap<BaseNPCContextTests>();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByTypeWhatIsNotBasedOnBaseNPCProcess_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(BaseNPCContextTests);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap(type);
            });
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeWhatIsNotBasedOnBaseNPCProcess_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap<BaseNPCContextTests>();
            });
        }

        [Fact]
        public void Bootstrap_ByTypeWithoutEntryPoints_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            testedContext.Bootstrap(type);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeWithoutEntryPoints_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap<TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess>();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByTypeWithoutEntryPoints_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap(type);
            });
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeWithoutEntryPoints_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap<TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess>();
            });
        }

        [Fact]
        public void Bootstrap_ByTypeWithEntryPointWithoutArguments_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            testedContext.Bootstrap(type);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeWithEntryPointWithoutArguments_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByTypeWithEntryPointWithoutArguments_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap(type);
            });
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeWithEntryPointWithoutArguments_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();
            });
        }

        [Fact]
        public void Bootstrap_ByTypeOnlyWithEntryPointWithArgument_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess);

            testedContext.Bootstrap(type);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeOnlyWithEntryPointWithArgument_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap<TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess>();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void Bootstrap_ByTypeOnlyWithEntryPointWithArgument_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap(type);
            });
        }

        [Fact]
        public void Bootstrap_ByGeneric_ByTypeOnlyWithEntryPointWithArgument_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                testedContext.Bootstrap<TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess>();
            });
        }

        [Fact]
        public void BootstrapTwice_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            testedContext.Bootstrap(type);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);

            testedContext.Bootstrap(type);

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }

        [Fact]
        public void BootstrapTwice_ByGeneric_GotWorkingNPCContext()
        {
            var entityLogger = new EmptyEntityLogger();
            var testedContext = new TestedNPCContext(entityLogger);

            testedContext.Bootstrap<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);

            testedContext.Bootstrap<TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess>();

            Assert.Equal(StateOfNPCContext.Working, testedContext.State);
        }
    }
}
