using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class IdFactoryTests
    {
        [Fact]
        public void TestCase1()
        {
            IIdFactory idFactory = new IdFactory();

            var value1 = idFactory.GetNewId();
            Assert.Equal(1ul, value1);

            var value2 = idFactory.GetNewId();
            Assert.Equal(2ul, value2);

            var value3 = idFactory.GetNewId();
            Assert.Equal(3ul, value3);
        }
    }
}
