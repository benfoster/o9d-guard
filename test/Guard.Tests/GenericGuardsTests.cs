using System;
using O9d.Guard;
using Shouldly;
using Xunit;

namespace test.Guard.Tests
{
    public class GenericGuardsTests
    {
        [Fact]
        public void Throws_if_null()
        {
            Assert.Throws<ArgumentNullException>("name", () => default(string).NotNull("name"));
        }

        [Fact]
        public void Returns_value_if_not_null()
        {
            "value".NotNull("name").ShouldBe("value");
        }
    }
}