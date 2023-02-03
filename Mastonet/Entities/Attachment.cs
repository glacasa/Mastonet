using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a file or media attachment that can be added to a status.
/// </summary>
public class Attachment
{
    /// <summary>
    /// The ID of the attachment in the database.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The type of the attachment.
    /// One of: "unknown", "image", "gifv", "video", "audio
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The location of the original full-size attachment.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// The location of a scaled-down preview of the attachment.
    /// </summary>
    [JsonProperty("preview_url")]
    public string PreviewUrl { get; set; } = string.Empty;

    /// <summary>
    /// The location of the full-size original attachment on the remote website.
    /// </summary>
    [JsonProperty("remote_url")]
    public string? RemoteUrl { get; set; }

    ///<summary>
    /// Metadata returned by Paperclip.
    ///</summary>
    [JsonProperty("meta")]
    public AttachmentMeta? Meta { get; set; }

    /// <summary>
    /// Alternate text that describes what is in the media attachment, to be used for the visually 
    /// impaired or when media attachments do not load.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// A hash computed by the BlurHash algorithm, for generating colorful preview thumbnails when 
    /// media has not been downloaded yet.
    /// </summary>
    [JsonProperty("blurhash")]
    public string? BlurHash { get; set; }

    /// <summary>
    /// A shorter URL for the attachment.
    /// </summary>
    [JsonProperty("text_url")]
    [Obsolete("Attribute was deprecated in version 3.5.0")]
    public string? TextUrl { get; set; }
}

public class AttachmentMeta
{
    [JsonProperty("original")]
    public AttachmentSizeData? Original { get; set; }

    [JsonProperty("small")]
    public AttachmentSizeData? Small { get; set; }

    [JsonProperty("focus")]
    public AttachmentFocusData? Focus { get; set; }
}

public class AttachmentSizeData
{

    [JsonProperty("width")]
    public int? Width { get; set; }

    [JsonProperty("height")]
    public int? Height { get; set; }


    [JsonProperty("size")]
    public string? Size { get; set; }

    [JsonProperty("aspect")]
    public double? Aspect { get; set; }

    [JsonProperty("frame_rate")]
    public string? FrameRate { get; set; }

    [JsonProperty("duration")]
    public double? Duration { get; set; }

    [JsonProperty("bitrate")]
    public int? BitRate { get; set; }
}

public class AttachmentFocusData
{
    [JsonProperty("x")]
    public double X { get; set; }

    [JsonProperty("y")]
    public double Y { get; set; }
}
