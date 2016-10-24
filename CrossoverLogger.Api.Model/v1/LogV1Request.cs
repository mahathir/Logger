using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverLogger.Api.Model.v1
{
    public class LogV1Request : BaseV1Request
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("logger")]
        public string Logger { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
