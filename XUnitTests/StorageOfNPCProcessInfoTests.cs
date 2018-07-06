using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTests.Helpers;

namespace XUnitTests
{
    public class StorageOfNPCProcessInfoTests
    {
        [Fact]
        public void AddTypeOfProcess_AddNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var e = Assert.Throws<ArgumentNullException>(() => {
                storage.AddTypeOfProcess(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void AddTypeOfProcess_AddNullToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                storage.AddTypeOfProcess(null);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_GotTypeIsNotNPCProcessException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(StorageOfNPCProcessInfoTests);

            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                storage.AddTypeOfProcess(type);
            });

            Assert.Equal(type, e.WrongType);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            storage.Dispose();

            var type = typeof(StorageOfNPCProcessInfoTests);

            Assert.Throws<ElementIsNotActiveException>(() => {
                storage.AddTypeOfProcess(type);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_GotTrueFirstlyAndFalseSecondly()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var result_2 = storage.AddTypeOfProcess(type);

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddTwoDifferentTypesOfProcesesWithTheSameName_GotTrueFirstlyAndFalseSecondly()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var type_2 = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            var result_2 = storage.AddTypeOfProcess(type_2);

            Assert.Equal(false, result_2);

            Assert.NotEqual(type, type_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddOneTypeToTwoLinkedStorages_NextTryGetInfo_GotEqualsReferences()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var storage_2 = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            Assert.NotEqual(storage, storage_2);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);
            storage_2.AddTypeOfProcess(type);

            var npcProcessInfo = storage.GetNPCProcessInfo(type);
            var npcProcessInfo_2 = storage_2.GetNPCProcessInfo(type);

            Assert.NotEqual(null, npcProcessInfo);
            Assert.NotEqual(null, npcProcessInfo_2);
            Assert.Equal(true, ReferenceEquals(npcProcessInfo, npcProcessInfo_2));
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedType_GotThatInfoByTypeAndKey()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);

            var name = "SomeName";

            var key = globalEntityDictionary.GetKey(name);

            var npcProcessInfo = storage.GetNPCProcessInfo(type);

            Assert.NotEqual(null, npcProcessInfo);

            var npcProcessInfo_1 = storage.GetNPCProcessInfo(key);

            Assert.NotEqual(null, npcProcessInfo_1);

            Assert.Equal(npcProcessInfo, npcProcessInfo_1);
            Assert.Equal(true, ReferenceEquals(npcProcessInfo, npcProcessInfo_1));

            NPCProcessInfoFactoryTests.CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo, globalEntityDictionary, type);
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedTypeByKey_IfPreviouslyWasAddedTwoDifferentTypesOfProcesesWithTheSameName_GotInfoAboutFirstType()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);

            var name = "SomeName";

            var key = globalEntityDictionary.GetKey(name);

            var type_2 = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            Assert.NotEqual(type, type_2);

            storage.AddTypeOfProcess(type_2);

            var npcProcessInfoByType = storage.GetNPCProcessInfo(type);

            NPCProcessInfoFactoryTests.CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfoByType, globalEntityDictionary, type);

            var npcProcessInfoByType_2 = storage.GetNPCProcessInfo(type_2);

            Assert.Equal(null, npcProcessInfoByType_2);

            Assert.NotEqual(npcProcessInfoByType, npcProcessInfoByType_2);

            var npcProcessInfoByKey = storage.GetNPCProcessInfo(key);

            Assert.NotEqual(null, npcProcessInfoByKey);

            Assert.Equal(npcProcessInfoByType, npcProcessInfoByKey);

            NPCProcessInfoFactoryTests.CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfoByType, globalEntityDictionary, type);
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedTypeByTypeToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var npcProcessInfo = storage.GetNPCProcessInfo(type);
            });
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedTypeByKeyToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);

            var name = "SomeName";

            var key = globalEntityDictionary.GetKey(name);

            var npcProcessInfo = storage.GetNPCProcessInfo(type);

            Assert.NotEqual(null, npcProcessInfo);

            NPCProcessInfoFactoryTests.CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfo, globalEntityDictionary, type);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var npcProcessInfo_1 = storage.GetNPCProcessInfo(key);
            });
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedTypeByKey_IfPreviouslyWasAddedTwoDifferentTypesOfProcesesWithTheSameName_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);

            var name = "SomeName";

            var key = globalEntityDictionary.GetKey(name);

            var type_2 = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            Assert.NotEqual(type, type_2);

            storage.AddTypeOfProcess(type_2);

            var npcProcessInfoByType = storage.GetNPCProcessInfo(type);

            NPCProcessInfoFactoryTests.CheckTestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess(npcProcessInfoByType, globalEntityDictionary, type);

            var npcProcessInfoByType_2 = storage.GetNPCProcessInfo(type_2);

            Assert.Equal(null, npcProcessInfoByType_2);

            Assert.NotEqual(npcProcessInfoByType, npcProcessInfoByType_2);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var npcProcessInfoByKey = storage.GetNPCProcessInfo(key);
            });
        }

        [Fact]
        public void TryGetInfoAboutOfNotRegisterType_GotNull()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            var name = "SomeName";

            var key = globalEntityDictionary.GetKey(name);

            var npcProcessInfoByType = storage.GetNPCProcessInfo(type);

            Assert.Equal(null, npcProcessInfoByType);

            var npcProcessInfoByKey = storage.GetNPCProcessInfo(key);

            Assert.Equal(null, npcProcessInfoByKey);
        }

        [Fact]
        public void TryGetInfoAboutOfNotRegisterType_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            var name = "SomeName";

            var key = globalEntityDictionary.GetKey(name);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var npcProcessInfoByType = storage.GetNPCProcessInfo(type);
            });

            Assert.Throws<ElementIsNotActiveException>(() => {
                var npcProcessInfoByKey = storage.GetNPCProcessInfo(key);
            });  
        }

        [Fact]
        public void TryGetInfoByNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var e = Assert.Throws<ArgumentNullException>(() => {
                var npcProcessInfoByKey = storage.GetNPCProcessInfo(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void TryGetInfoByNull_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var npcProcessInfoByKey = storage.GetNPCProcessInfo(null);
            });
        }

        [Fact]
        public void TryGetInfoByZero_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            var e = Assert.Throws<ArgumentNullException>(() => {
                var npcProcessInfoByKey = storage.GetNPCProcessInfo(0u);
            });

            Assert.Equal("key", e.ParamName);
        }

        [Fact]
        public void TryGetInfoByZero_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var npcProcessInfoByKey = storage.GetNPCProcessInfo(0);
            });
        }

        [Fact]
        public void CallDisposeTwise_GotNothing()
        {
            var entityLogger = new EmptyEntityLogger();
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(entityLogger, globalEntityDictionary, globalNPCProcessInfoCache);
            storage.Dispose();
            storage.Dispose();
        }
    }
}
