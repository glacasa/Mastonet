using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class List
    {
        /// <summary>
        /// ID of the list
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Title of the list
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
