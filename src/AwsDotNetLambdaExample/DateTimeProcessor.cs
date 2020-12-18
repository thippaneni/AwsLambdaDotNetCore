using System;

namespace AwsDotNetLambdaExample
{
    public class DateTimeProcessor : IDateTimeProcessor
    {
        public DateTime GetCurrentUTCTime()
        {
            return DateTime.UtcNow;
        }
    }
}
