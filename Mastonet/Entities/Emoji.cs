using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Mastonet.Entities
{
    public class Emoji
    {
        /// <summary>
        /// The shortcode of the emoji
        /// </summary>
        [JsonProperty("shortcode")]
        public string Shortcode { get; set; }

        /// <summary>
        /// URL to the emoji static image
        /// </summary>
        [JsonProperty("static_url")]
        public string StaticUrl { get; set; }

        /// <summary>
        /// URL to the emoji image
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Boolean that indicates if the emoji is visible in picker
        /// </summary>
        [JsonProperty("visible_in_picker")]
        public bool VisibleInPicker { get; set; }
    }
}
