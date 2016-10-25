using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverLogger.Commons
{
    public static class Constants
    {
        public static class Config
        {
            public const string RATE_LIMIT = "RateLimit";
            public const string RATE_LIMIT_TIME = "RateLimitTime";
            public const string SUSPEND_TIME = "SuspendTime";
        }

        public static class HttpHeader
        {
            public const string API_VERSION = "X-CrossoverLogger-Version";
        }

        public static class Others
        {
            public const string API_DEFAULT_VERSION = "V1";
        }
    }
}
