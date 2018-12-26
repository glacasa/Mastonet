using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class AppRegistration
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("redirect_uri")]
        public string? RedirectUri { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; } = string.Empty;

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;

        [JsonIgnore]
        public string Instance { get; set; } = string.Empty;

        [JsonIgnore]
        public Scope Scope { get; set; }
    }
}
