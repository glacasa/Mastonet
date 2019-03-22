using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public long Id { get; set; }

        /// <summary>
        /// One of: "image", "video", "gifv", "unknown"
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

        ///<summary>
        /// Metadata of the attachment
        ///</summary>
        [JsonProperty("meta")]
        public AttachmentMeta Meta { get; set; }

        /// <summary>
        /// Description of the attachment
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class AttachmentMeta
    {
        [JsonProperty("original")]
        public AttachmentSizeData Original { get; set; }

        [JsonProperty("small")]
        public AttachmentSizeData Small { get; set; }

        [JsonProperty("focus")]
        public AttachmentFocusData Focus { get; set; }
    }

    public class AttachmentSizeData
    {

        [JsonProperty("width")]
        public int? Width { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }


        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("aspect")]
        public float? Aspect { get; set; }

        [JsonProperty("frame_rate")]
        public string FrameRate { get; set; }

        [JsonProperty("duration")]
        public float? Duration { get; set; }

        [JsonProperty("bitrate")]
        public int? BitRate { get; set; }
    }

    public class AttachmentFocusData
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }
    }
}
