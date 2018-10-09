using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class StringHelperTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(10)]
        public void PutNumber_GotStringWithWhiteSpacesOnlyAndLenghtEquTheNumber(uint n)
        {
            var spaces = StringHelper.Spaces(n);

            Assert.NotEqual(null, spaces);
            Assert.Equal(n, (uint)spaces.Length);
        }
    }
}
