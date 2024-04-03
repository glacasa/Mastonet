using System.Text.Json.Serialization;
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
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The type of the attachment.
    /// One of: "unknown", "image", "gifv", "video", "audio"
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The location of the original full-size attachment.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// The location of a scaled-down preview of the attachment.
    /// </summary>
    [JsonPropertyName("preview_url")]
    public string PreviewUrl { get; set; } = string.Empty;

    /// <summary>
    /// The location of the full-size original attachment on the remote website.
    /// </summary>
    [JsonPropertyName("remote_url")]
    public string? RemoteUrl { get; set; }

    ///<summary>
    /// Metadata returned by Paperclip.
    ///</summary>
    [JsonPropertyName("meta")]
    public AttachmentMeta? Meta { get; set; }

    /// <summary>
    /// Alternate text that describes what is in the media attachment, to be used for the visually 
    /// impaired or when media attachments do not load.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// A hash computed by the BlurHash algorithm, for generating colorful preview thumbnails when 
    /// media has not been downloaded yet.
    /// </summary>
    [JsonPropertyName("blurhash")]
    public string? BlurHash { get; set; }

    /// <summary>
    /// A shorter URL for the attachment.
    /// </summary>
    [JsonPropertyName("text_url")]
    [Obsolete("Attribute was deprecated in version 3.5.0")]
    public string? TextUrl { get; set; }
}

public class AttachmentMeta
{
    [JsonPropertyName("original")]
    public AttachmentSizeData? Original { get; set; }

    [JsonPropertyName("small")]
    public AttachmentSizeData? Small { get; set; }

    [JsonPropertyName("focus")]
    public AttachmentFocusData? Focus { get; set; }
}

public class AttachmentSizeData
{

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }


    [JsonPropertyName("size")]
    public string? Size { get; set; }

    [JsonPropertyName("aspect")]
    public double? Aspect { get; set; }

    [JsonPropertyName("frame_rate")]
    public string? FrameRate { get; set; }

    [JsonPropertyName("duration")]
    public double? Duration { get; set; }

    [JsonPropertyName("bitrate")]
    public int? BitRate { get; set; }
}

public class AttachmentFocusData
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }
}
