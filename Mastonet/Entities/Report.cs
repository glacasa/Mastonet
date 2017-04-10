using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Report
    {
        /// <summary>
        /// The ID of the report
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The action taken in response to the report
        /// </summary>
        [JsonProperty("action_taken")]
        public string ActionTaken { get; set; }
    }
}
