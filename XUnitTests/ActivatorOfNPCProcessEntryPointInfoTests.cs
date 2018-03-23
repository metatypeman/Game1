using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class ActivatorOfNPCProcessEntryPointInfoTests
    {
        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsNull_GotArgumentNullException()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var e = Assert.Throws<ArgumentNullException>(() => {
                var rank = activator.GetRankByTypesOfParameters(null, typeof(int));
            });

            Assert.Equal("typeOfArgument", e.ParamName);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfParameterAsNull_GotArgumentNullException()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var e = Assert.Throws<ArgumentNullException>(() => {
                var rank = activator.GetRankByTypesOfParameters(typeof(int), null);
            });

            Assert.Equal("typeOfParameter", e.ParamName);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsInt32_PutTypeOfParameterAsInt32_GotOne()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(int));

            Assert.Equal(1f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsObject_PutTypeOfParameterAsObject_GotOne()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(object), typeof(object));

            Assert.Equal(1f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsString_PutTypeOfParameterAsString_GotOne()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(string), typeof(string));

            Assert.Equal(1f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsInt32_PutTypeOfParameterAsObject_GotZero()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(object));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsString_PutTypeOfParameterAsObject_GotZero()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(string), typeof(object));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsInt32_PutTypeOfParameterAsString_GotZero()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(string));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsObject_PutTypeOfParameterAsString_Got05()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(object), typeof(string));

            Assert.Equal(0.5f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsString_PutTypeOfParameterAsInt32_GotZero()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            var rank = activator.GetRankByTypesOfParameters(typeof(string), typeof(int));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoAsNull_GotArgumentNullException()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutCommandAsNull_GotArgumentNullException()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithEntryPointsListAsNull_GotArgumentNullException()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutCommandWithIndexedArgumentsDictionaryAsNull_GotArgumentNullException()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithoutAnyEntryPoints_AndPutCommand_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithoutArguments_AndPutCommandWithArguments_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithArguments_AndPutCommandWithoutArguments_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints__PutNpcProcessInfoWithMethodWithoutArguments_AndPutCommandWithoutArguments_GotListWithTargetEntryPointRankedAsOne()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgument_AndPutCommandWithoutArguments_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgument_AndPutCommandWithTwoArguments_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithTwoArguments_AndPutCommandWithoutArguments_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithTwoArguments_AndPutCommandWithOneArgument_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32_AndPutCommandWithOneArgumentWithTypeInt32_GotListWithTargetEntryPointRankedAsOne()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeObject_AndPutCommandWithOneArgumentWithTypeInt32_GotListWithTargetEntryPointRankedAs05()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeString_AndPutCommandWithOneArgumentWithTypeInt32_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32_AndPutCommandWithOneArgumentWithTypeObject__GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeObject_AndPutCommandWithOneArgumentWithTypeObject_GotListWithTargetEntryPointRankedAsOne()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeString_AndPutCommandWithOneArgumentWithTypeObject_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32_AndPutCommandWithOneArgumentWithTypeString_GotEmptyList()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeObject_AndPutCommandWithOneArgumentWithTypeString_GotListWithTargetEntryPointRankedAs05()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeString_AndPutCommandWithOneArgumentWithTypeString_GotListWithTargetEntryPointRankedAsOne()
        {
            var activator = new ActivatorOfNPCProcessEntryPointInfo();

            throw new NotImplementedException();
        }
    }
}
