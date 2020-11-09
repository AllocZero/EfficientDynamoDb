using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Runtime
{
    public static class PaginatorUtils
    {
        public static void SetUserAgentAdditionOnRequest(AmazonWebServiceRequest request)
        {
            request.UserAgentAddition = $" Paginator";
        }
    }
}
