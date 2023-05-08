using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a rich preview card that is generated using OpenGraph tags from a URL.
/// </summary>
public class Card
{
    /// <summary>
    /// Location of linked resource.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Title of linked resource.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of preview.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The type of the preview card. One of :
    /// link = Link OEmbed
    /// photo = Photo OEmbed
    /// video = Video OEmbed
    /// rich = iframe OEmbed.Not currently accepted, so won't show up in practice.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The author of the original resource.
    /// </summary>
    [JsonProperty("author_name")]
    public string? AuthorName { get; set; }

    /// <summary>
    /// A link to the author of the original resource.
    /// </summary>
    [JsonProperty("author_url")]
    public string? AuthorUrl { get; set; }

    /// <summary>
    /// The provider of the original resource.
    /// </summary>
    [JsonProperty("provider_name")]
    public string? ProviderName { get; set; }

    /// <summary>
    /// A link to the provider of the original resource.
    /// </summary>
    [JsonProperty("provider_url")]
    public string? ProviderUrl { get; set; }

    /// <summary>
    /// HTML to be used for generating the preview card.
    /// </summary>
    [JsonProperty("html")]
    public string? Html { get; set; }

    /// <summary>
    /// Width of preview, in pixels.
    /// </summary>
    [JsonProperty("width")]
    public int? Width { get; set; }

    /// <summary>
    /// Height of preview, in pixels.
    /// </summary>
    [JsonProperty("height")]
    public int? Height { get; set; }

    /// <summary>
    /// Preview thumbnail.
    /// </summary>
    [JsonProperty("image")]
    public string? Image { get; set; }

    /// <summary>
    /// Used for photo embeds, instead of custom html.
    /// </summary>
    [JsonProperty("embed_url")]
    public string? EmbedUrl { get; set; }

    /// <summary>
    /// A hash computed by the BlurHash algorithm, for generating colorful preview thumbnails 
    /// when media has not been downloaded yet.
    /// </summary>
    [JsonProperty("blurhash")]
    public string? BlurHash { get; set; }
}
