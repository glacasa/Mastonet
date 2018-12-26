using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#error
    public class Error
    {
        /// <summary>
        /// A textual description of the error
        /// </summary>
        [JsonProperty("error")]
        public string Description { get; set; } = string.Empty;
    }
}
