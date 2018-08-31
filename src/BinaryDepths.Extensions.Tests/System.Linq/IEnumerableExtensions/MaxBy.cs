using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace BinaryDepths.Extensions.Tests.System.Linq.IEnumerableExtensions
{
    public class MaxBy
    {
        [Fact]
        public void Given_Enumerable_With_NoElements_Throws_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                                                     {
                                                         var enumerable = new object[] { };
                                                         return enumerable.MaxBy(o => o.GetHashCode());
                                                     });
        }

        [Fact]
        public void Given_Enumerable_With_SingleElement_Returns_ThatElement()
        {
            var someObject = new object();
            var enumerable = new[] {someObject};
            Assert.Equal(someObject, enumerable.MaxBy(o => o.GetHashCode()));
        }

        [Fact]
        public void Given_Enumerable_With_MultipleElements_Returns_ElementWithMaximumPropertyValue()
        {
            var maxObject = "this is the longest string";
            var enumerable = new[] {"short", "also short", maxObject, "not max", "another"};
            Assert.Equal(maxObject, enumerable.MaxBy(o => o.Length));
        }
    }
}