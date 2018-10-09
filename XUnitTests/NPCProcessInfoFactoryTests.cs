using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using XUnitTests.Helpers;

namespace XUnitTests
{
    public class NPCProcessInfoFactoryTests
    {
        [Fact]
        public void TryCreateByNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var e = Assert.Throws<ArgumentNullException>(() => {
                var npcProcessInfo = npcProcessInfoFactory.CreateInfo(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void TryCreateByTypeWhatIsNotBasedOnBaseNPCProcess_GotTypeIsNotNPCProcessException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(NPCProcessInfoFactoryTests);
            
            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);
            });

            Assert.Equal(type, e.WrongType);
        }

        private static void CommonAssertsForEachCreatedNPCProcessInfo(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary, Type type)
        {
            Assert.NotEqual(null, npcProcessInfo);
            Assert.NotEqual(null, npcProcessInfo.Name);
            Assert.NotEqual(string.Empty, npcProcessInfo.Name.Trim());
            Assert.NotEqual(0ul, npcProcessInfo.Key);
            Assert.Equal(npcProcessInfo.Key, entityDictionary.GetKey(npcProcessInfo.Name));

            Assert.NotEqual(null, npcProcessInfo.Type);
            Assert.NotEqual(null, npcProcessInfo.TypeInfo);
            Assert.NotEqual(typeof(NPCProcessInfo), npcProcessInfo.Type);
            Assert.Equal(npcProcessInfo.Type.FullName, npcProcessInfo.TypeInfo.FullName);
            Assert.Equal(type, npcProcessInfo.Type);

            Assert.NotEqual(null, npcProcessInfo.EntryPointsInfoList);
        }

        private void CommonAssertsForEachWithoutEntryPointsAndWithoutAttributesNPCProcess(NPCProcessInfo npcProcessInfo, Type type)
        {
            Assert.Equal(type.FullName, npcProcessInfo.Name);
            Assert.Equal(NPCProcessStartupMode.NewInstance, npcProcessInfo.StartupMode);
        }

        private static void CommonAssertsForEachWithoutEntryPoints(NPCProcessInfo npcProcessInfo)
        {
            Assert.Equal(0, npcProcessInfo.EntryPointsInfoList.Count);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithoutAttributesNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutEntryPointsAndWithoutAttributesNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithoutEntryPoints(npcProcessInfo);
        }

        private static void CommonAssertsForEachWithOneEntryPointWithoutArgs(NPCProcessInfo npcProcessInfo)
        {
            Assert.Equal(1, npcProcessInfo.EntryPointsInfoList.Count);

            CommonAssertsForMainWithoutArgs(npcProcessInfo.EntryPointsInfoList);
        }

        private static void CommonAssertsForMainWithoutArgs(List<NPCProcessEntryPointInfo> entryPointsInfoList)
        {
            var npcProcessInfoEntryPoint = entryPointsInfoList.Single(p => p.ParametersMap.Count == 0);

            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        public static void CommonAssertsForEntryPoint(NPCProcessEntryPointInfo npcProcessInfoEntryPoint)
        {
            Assert.NotEqual(null, npcProcessInfoEntryPoint.MethodInfo);
            Assert.Equal("Main", npcProcessInfoEntryPoint.MethodInfo.Name, true);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);

            Assert.Equal(npcProcessInfoEntryPoint.ParametersMap.Count, npcProcessInfoEntryPoint.IndexedParametersMap.Count);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutEntryPointsAndWithoutAttributesNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithOneEntryPointWithoutArgs(npcProcessInfo);
        }

        private void CommonAssertsForEachWithTwoEntryPoints(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary)
        {
            Assert.Equal(2, npcProcessInfo.EntryPointsInfoList.Count);

            CommonAssertsForMainWithoutArgs(npcProcessInfo.EntryPointsInfoList);
            CommonAssertsForMain_int(npcProcessInfo.EntryPointsInfoList, entityDictionary);
        }

        private void CommonAssertsForMain_int(List<NPCProcessEntryPointInfo> entryPointsInfoList, IEntityDictionary entityDictionary)
        {
            var npcProcessInfoEntryPoint = entryPointsInfoList.Single(p => p.ParametersMap.Count == 1 && p.ParametersMap.ContainsValue(typeof(int)));

            CommonAssertsForMain_int(npcProcessInfoEntryPoint, entityDictionary);
        }

