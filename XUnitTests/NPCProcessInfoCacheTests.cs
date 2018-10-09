using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTests.Helpers;

namespace XUnitTests
{
    public class NPCProcessInfoCacheTests
    {
        [Fact]
        public void SetNull_GotArgumentNullException()
        {
            var npcProcessInfoCache = new NPCProcessInfoCache();

            var e = Assert.Throws<ArgumentNullException>(() => {
                npcProcessInfoCache.Set(null);
            });

            Assert.Equal("info", e.ParamName);
        }

        [Fact]
        public void SetValidInfo_GotTrue()
        {
            var entityLogger = new EmptyEntityLogger();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = npcProcessInfoCache.Set(npcProcessInfo);

            Assert.Equal(true, result);
        }

        [Fact]
        public void SetValidInfoTwise_GotFalseAtSecontAttempt()
        {
            var entityLogger = new EmptyEntityLogger();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var result = npcProcessInfoCache.Set(npcProcessInfo);
            Assert.Equal(true, result);

            var result_2 = npcProcessInfoCache.Set(npcProcessInfo);
            Assert.Equal(false, result_2);
        }

        [Fact]
        public void GetByNull_GotArgumentNullException()
        {
            var npcProcessInfoCache = new NPCProcessInfoCache();

            var e = Assert.Throws<ArgumentNullException>(() => {
                npcProcessInfoCache.Get(null);
            });

            Assert.Equal("type", e.ParamName);
        }

        [Fact]
        public void GetByNotRegisteredType_GotNull()
        {
            var npcProcessInfoCache = new NPCProcessInfoCache();

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);

            var result = npcProcessInfoCache.Get(type);

            Assert.Equal(null, result);
        }

        [Fact]
        public void GetRegisteredType_GotInfoForTheType()
        {
            var entityLogger = new EmptyEntityLogger();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithOneEntryPointWithoutArgsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            npcProcessInfoCache.Set(npcProcessInfo);

            var result = npcProcessInfoCache.Get(type);

            Assert.NotEqual(null, result);
            Assert.NotEqual(null, result.Type);

            Assert.Equal(type, result.Type);
        }
    }
}
