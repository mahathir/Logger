namespace CrossoverLogger.Api.Client.v1
{
    using CrossoverLogger.Api.Model.v1;
    using CrossoverLogger.Commons;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class CrossoverLoggerClient
    {
        private WebClient client;
        public string Token { get; set; }

        public CrossoverLoggerClient(string baseUrl, string token = null)
        {
            this.client = new WebClient();
            this.client.BaseAddress = baseUrl;
            this.client.Headers.Remove(Constants.HttpHeader.API_VERSION);
            this.client.Headers.Add(Constants.HttpHeader.API_VERSION, "V1");
            this.client.Headers.Remove("Accept");
            this.client.Headers.Add("Accept", "application/json");
            this.client.Headers.Remove("Content-Type");
            this.client.Headers.Add("Content-Type", "application/json");
            this.Token = token;
        }

        public RegisterV1Response Register(RegisterV1Request request)
        {
            var text = this.client.UploadString("register", JsonConvert.SerializeObject(request));
            return JsonConvert.DeserializeObject<RegisterV1Response>(text);
        }

        public AuthV1Response Auth(string appId, string appSecret)
        {
            this.client.Headers.Remove("Authorization");
            this.client.Headers.Add("Authorization", string.Format("{0}:{1}", appId, appSecret));
            var text = this.client.UploadString("auth", string.Empty);
            return JsonConvert.DeserializeObject<AuthV1Response>(text);
        }

        public LogV1Response Log(LogV1Request request, string token = null)
        {
            token = token ?? this.Token;
            this.client.Headers.Remove("Authorization");
            this.client.Headers.Add("Authorization", token);
            var text = this.client.UploadString("log", JsonConvert.SerializeObject(request));
            return JsonConvert.DeserializeObject<LogV1Response>(text);
        }
    }
}
