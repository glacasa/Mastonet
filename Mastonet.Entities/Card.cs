using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mastonet.Entities;

/// <summary>
/// Represents a rich preview card that is generated using OpenGraph tags from a URL.
/// </summary>
public class Card
{
    /// <summary>
    /// Location of linked resource.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Title of linked resource.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of preview.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The type of the preview card. One of :
    /// link = Link OEmbed
    /// photo = Photo OEmbed
    /// video = Video OEmbed
    /// rich = iframe OEmbed.Not currently accepted, so won't show up in practice.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Fediverse account of the authors of the original resource.
    /// </summary>
    [JsonPropertyName("authors")]
    public IEnumerable<CardAuthor> Authors { get; set; } = Enumerable.Empty<CardAuthor>();

    /// <summary>
    /// The author of the original resource.
    /// </summary>
    [JsonPropertyName("author_name")]
    [Obsolete("This property was deprecated in Mastodon v4.3. Use the Authors property instead.")]
    public string? AuthorName { get; set; }

    /// <summary>
    /// A link to the author of the original resource.
    /// </summary>
    [JsonPropertyName("author_url")]
    [Obsolete("This property was deprecated in Mastodon v4.3. Use the Authors property instead.")]
    public string? AuthorUrl { get; set; }

    /// <summary>
    /// The provider of the original resource.
    /// </summary>
    [JsonPropertyName("provider_name")]
    public string? ProviderName { get; set; }

    /// <summary>
    /// A link to the provider of the original resource.
    /// </summary>
    [JsonPropertyName("provider_url")]
    public string? ProviderUrl { get; set; }

    /// <summary>
    /// HTML to be used for generating the preview card.
    /// </summary>
    [JsonPropertyName("html")]
    public string? Html { get; set; }

    /// <summary>
    /// Width of preview, in pixels.
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <summary>
    /// Height of preview, in pixels.
    /// </summary>
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <summary>
    /// Preview thumbnail.
    /// </summary>
    [JsonPropertyName("image")]
    public string? Image { get; set; }

    /// <summary>
    /// Used for photo embeds, instead of custom html.
    /// </summary>
    [JsonPropertyName("embed_url")]
    public string? EmbedUrl { get; set; }

    /// <summary>
    /// A hash computed by the BlurHash algorithm, for generating colorful preview thumbnails 
    /// when media has not been downloaded yet.
    /// </summary>
    [JsonPropertyName("blurhash")]
    public string? BlurHash { get; set; }
}
