using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using XUnitTests.Helpers;

namespace XUnitTests
{
    public class ActivatorOfNPCProcessEntryPointInfoTests
    {
        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var e = Assert.Throws<ArgumentNullException>(() => {
                var rank = activator.GetRankByTypesOfParameters(null, typeof(int));
            });

            Assert.Equal("typeOfArgument", e.ParamName);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsInt32_PutTypeOfParameterAsNull_GotZero()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(int), null);

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsNullableInt32_PutTypeOfParameterAsNull_Got05()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(int?), null);

            Assert.Equal(0.5f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsObject_PutTypeOfParameterAsNull_Got05()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(object), null);

            Assert.Equal(0.5f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsString_PutTypeOfParameterAsNull_Got05()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(string), null);

            Assert.Equal(0.5f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsInt32_PutTypeOfParameterAsInt32_GotOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(int));

            Assert.Equal(1f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsObject_PutTypeOfParameterAsObject_GotOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(object), typeof(object));

            Assert.Equal(1f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsString_PutTypeOfParameterAsString_GotOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(string), typeof(string));

            Assert.Equal(1f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsInt32_PutTypeOfParameterAsObject_GotZero()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(object));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsString_PutTypeOfParameterAsObject_GotZero()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(string), typeof(object));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsInt32_PutTypeOfParameterAsString_GotZero()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(string));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsObject_PutTypeOfParameterAsString_Got05()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(object), typeof(string));

            Assert.Equal(0.5f, rank);
        }

        [Fact]
        public void GetRankByTypesOfParameters_PutTypeOfArgumentAsString_PutTypeOfParameterAsInt32_GotZero()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var rank = activator.GetRankByTypesOfParameters(typeof(string), typeof(int));

            Assert.Equal(0f, rank);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoAsNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var e = Assert.Throws<ArgumentNullException>(() => {
                var result = activator.GetRankedEntryPoints(null, new Dictionary<ulong, object>());
            });

            Assert.Equal("npcProcessInfo", e.ParamName);
        }

        [Fact]
        public void GetRankedEntryPoints_PutCommandAsNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var e = Assert.Throws<ArgumentNullException>(() => {
                var result = activator.GetRankedEntryPoints(new NPCProcessInfo(), null);
            });

            Assert.Equal("paramsOfCommand", e.ParamName);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithEntryPointsListAsNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var npcProcessInfo = new NPCProcessInfo();
            npcProcessInfo.EntryPointsInfoList = null;

            var e = Assert.Throws<ArgumentNullException>(() => {
                var result = activator.GetRankedEntryPoints(npcProcessInfo, new Dictionary<ulong, object>());
            });

            Assert.Equal("npcProcessInfo.EntryPointsInfoList", e.ParamName);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithoutAnyEntryPoints_AndPutCommand_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = activator.GetRankedEntryPoints(npcProcessInfo, new Dictionary<ulong, object>());

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithoutArguments_AndPutCommandWithArguments_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var paramsDict = new Dictionary<ulong, object>() { { 1ul, true } };
            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithArguments_AndPutCommandWithoutArguments_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = activator.GetRankedEntryPoints(npcProcessInfo, new Dictionary<ulong, object>());

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints__PutNpcProcessInfoWithMethodWithoutArguments_AndPutCommandWithoutArguments_GotListWithTargetEntryPointRankedAsOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = activator.GetRankedEntryPoints(npcProcessInfo, new Dictionary<ulong, object>());

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            var targetItem = result.Single();

            Assert.Equal(1f, targetItem.Rank);

            Assert.NotEqual(null, targetItem.EntryPoint);

            NPCProcessInfoFactoryTests.CommonAssertsForEntryPoint(targetItem.EntryPoint);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgument_AndPutCommandWithoutArguments_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = activator.GetRankedEntryPoints(npcProcessInfo, new Dictionary<ulong, object>());

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgument_AndPutCommandWithTwoArguments_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var paramsDict = new Dictionary<ulong, object>() { { 1ul, true }, { 2ul, 12 } };
            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithTwoArguments_AndPutCommandWithoutArguments_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithTwoArgumentsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = activator.GetRankedEntryPoints(npcProcessInfo, new Dictionary<ulong, object>());

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithTwoArguments_AndPutCommandWithOneArgument_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithTwoArgumentsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var paramsDict = new Dictionary<ulong, object>() { { 1ul, true }};

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32_AndPutCommandWithOneArgumentWithTypeInt32_GotListWithTargetEntryPointRankedAsOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, 12 } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            var targetItem = result.Single();

            Assert.Equal(1f, targetItem.Rank);

            Assert.NotEqual(null, targetItem.EntryPoint);

            NPCProcessInfoFactoryTests.CommonAssertsForMain_int(targetItem.EntryPoint, globalEntityDictionary);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeObject_AndPutCommandWithOneArgumentWithTypeInt32_GotListWithTargetEntryPointRankedAs05()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentAndWithTypeObjectWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, 12 } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            var targetItem = result.Single();

            Assert.Equal(0.5f, targetItem.Rank);

            Assert.NotEqual(null, targetItem.EntryPoint);

            NPCProcessInfoFactoryTests.CommonAssertsForMain_object(targetItem.EntryPoint, globalEntityDictionary);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeString_AndPutCommandWithOneArgumentWithTypeInt32_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeStringAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, 12 } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32_AndPutCommandWithOneArgumentWithTypeObject_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, new object() } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeObject_AndPutCommandWithOneArgumentWithTypeObject_GotListWithTargetEntryPointRankedAsOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentAndWithTypeObjectWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, new object() } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            var targetItem = result.Single();

            Assert.Equal(1f, targetItem.Rank);

            Assert.NotEqual(null, targetItem.EntryPoint);

            NPCProcessInfoFactoryTests.CommonAssertsForMain_object(targetItem.EntryPoint, globalEntityDictionary);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeString_AndPutCommandWithOneArgumentWithTypeObject_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeStringAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, new object() } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32_AndPutCommandWithOneArgumentWithTypeString_GotEmptyList()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeInt32AndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, "qqq" } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeObject_AndPutCommandWithOneArgumentWithTypeString_GotListWithTargetEntryPointRankedAs05()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentAndWithTypeObjectWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, "qqq" } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            var targetItem = result.Single();

            Assert.Equal(0.5f, targetItem.Rank);

            Assert.NotEqual(null, targetItem.EntryPoint);

            NPCProcessInfoFactoryTests.CommonAssertsForMain_object(targetItem.EntryPoint, globalEntityDictionary);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOnlyMethodWithOneArgumentWithTypeString_AndPutCommandWithOneArgumentWithTypeString_GotListWithTargetEntryPointRankedAsOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithOnlyMethodWithOneArgumentWithTypeStringAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, "qqq" } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            var targetItem = result.Single();

            Assert.Equal(1f, targetItem.Rank);

            Assert.NotEqual(null, targetItem.EntryPoint);

            NPCProcessInfoFactoryTests.CommonAssertsForMain_string(targetItem.EntryPoint, globalEntityDictionary);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithSeveralEntryPoints_AndPutCommandWithOneArgumentWithTypeInt32_GotListWithThreeTargetEntryPointsRankedAsOneAnd05()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNpcProcessInfoWithSeveralEntryPointsWithOneArgumentAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var keyOfArgument = globalEntityDictionary.GetKey("someArgument");

            var paramsDict = new Dictionary<ulong, object>() { { keyOfArgument, 12 } };

            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            Assert.NotEqual(null, result);
            Assert.Equal(3, result.Count);

            var intEntryPoint = result.Single(p => p.Rank == 1f);

            NPCProcessInfoFactoryTests.CommonAssertsForMain_int(intEntryPoint.EntryPoint, globalEntityDictionary);

            var objectEntryPoint = result.Single(p => p.Rank == 0.5f && p.EntryPoint.ParametersMap.ContainsValue(typeof(object)));

            NPCProcessInfoFactoryTests.CommonAssertsForMain_object(objectEntryPoint.EntryPoint, globalEntityDictionary);

            var nullableIntEntryPoint = result.Single(p => p.Rank == 0.5f && p.EntryPoint.ParametersMap.ContainsValue(typeof(int?)));

            NPCProcessInfoFactoryTests.CommonAssertsForMain_NullableInt(nullableIntEntryPoint.EntryPoint, globalEntityDictionary);
        }

        [Fact]
        public void GetRankedEntryPoints_PutNpcProcessInfoWithOneEntryPointWithDefaultValue_AndPutCommandWithoutArguments_GotListWithTargetEntryPointRankedAsOne()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var type = typeof(TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = activator.GetRankedEntryPoints(npcProcessInfo, new Dictionary<ulong, object>());

            Assert.NotEqual(null, result);
            Assert.Equal(1, result.Count);

            var targetItem = result.Single();

            Assert.Equal(1f, targetItem.Rank);

            Assert.NotEqual(null, targetItem.EntryPoint);

            NPCProcessInfoFactoryTests.CommonAssertsForEntryPoint(targetItem.EntryPoint);
        }

        [Fact]
        public void CallEntryPoint_SetNPCProcessAsNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var e = Assert.Throws<ArgumentNullException>(() => {
                activator.CallEntryPoint(null, new NPCProcessEntryPointInfo(), new Dictionary<ulong, object>());
            });

            Assert.Equal("npcProcess", e.ParamName);
        }

        [Fact]
        public void CallEntryPoint_SetEntryPointInfoAsNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var instance = new TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcess();

            var e = Assert.Throws<ArgumentNullException>(() => {
                activator.CallEntryPoint(instance, null, new Dictionary<ulong, object>());
            });

            Assert.Equal("entryPoint", e.ParamName);
        }

        [Fact]
        public void CallEntryPoint_SetNPCInternalCommandAsNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var instance = new TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcess();

            var e = Assert.Throws<ArgumentNullException>(() => {
                activator.CallEntryPoint(instance, new NPCProcessEntryPointInfo(), null);
            });

            Assert.Equal("paramsOfCommand", e.ParamName);
        }

        [Fact]
        public void CallEntryPoint_ByEntryPointWithoutArguments_GotNothing()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var instance = new TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcess();
            var type = instance.GetType();

            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var command = new NPCCommand();
            command.Name = "SomeName";

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var targetEntryPoints = activator.GetRankedEntryPoints(npcProcessInfo, internalCommand.Params);

            var targetEntryPoint = targetEntryPoints.First();

            Assert.Equal(false, instance.IsCalledMain);

            activator.CallEntryPoint(instance, targetEntryPoint.EntryPoint, internalCommand.Params);

            Assert.Equal(true, instance.IsCalledMain);
        }

        [Fact]
        public void CallEntryPoint_ByEntryPointWithArgument_GotNothing()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);

            var instance = new TestedNPCProcessInfoWithOnlyMethodWithTwoArgumentsAndWithNameAndWithStartupModeNPCProcess();
            var type = instance.GetType();

            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var command = new NPCCommand
            {
                Name = "SomeName"
            };
            command.Params.Add("someArgument", true);
            command.Params.Add("secondArgument", 12);

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var targetEntryPoints = activator.GetRankedEntryPoints(npcProcessInfo, internalCommand.Params);

            var targetEntryPoint = targetEntryPoints.First();

            Assert.Equal(false, instance.IsCalledMain_bool_int);
            Assert.Equal(null, instance.BoolValue);
            Assert.Equal(null, instance.IntValue);

            activator.CallEntryPoint(instance, targetEntryPoint.EntryPoint, internalCommand.Params);

            Assert.Equal(true, instance.IsCalledMain_bool_int);
            Assert.Equal(true, instance.BoolValue);
            Assert.Equal(12, instance.IntValue);
        }
    }
}
