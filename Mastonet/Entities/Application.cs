using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Application
    {
        /// <summary>
        /// Name of the app
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Homepage URL of the app
        /// </summary>
        [JsonProperty("website")]
        public string Website { get; set; }
    }
}
