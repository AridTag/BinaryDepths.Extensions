using System;
using System.Linq;
using Xunit;

namespace BinaryDepths.Extensions.Tests.System.Linq.IEnumerableExtensions
{
    public class MinBy
    {
        [Fact]
        public void Given_Enumerable_With_NoElements_Throws_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                                                     {
                                                         var enumerable = new object[] { };
                                                         return enumerable.MinBy(o => o.GetHashCode());
                                                     });
        }

        [Fact]
        public void Given_Enumerable_With_SingleElement_Returns_ThatElement()
        {
            var someObject = new object();
            var enumerable = new[] {someObject};
            Assert.Equal(someObject, enumerable.MinBy(o => o.GetHashCode()));
        }

        [Fact]
        public void Given_Enumerable_With_MultipleElements_Returns_ElementWithMinimumPropertyValue()
        {
            var minObject = string.Empty;
            var enumerable = new[] {"short", "also short", "not at all short", "sadasfafdsf", minObject, "another"};
            Assert.Equal(minObject, enumerable.MinBy(o => o.Length));
        }
    }
}