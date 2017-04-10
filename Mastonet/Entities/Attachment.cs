using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Attachment
    {
        /// <summary>
        /// ID of the attachment
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// One of: "image", "video", "gifv"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// URL of the locally hosted version of the image
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// For remote images, the remote URL of the original image
        /// </summary>
        [JsonProperty("remote_url")]
        public string RemoteUrl { get; set; }

        /// <summary>
        /// URL of the preview image
        /// </summary>
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Shorter URL for the image, for insertion into text (only present on local images)
        /// </summary>
        [JsonProperty("text_url")]
        public string TextUrl { get; set; }
    }
}
