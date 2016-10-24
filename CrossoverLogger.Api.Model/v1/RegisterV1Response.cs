using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverLogger.Api.Model.v1
{
    public class RegisterV1Response : BaseV1Response
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }
        [JsonProperty("application_secret")]
        public string ApplicationSecret { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
