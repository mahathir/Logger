using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverLogger.Api.Model.v1
{
    public class RegisterV1Request : BaseV1Request
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
