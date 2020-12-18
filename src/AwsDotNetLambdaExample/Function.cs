using System.Collections.Generic;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotNetLambdaExample
{
    public class Functions
    {
        IDateTimeProcessor processor;
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
            processor = new DateTimeProcessor();
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");
            var statusCode = HttpStatusCode.InternalServerError;
            string body = string.Empty;

            var currentUtcTime = processor.GetCurrentUTCTime();

            if (currentUtcTime != null)
            {
                statusCode = HttpStatusCode.OK;
                body = JsonConvert.SerializeObject(currentUtcTime);
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)statusCode,
                Body = body,
                Headers = new Dictionary<string, string> 
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" } 
                }
            };           
        }
    }
}