        public static void CommonAssertsForMain_int(NPCProcessEntryPointInfo npcProcessInfoEntryPoint, IEntityDictionary entityDictionary)
        {
            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(int), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(int), indexedSomeArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        public static void CommonAssertsForMain_object(NPCProcessEntryPointInfo npcProcessInfoEntryPoint, IEntityDictionary entityDictionary)
        {
            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(object), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(object), indexedSomeArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        public static void CommonAssertsForMain_string(NPCProcessEntryPointInfo npcProcessInfoEntryPoint, IEntityDictionary entityDictionary)
        {
            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(string), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(string), indexedSomeArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        public static void CommonAssertsForMain_NullableInt(NPCProcessEntryPointInfo npcProcessInfoEntryPoint, IEntityDictionary entityDictionary)
        {
            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(int?), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(int?), indexedSomeArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutEntryPointsAndWithoutAttributesNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithTwoEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        private void CommonAssertsForEachThreeEntryPoints(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary)
        {
            Assert.Equal(3, npcProcessInfo.EntryPointsInfoList.Count);

            CommonAssertsForMainWithoutArgs(npcProcessInfo.EntryPointsInfoList);
            CommonAssertsForMain_int(npcProcessInfo.EntryPointsInfoList, entityDictionary);
            CommonAssertsForMain_bool(npcProcessInfo.EntryPointsInfoList, entityDictionary);
        }

        private void CommonAssertsForMain_bool(List<NPCProcessEntryPointInfo> entryPointsInfoList, IEntityDictionary entityDictionary)
        {
            var npcProcessInfoEntryPoint = entryPointsInfoList.Single(p => p.ParametersMap.Count == 1 && p.ParametersMap.ContainsValue(typeof(bool)));

            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(bool), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(bool), indexedSomeArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithThreeEntryPointsAndWithoutAttributesNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithThreeEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutEntryPointsAndWithoutAttributesNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachThreeEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        private void CommonAssertsForEachWithFourEntryPoints(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary)
        {
            Assert.Equal(4, npcProcessInfo.EntryPointsInfoList.Count);

            CommonAssertsForMainWithoutArgs(npcProcessInfo.EntryPointsInfoList);
            CommonAssertsForMain_int(npcProcessInfo.EntryPointsInfoList, entityDictionary);
            CommonAssertsForMain_bool(npcProcessInfo.EntryPointsInfoList, entityDictionary);
            CommonAssertsForMain_bool_int(npcProcessInfo.EntryPointsInfoList, entityDictionary);
        }

        private void CommonAssertsForMain_bool_int(List<NPCProcessEntryPointInfo> entryPointsInfoList, IEntityDictionary entityDictionary)
        {
            var npcProcessInfoEntryPoint = entryPointsInfoList.Single(p => p.ParametersMap.Count == 2 && p.ParametersMap.ContainsValue(typeof(bool)) && p.ParametersMap.ContainsValue(typeof(int)));

            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(2, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(2, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(bool), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(bool), indexedSomeArgument);


            var secondArgument = npcProcessInfoEntryPoint.ParametersMap["secondArgument"];
            Assert.Equal(typeof(int), secondArgument);

            var secondArgumentKey = entityDictionary.GetKey("secondArgument");

            var indexedSecondArgument = npcProcessInfoEntryPoint.IndexedParametersMap[secondArgumentKey];
            Assert.Equal(typeof(int), indexedSecondArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithoutAttributesNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFourEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutEntryPointsAndWithoutAttributesNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithFourEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        private void CommonAssertsForEachWithFiveEntryPoints(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary)
        {
            Assert.Equal(5, npcProcessInfo.EntryPointsInfoList.Count);

            CommonAssertsForMainWithoutArgs(npcProcessInfo.EntryPointsInfoList);
            CommonAssertsForMain_int(npcProcessInfo.EntryPointsInfoList, entityDictionary);
            CommonAssertsForMain_bool(npcProcessInfo.EntryPointsInfoList, entityDictionary);
            CommonAssertsForMain_bool_int(npcProcessInfo.EntryPointsInfoList, entityDictionary);
            CommonAssertsForMain_int_int(npcProcessInfo.EntryPointsInfoList, entityDictionary);
        }

        private void CommonAssertsForMain_int_int(List<NPCProcessEntryPointInfo> entryPointsInfoList, IEntityDictionary entityDictionary)
        {
            var npcProcessInfoEntryPoint = entryPointsInfoList.SingleOrDefault(p => p.ParametersMap.Count == 2 && p.ParametersMap.Values.Count(x => x == typeof(int)) == 2);

            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(2, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(2, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(int), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(int), indexedSomeArgument);

            var secondArgument = npcProcessInfoEntryPoint.ParametersMap["secondArgument"];
            Assert.Equal(typeof(int), secondArgument);

            var secondArgumentKey = entityDictionary.GetKey("secondArgument");

            var indexedSecondArgument = npcProcessInfoEntryPoint.IndexedParametersMap[secondArgumentKey];
            Assert.Equal(typeof(int), indexedSecondArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(0, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithoutAttributesNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFiveEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutEntryPointsAndWithoutAttributesNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithFiveEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        private static void CommonAssertsForEachWithNameAndWithoutStartupModeNPCProcess(NPCProcessInfo npcProcessInfo)
        {
            Assert.Equal("SomeName", npcProcessInfo.Name);
            Assert.Equal(NPCProcessStartupMode.NewInstance, npcProcessInfo.StartupMode);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo, globalEntityDictionary, type);
        }

        public static void CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary, Type type)
        {
            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, entityDictionary, type);
            CommonAssertsForEachWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithoutEntryPoints(npcProcessInfo);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CheckTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo, globalEntityDictionary, type);
        }

        public static void CheckTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary, Type type)
        {
            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, entityDictionary, type);
            CommonAssertsForEachWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithOneEntryPointWithoutArgs(npcProcessInfo);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithTwoEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithoutStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachThreeEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithoutStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithFourEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithoutStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithFiveEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        private void CommonAssertsForEachWithoutNameAndWithStartupModeNPCProcess(NPCProcessInfo npcProcessInfo, Type type)
        {
            Assert.Equal(type.FullName, npcProcessInfo.Name);
            Assert.Equal(NPCProcessStartupMode.Singleton, npcProcessInfo.StartupMode);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithoutNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithoutNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutNameAndWithStartupModeNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithoutEntryPoints(npcProcessInfo);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutNameAndWithStartupModeNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithOneEntryPointWithoutArgs(npcProcessInfo);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithoutNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutNameAndWithStartupModeNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithTwoEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoThreeEntryPointsAndWithoutNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoThreeEntryPointsAndWithoutNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutNameAndWithStartupModeNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachThreeEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithoutNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFourEntryPointsAndWithoutNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutNameAndWithStartupModeNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithFourEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithoutNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFiveEntryPointsAndWithoutNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithoutNameAndWithStartupModeNPCProcess(npcProcessInfo, type);
            CommonAssertsForEachWithFiveEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        private void CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(NPCProcessInfo npcProcessInfo)
        {
            Assert.Equal("SomeName", npcProcessInfo.Name);
            Assert.Equal(NPCProcessStartupMode.Singleton, npcProcessInfo.StartupMode);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithoutEntryPoints(npcProcessInfo);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithOneEntryPointWithoutArgs(npcProcessInfo);
        }

        private static void CommonAssertsForEachWithPointWithDefaultValueOfArgument(NPCProcessInfo npcProcessInfo, IEntityDictionary entityDictionary)
        {
            Assert.Equal(1, npcProcessInfo.EntryPointsInfoList.Count);

            CommonAssertsForMain_int_WithDefaultValue(npcProcessInfo.EntryPointsInfoList, entityDictionary);
        }

        private static void CommonAssertsForMain_int_WithDefaultValue(List<NPCProcessEntryPointInfo> entryPointsInfoList, IEntityDictionary entityDictionary)
        {
            var npcProcessInfoEntryPoint = entryPointsInfoList.Single(p => p.ParametersMap.Count == 1 && p.ParametersMap.ContainsValue(typeof(int)));

            CommonAssertsForEntryPoint(npcProcessInfoEntryPoint);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.ParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.ParametersMap.Count);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedParametersMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.IndexedParametersMap.Count);

            var someArgument = npcProcessInfoEntryPoint.ParametersMap["someArgument"];
            Assert.Equal(typeof(int), someArgument);

            var someArgumentKey = entityDictionary.GetKey("someArgument");

            var indexedSomeArgument = npcProcessInfoEntryPoint.IndexedParametersMap[someArgumentKey];
            Assert.Equal(typeof(int), indexedSomeArgument);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.DefaultValuesMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.DefaultValuesMap.Count);

            var someDefaultValue = npcProcessInfoEntryPoint.DefaultValuesMap["someArgument"];

            Assert.NotEqual(null, someDefaultValue);
            Assert.Equal(12, (int)someDefaultValue);

            Assert.NotEqual(null, npcProcessInfoEntryPoint.IndexedDefaultValuesMap);
            Assert.Equal(1, npcProcessInfoEntryPoint.IndexedDefaultValuesMap.Count);

            var indexedSomeDefaultValue = npcProcessInfoEntryPoint.IndexedDefaultValuesMap[someArgumentKey];

            Assert.NotEqual(null, indexedSomeDefaultValue);
            Assert.Equal(12, (int)indexedSomeDefaultValue);
        }

        [Fact]
        public void CreateTestedTestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithPointWithDefaultValueOfArgument(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithTwoEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoThreeEntryPointsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachThreeEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFourEntryPointsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithFourEntryPoints(npcProcessInfo, globalEntityDictionary);
        }

        [Fact]
        public void CreateTestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithStartupModeNPCProcess()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithFiveEntryPointsAndWithNameAndWithStartupModeNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            CommonAssertsForEachCreatedNPCProcessInfo(npcProcessInfo, globalEntityDictionary, type);
            CommonAssertsForEachWithNameAndWithStartupModeNPCProcess(npcProcessInfo);
            CommonAssertsForEachWithFiveEntryPoints(npcProcessInfo, globalEntityDictionary);
        } 
    }
}
