﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#report
    public class Report
    {
        /// <summary>
        /// The ID of the report
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The action taken in response to the report
        /// </summary>
        [JsonProperty("action_taken")]
        public string ActionTaken { get; set; } = string.Empty;
    }
}
