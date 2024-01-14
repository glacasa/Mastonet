using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Mastonet.Entities;

public class InstanceConfiguration
{
    /// <summary>
    /// URLs of interest for clients apps.
    /// </summary>
    [JsonPropertyName("urls")]
    public InstanceConfigurationUrls Urls { get; set; }= new InstanceConfigurationUrls();

    /// <summary>
    /// Limits related to accounts.
    /// </summary>
    [JsonPropertyName("accounts")]
    public InstanceConfigurationAccounts Accounts { get; set; }= new InstanceConfigurationAccounts();

    /// <summary>
    /// Limits related to authoring statuses.
    /// </summary>
    [JsonPropertyName("statuses")]
    public InstanceConfigurationStatuses Statuses { get; set; }= new InstanceConfigurationStatuses();

    /// <summary>
    /// Hints for which attachments will be accepted.
    /// </summary>
    [JsonPropertyName("media_attachments")]
    public InstanceConfigurationMediaAttachments MediaAttachments { get; set; }= new InstanceConfigurationMediaAttachments();

    /// <summary>
    /// Limits related to polls.
    /// </summary>
    [JsonPropertyName("polls")]
    public InstanceConfigurationPolls Polls { get; set; }= new InstanceConfigurationPolls();

    /// <summary>
    /// Hints related to translation.
    /// </summary>
    [JsonPropertyName("translation")]
    public InstanceConfigurationTranslation Translation { get; set; }= new InstanceConfigurationTranslation();
}

public class InstanceConfigurationUrls
{
    /// <summary>
    /// The Websockets URL for connecting to the streaming API.
    /// </summary>
    [JsonPropertyName("streaming")]
    public string Streaming { get; set; }= string.Empty;
}

public class InstanceConfigurationAccounts
{
    /// <summary>
    /// The maximum number of featured tags allowed for each account.
    /// </summary>
    [JsonPropertyName("max_featured_tags")]
    public int MaxFeaturedTags { get; set; }
}

public class InstanceConfigurationStatuses
{
    /// <summary>
    /// The maximum number of allowed characters per status.
    /// </summary>
    [JsonPropertyName("max_characters")]
    public int MaxCharacters { get; set; }

    /// <summary>
    /// The maximum number of media attachments that can be added to a status.
    /// </summary>
    [JsonPropertyName("max_media_attachments")]
    public int MaxMediaAttachments { get; set; }

    /// <summary>
    /// Each URL in a status will be assumed to be exactly this many characters.
    /// </summary>
    [JsonPropertyName("characters_reserved_per_url")]
    public int CharactersReservedPerUrl { get; set; }

    /// <summary>
    /// Contains MIME types that can be used (available on some non-Mastodon instances)
    /// </summary>
    [JsonPropertyName("supported_mime_types")]
    public IEnumerable<string>? SupportedMimeTypes { get; set; } 
}

public class InstanceConfigurationMediaAttachments
{
    /// <summary>
    /// Contains MIME types that can be uploaded.
    /// </summary>
    [JsonPropertyName("supported_mime_types")]
    public IEnumerable<string> SupportedMimeTypes { get; set; }= Enumerable.Empty<string>();

    /// <summary>
    /// The maximum size of any uploaded image, in bytes.
    /// </summary>
    [JsonPropertyName("image_size_limit")]
    public int ImageSizeLimit { get; set; }

    /// <summary>
    /// The maximum number of pixels (width times height) for image uploads.
    /// </summary>
    [JsonPropertyName("image_matrix_limit")]
    public int ImageMatrixLimit { get; set; }

    /// <summary>
    /// The maximum size of any uploaded video, in bytes.
    /// </summary>
    [JsonPropertyName("video_size_limit")]
    public int VideoSizeLimit { get; set; }

    /// <summary>
    /// The maximum frame rate for any uploaded video.
    /// </summary>
    [JsonPropertyName("video_frame_rate_limit")]
    public int VideoFrameRateLimit { get; set; }

    /// <summary>
    /// The maximum number of pixels (width times height) for video uploads.
    /// </summary>
    [JsonPropertyName("video_matrix_limit")]
    public int VideoMatrixLimit { get; set; }
}

public class InstanceConfigurationPolls
{
    /// <summary>
    /// Each poll is allowed to have up to this many options.
    /// </summary>
    [JsonPropertyName("max_options")]
    public int MaxOptions { get; set; }

    /// <summary>
    /// Each poll option is allowed to have this many characters.
    /// </summary>
    [JsonPropertyName("max_characters_per_option")]
    public int MaxCharactersPerOption { get; set; }

    /// <summary>
    /// The shortest allowed poll duration, in seconds.
    /// </summary>
    [JsonPropertyName("min_expiration")]
    public int MinExpiration { get; set; }

    /// <summary>
    /// The longest allowed poll duration, in seconds.
    /// </summary>
    [JsonPropertyName("max_expiration")]
    public int MaxExpiration { get; set; }
}

public class InstanceConfigurationTranslation
{
    /// <summary>
    /// Whether the Translations API is available on this instance.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}