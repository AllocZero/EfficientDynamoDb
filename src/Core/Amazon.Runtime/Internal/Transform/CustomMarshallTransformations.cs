using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Runtime.Internal.Transform
{
    public static class CustomMarshallTransformations
    {
        public static long ConvertDateTimeToEpochMilliseconds(DateTime dateTime)
        {
            TimeSpan ts = new TimeSpan(dateTime.ToUniversalTime().Ticks - Amazon.Util.AWSSDKUtils.EPOCH_START.Ticks);
            return (long)ts.TotalMilliseconds;
        }
    }
}
