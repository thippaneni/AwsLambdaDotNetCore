using System;

namespace AwsDotNetLambdaExample
{
    public interface IDateTimeProcessor
    {
        DateTime GetCurrentUTCTime();       
    }
}
