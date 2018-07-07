using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BinaryDepths.Extensions.Tests.System.IO.StreamExtensions
{
    public class ToByteArray
    {
        [Fact]
        public void Given_Empty_Stream_Should_Return_Empty_Array()
        {
            using (var stream = new MemoryStream(new byte[0], false))
            {
                var bytes = stream.ToByteArray();

                Assert.Empty(bytes);
            }
        }

        [Fact]
        public void Given_Stream_With_Length_1_Should_Return_Array_Length_1()
        {
            using (var stream = new MemoryStream(new byte[] { 0xDE }, false))
            {
                var bytes = stream.ToByteArray();

                Assert.Single(bytes);
            }
        }

        [Fact]
        public void Given_Stream_With_Known_Values_Should_Return_Array_With_Same_Values()
        {
            var knownBytes = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
            using (var stream = new MemoryStream(knownBytes, false))
            {
                var bytes = stream.ToByteArray();

                Assert.Equal(knownBytes, bytes);
            }
        }
    }
}
