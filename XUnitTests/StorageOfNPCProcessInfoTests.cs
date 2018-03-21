using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class StorageOfNPCProcessInfoTests
    {
        [Fact]
        public void AddTypeOfProcess_AddNull_GotArgumentNullException()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            var e = Assert.Throws<ArgumentNullException>(() => {
                storage.AddTypeOfProcess(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void AddTypeOfProcess_AddNullToDisposed_GotElementIsNotActiveException()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                storage.AddTypeOfProcess(null);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_GotTypeIsNotNPCProcessException()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(StorageOfNPCProcessInfoTests);

            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                storage.AddTypeOfProcess(type);
            });

            Assert.Equal(type, e.WrongType);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_GotElementIsNotActiveException()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            storage.Dispose();

            var type = typeof(StorageOfNPCProcessInfoTests);

            Assert.Throws<ElementIsNotActiveException>(() => {
                storage.AddTypeOfProcess(type);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_GotNothing()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            throw new NotImplementedException();
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_GotNothing()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            storage.AddTypeOfProcess(type);
        }

        [Fact]
        public void AddTypeOfProcess_AddTwoDifferentTypesOfProcesesWithTheSameName_GotNothing()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);

            var type_2 = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type_2);

            Assert.NotEqual(type, type_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddOneTypeToTwoLinkedStorages_GotEqualsReferences()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            var storage_2 = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            Assert.NotEqual(storage, storage_2);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            storage.AddTypeOfProcess(type);
            storage_2.AddTypeOfProcess(type);
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedType_GotThatInfoByTypeAndKey()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            throw new NotImplementedException();
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedTypeByKey_IfPreviouslyWasAddedTwoDifferentTypesOfProcesesWithTheSameName_GotInfoAboutLastType()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            throw new NotImplementedException();
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedTypeToDisposed_GotElementIsNotActiveException()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            throw new NotImplementedException();
        }

        [Fact]
        public void TryGetInfoAboutPreviouslyAddedTypeByKey_IfPreviouslyWasAddedTwoDifferentTypesOfProcesesWithTheSameName_OnDisposed_GotElementIsNotActiveException()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            throw new NotImplementedException();
        }

        [Fact]
        public void TryGetInfoAboutOfNotRegisterType_GotNull()
        {
            var globalEntityDictionary = new EntityDictionary();
            var globalNPCProcessInfoCache = new NPCProcessInfoCache();
            var storage = new StorageOfNPCProcessInfo(globalEntityDictionary, globalNPCProcessInfoCache);

            throw new NotImplementedException();
        }
    }
}
