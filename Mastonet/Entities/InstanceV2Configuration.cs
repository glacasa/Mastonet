using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mastonet.Entities;

public class InstanceConfiguration
{
    /// <summary>
    /// URLs of interest for clients apps.
    /// </summary>
    [JsonProperty("urls")]
    public InstanceConfigurationUrls Urls { get; set; }

    /// <summary>
    /// Limits related to accounts.
    /// </summary>
    [JsonProperty("accounts")]
    public InstanceConfigurationAccounts Accounts { get; set; }

    /// <summary>
    /// Limits related to authoring statuses.
    /// </summary>
    [JsonProperty("statuses")]
    public InstanceConfigurationStatutes Statutes { get; set; }

    /// <summary>
    /// Hints for which attachments will be accepted.
    /// </summary>
    [JsonProperty("media_attachments")]
    public InstanceConfigurationMediaAttachments MediaAttachments { get; set; }

    /// <summary>
    /// Limits related to polls.
    /// </summary>
    [JsonProperty("polls")]
    public InstanceConfigurationPolls Polls { get; set; }

    /// <summary>
    /// Hints related to translation.
    /// </summary>
    [JsonProperty("translation")]
    public InstanceConfigurationTranslation Translation { get; set; }
}

public class InstanceConfigurationUrls
{
    /// <summary>
    /// The Websockets URL for connecting to the streaming API.
    /// </summary>
    [JsonProperty("streaming")]
    public string Streaming { get; set; }
}

public class InstanceConfigurationAccounts
{
    /// <summary>
    /// The maximum number of featured tags allowed for each account.
    /// </summary>
    [JsonProperty("max_featured_tags")]
    public int MaxFeaturedTags { get; set; }
}

public class InstanceConfigurationStatutes
{
    /// <summary>
    /// The maximum number of allowed characters per status.
    /// </summary>
    [JsonProperty("max_characters")]
    public int MaxCharacters { get; set; }

    /// <summary>
    /// The maximum number of media attachments that can be added to a status.
    /// </summary>
    [JsonProperty("max_media_attachments")]
    public int MaxMediaAttachments { get; set; }

    /// <summary>
    /// Each URL in a status will be assumed to be exactly this many characters.
    /// </summary>
    [JsonProperty("characters_reserved_per_url")]
    public int CharactersReservedPerUrl { get; set; }
}

public class InstanceConfigurationMediaAttachments
{
    /// <summary>
    /// Contains MIME types that can be uploaded.
    /// </summary>
    [JsonProperty("supported_mime_types")]
    public IEnumerable<string> SupportedMimeTypes { get; set; }

    /// <summary>
    /// The maximum size of any uploaded image, in bytes.
    /// </summary>
    [JsonProperty("image_size_limit")]
    public int ImageSizeLimit { get; set; }

    /// <summary>
    /// The maximum number of pixels (width times height) for image uploads.
    /// </summary>
    [JsonProperty("image_matrix_limit")]
    public int ImageMatrixLimit { get; set; }

    /// <summary>
    /// The maximum size of any uploaded video, in bytes.
    /// </summary>
    [JsonProperty("video_size_limit")]
    public int VideoSizeLimit { get; set; }

    /// <summary>
    /// The maximum frame rate for any uploaded video.
    /// </summary>
    [JsonProperty("video_frame_rate_limit")]
    public int VideoFrameRateLimit { get; set; }

    /// <summary>
    /// The maximum number of pixels (width times height) for video uploads.
    /// </summary>
    [JsonProperty("video_matrix_limit")]
    public int VideoMatrixLimit { get; set; }
}

public class InstanceConfigurationPolls
{
    /// <summary>
    /// Each poll is allowed to have up to this many options.
    /// </summary>
    [JsonProperty("max_options")]
    public int MaxOptions { get; set; }

    /// <summary>
    /// Each poll option is allowed to have this many characters.
    /// </summary>
    [JsonProperty("max_characters_per_option")]
    public int MaxCharactersPerOption { get; set; }

    /// <summary>
    /// The shortest allowed poll duration, in seconds.
    /// </summary>
    [JsonProperty("min_expiration")]
    public int MinExpiration { get; set; }

    /// <summary>
    /// The longest allowed poll duration, in seconds.
    /// </summary>
    [JsonProperty("max_expiration")]
    public int MaxExpiration { get; set; }
}

public class InstanceConfigurationTranslation
{
    /// <summary>
    /// Whether the Translations API is available on this instance.
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
}