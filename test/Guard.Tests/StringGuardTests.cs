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

#if NET5_0_OR_GREATER
        [Fact]
        public void Can_implicitly_capture_param_name()
        {
            string? sut = "";
            Assert.Throws<ArgumentException>(nameof(sut), () => sut.NotNullOrWhiteSpace());
        }
#endif
    }
}
