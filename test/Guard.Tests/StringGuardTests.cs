using System;
using Shouldly;
using Xunit;

namespace O9d.Guard.Tests
{
    public class StringGuardTests
    {
        [Theory]
        [InlineData(default(string))]
        [InlineData("")]
        [InlineData(" ")]
        public void NotNullOrWhiteSpace_throws_if_no_value_provided(string value)
        {
            Assert.Throws<ArgumentException>("name", () => value.NotNullOrWhiteSpace("name"));
        }

        [Fact]
        public void NotNullOrWhiteSpace_returns_value_if_provided()
        {
            "value".NotNullOrWhiteSpace("name").ShouldBe("value");
        }
    }
}