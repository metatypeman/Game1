using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTests.Helpers;

namespace XUnitTests
{
    public class StorageOfAbstractNPCProcessesTests
    {
        [Fact]
        public void AddTypeOfProcess_SetNull_GotArgumentNullException()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var e = Assert.Throws<ArgumentNullException>(() => {
                storage.AddTypeOfProcess(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void AddTypeOfProcess_SetNull_OnDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                storage.AddTypeOfProcess(null);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcess_GotTypeIsNotNPCProcessException()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = GetType();

            var e = Assert.Throws<TypeIsNotNPCProcessException>(() => {
                storage.AddTypeOfProcess(type);
            });

            Assert.Equal(type, e.WrongType);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWhatIsNotBasedOnBaseNPCProcessToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            storage.Dispose();

            var type = GetType();

            Assert.Throws<ElementIsNotActiveException>(() => {
                storage.AddTypeOfProcess(type);
            });
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalType_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);
        }

        [Fact]
        public void AddTypeOfProcess_AddNormalTypeTwise_GotTrueFirstlyAndFalseSecondly()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

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
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var type_2 = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithNameAndWithoutStartupModeNPCProcess);

            var result_2 = storage.AddTypeOfProcess(type_2);

            Assert.Equal(false, result_2);
        }

        [Fact]
        public void AddTypeOfProcess_AddTypeWithoutEntryPoints_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);
        }

        [Fact]
        public void TryGetProcessOfNotAddedType_GotNull()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var process = storage.GetProcess(internalCommand);

            Assert.Equal(null, process);
        }

        [Fact]
        public void TryGetProcessOfNotAddedType_ToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var process = storage.GetProcess(internalCommand);
            });
        }

        [Fact]
        public void TryGetProcessOfTypeWithoutEntryPoints_GotNull()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var process = storage.GetProcess(internalCommand);
            Assert.Equal(null, process);
        }

        private static void CheckCreatedBaseNPCProcess(BaseNPCProcessInvocablePackage processPack, Type targetType, INPCContext context)
        {
            Assert.NotEqual(null, processPack);

            Assert.NotEqual(null, processPack.Command);
            Assert.NotEqual(null, processPack.EntryPoint);
            Assert.NotEqual(0f, processPack.RankOfEntryPoint);

            var process = processPack.Process;

            Assert.NotEqual(null, process);

            Assert.Equal(targetType, process.GetType());
            Assert.Equal(StateOfNPCProcess.Created, process.State);
            Assert.Equal(context, process.Context);
            Assert.NotEqual(0ul, process.Id);
            Assert.Equal(KindOfNPCProcess.Abstract, process.Kind);
        }

        [Fact]
        public void TryGetProcess_GotProcessOfTheType()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var process = storage.GetProcess(internalCommand);

            CheckCreatedBaseNPCProcess(process, type, testedContext);          
        }

        [Fact]
        public void TryGetProcess_ToDisposed_GotElementIsNotActiveException()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithoutStartupModeNPCProcess);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            storage.Dispose();

            Assert.Throws<ElementIsNotActiveException>(() => {
                var process = storage.GetProcess(internalCommand);
            });
        }

        [Fact]
        public void TryGetSingletonProcessTwice_GotInstancesWithEqualsReferences()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcessSingleton);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var process_1 = storage.GetProcess(internalCommand);
            CheckCreatedBaseNPCProcess(process_1, type, testedContext);

            var process_2 = storage.GetProcess(internalCommand);
            CheckCreatedBaseNPCProcess(process_2, type, testedContext);

            Assert.Equal(false, ReferenceEquals(process_1, process_2));
            Assert.Equal(true, ReferenceEquals(process_1.Process, process_2.Process));
        }

        [Fact]
        public void TryGetNewInstanceProcessTwice_GotInstancesWithDifferentReferences()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcessNewInstance);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var process_1 = storage.GetProcess(internalCommand);
            CheckCreatedBaseNPCProcess(process_1, type, testedContext);

            var process_2 = storage.GetProcess(internalCommand);
            CheckCreatedBaseNPCProcess(process_2, type, testedContext);

            Assert.Equal(false, ReferenceEquals(process_1, process_2));
        }

        [Fact]
        public void TryGetNewStandaloneInstanceProcessTwice_GotInstancesWithDifferentReferences()
        {
            var entityLogger = new EmptyEntityLogger();
            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithNameAndWithStartupModeNPCProcessNewStandaloneInstance);

            var result = storage.AddTypeOfProcess(type);

            Assert.Equal(true, result);

            var name = "SomeName";
            var command = new NPCCommand();
            command.Name = name;

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            var process_1 = storage.GetProcess(internalCommand);
            CheckCreatedBaseNPCProcess(process_1, type, testedContext);

            var process_2 = storage.GetProcess(internalCommand);
            CheckCreatedBaseNPCProcess(process_2, type, testedContext);

            Assert.Equal(false, ReferenceEquals(process_1, process_2));
        }
    }
}
