using System.IO;
using System.Linq;
using Xunit;

namespace BinaryDepths.Extensions.Tests.System.Linq.IEnumerableExtensions
{
    public class HasSingleElement
    {
        [Fact]
        public void Given_Enumerable_With_NoElements_Should_Return_False()
        {
            var enumerable = new int[] { };
            Assert.False(enumerable.HasSingleElement());
        }

        [Fact]
        public void Given_Enumerable_With_SingleElement_Should_Return_True()
        {
            var enumerable = new int[] {0};
            Assert.True(enumerable.HasSingleElement());
        }

        [Fact]
        public void Given_Enumerable_With_MultipleElements_Should_Return_False()
        {
            var enumerable = new int[] {0, 2};
            Assert.False(enumerable.HasSingleElement());
        }
    }
}