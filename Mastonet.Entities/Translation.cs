using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mastonet.Entities
{
    /// <summary>
    /// Represents the result of machine translating some status content
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// HTML-encoded translated content of the status.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The translated spoiler warning of the status.
        /// </summary>
        [JsonPropertyName("spoiler_text")]
        public string SpoilerText { get; set; } = string.Empty;

        /// <summary>
        /// The translated poll of the status.
        /// </summary>
        [JsonPropertyName("poll")]
        public TranslatedPoll? TranslatedPoll { get; set; }

        /// <summary>
        /// The translated media descriptions of the status.
        /// </summary>
        [JsonPropertyName("media_attachments")]
        public TranslatedAttachments? TranslatedAttachments { get; set; }

        /// <summary>
        /// The language of the source text, as auto-detected by the machine translation provider.
        /// </summary>
        [JsonPropertyName("detected_source_language")]
        public string DetectedSourceLanguage { get; set; } = string.Empty;

        /// <summary>
        /// The language of the source text, as auto-detected by the machine translation provider.
        /// </summary>
        [JsonPropertyName("provider")]
        public string provider { get; set; } = string.Empty;
    }

    public class TranslatedPoll
    {
        /// <summary>
        /// The ID of the poll.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The translated poll options.
        /// </summary>
        [JsonPropertyName("options")]
        public string Options { get; set; } = string.Empty;
    }

    public class TranslatedAttachments
    {
        /// <summary>
        /// The id of the attachment.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The translated description of the attachment.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
