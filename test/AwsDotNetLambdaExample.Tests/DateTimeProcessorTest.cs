using System;
using Xunit;

namespace AwsDotNetLambdaExample.Tests
{
    public class DateTimeProcessorTest
    {
        [Fact]
        public void TestCurrentTimeUTC()
        {
            // Arrange
            var processor = new DateTimeProcessor();
            var preTestTimeUtc = DateTime.UtcNow;

            // Act
            var result = processor.GetCurrentUTCTime();

            // Assert
            var postTestTimeUtc = DateTime.UtcNow;
            Assert.True(result >= preTestTimeUtc);
            Assert.True(result <= postTestTimeUtc);
        }
    }
}
