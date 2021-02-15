using System;
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
    }
}