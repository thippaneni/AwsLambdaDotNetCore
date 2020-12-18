using System;
using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

namespace AwsDotNetLambdaExample.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        [Fact]
        public void TetGetMethod()
        {
            // Arrange
            var context = new TestLambdaContext();
            var request = new APIGatewayProxyRequest();
            var utcDatetimenow = DateTime.UtcNow;
            Functions functions = new Functions();

            // Act
            var response = functions.Get(request, context);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.Contains(utcDatetimenow.Year.ToString(), response.Body);
        }
    }
}
