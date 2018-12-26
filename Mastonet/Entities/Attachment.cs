using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#attachment
    public class Attachment
    {
        /// <summary>
        /// ID of the attachment
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// One of: "image", "video", "gifv", "unknown"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// URL of the locally hosted version of the image
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// For remote images, the remote URL of the original image
        /// </summary>
        [JsonProperty("remote_url")]
        public string? RemoteUrl { get; set; }

        /// <summary>
        /// URL of the preview image
        /// </summary>
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; } = string.Empty;

        /// <summary>
        /// Shorter URL for the image, for insertion into text (only present on local images)
        /// </summary>
        [JsonProperty("text_url")]
        public string? TextUrl { get; set; }

        //TODO : attachment metadata 

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}
