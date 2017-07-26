using System;
using Xunit;

namespace presenter.predictions.tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ToDateTime_case()
        {
            const string TEXT = "3/14/2017 12:00:00 AM";
            var expected = new DateTime(2017, 3, 14);

            var actual = TEXT.ToDateTime();

            Assert.Equal(expected, actual);
        }
    }
}