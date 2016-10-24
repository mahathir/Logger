using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverLogger.Api.Model.v1
{
    public class AuthV1Response : BaseV1Response
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
    }
}
