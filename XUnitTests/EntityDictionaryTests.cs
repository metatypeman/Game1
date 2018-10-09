using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class EntityDictionaryTests
    {
        [Fact]
        public void GotNullReturnZero()
        {
            IEntityDictionary entityDictionary = new EntityDictionary();

            Assert.Equal(0ul, entityDictionary.GetKey(null));
        }

        [Fact]
        public void GotEmptyReturnZero()
        {
            IEntityDictionary entityDictionary = new EntityDictionary();

            Assert.Equal(0ul, entityDictionary.GetKey(string.Empty));
        }

        [Fact]
        public void GotWhiteSpaceReturnZero()
        {
            IEntityDictionary entityDictionary = new EntityDictionary();

            Assert.Equal(0ul, entityDictionary.GetKey(" "));
            Assert.Equal(0ul, entityDictionary.GetKey("              "));
        }

        [Fact]
        public void TestCase1()
        {
            IEntityDictionary entityDictionary = new EntityDictionary();

            var value_1 = "a";

            var key_1_1 = entityDictionary.GetKey(value_1);
            var key_1_2 = entityDictionary.GetKey(value_1);

            Assert.Equal(1ul, key_1_1);
            Assert.Equal(key_1_1, key_1_2);

            var value_2 = "b";

            var key_2_1 = entityDictionary.GetKey(value_2);
            var key_2_2 = entityDictionary.GetKey(value_2);

            Assert.Equal(2ul, key_2_1);
            Assert.Equal(key_2_1, key_2_2);

            Assert.NotEqual(key_1_1, key_2_1);
        }

        [Fact]
        public void GotValuesInDifferentCasesReturnTheSameKey()
        {
            IEntityDictionary entityDictionary = new EntityDictionary();

            var value_1 = "a";
            var key_1 = entityDictionary.GetKey(value_1);

            var value_2 = "A";
            var key_2 = entityDictionary.GetKey(value_2);

            Assert.Equal(key_1, key_2);

            var value_3 = "b";
            var key_3 = entityDictionary.GetKey(value_3);

            Assert.NotEqual(key_1, key_3);
        }
    }
}
