using System.Collections.Generic;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;
using Amazon.XRay.Recorder.Core;

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
            LogMessageToCloudWatch(context, "Get Request recieved and processing");

            var statusCode = HttpStatusCode.InternalServerError;
            string body = string.Empty;

            var currentUtcTime = TraceFunction(processor.GetCurrentUTCTime, "GetCurrentUTCTime");
            LogMessageToCloudWatch(context, "Processing request completed.");

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

        private void LogMessageToCloudWatch(ILambdaContext context, string message)
        {
            var logMessage = string.Format("{0}:{1} - {2}", context.AwsRequestId, context.FunctionName, message);
            context.Logger.LogLine(logMessage);
        }

        private T TraceFunction<T>(Func<T> func, string subSegmentName)
        {
            AWSXRayRecorder.Instance.BeginSubsegment(subSegmentName);
            T result = func();
            AWSXRayRecorder.Instance.EndSubsegment();

            return result;
        }
    }
}
