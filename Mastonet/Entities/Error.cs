using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Error
    {
        /// <summary>
        /// A textual description of the error
        /// </summary>
        [JsonProperty("error")]
        public string Description { get; set; }
    }
}
