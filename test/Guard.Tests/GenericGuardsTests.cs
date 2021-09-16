using System;
using System.Diagnostics;
using Shouldly;
using Xunit;

namespace O9d.Guard.Tests
{
    public class GenericGuardsTests
    {
        [Fact]
        public void NotNull_throws_if_null()
        {
            Assert.Throws<ArgumentNullException>("name", () => default(string).NotNull("name"));
        }

        [Fact]
        public void NotNull_returns_value_if_not_null()
        {
            "value".NotNull("name").ShouldBe("value");
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void Can_implicitly_capture_param_name()
        {
            object? sut = null;
            Assert.Throws<ArgumentNullException>(nameof(sut), () => sut.NotNull());
        }
#endif
    }
}
