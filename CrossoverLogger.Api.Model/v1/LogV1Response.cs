using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverLogger.Api.Model.v1
{
    public class LogV1Response : BaseV1Response
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
