using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#card
    public class Card
    {
        /// <summary>
        /// The url associated with the card
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// The title of the card
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The card description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The image associated with the card, if any
        /// </summary>
        [JsonProperty("image")]
        public string? Image { get; set; }

        /// <summary>
        /// "link", "photo", "video", or "rich"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// OEmbed data
        /// </summary>
        [JsonProperty("author_name")]
        public string? AuthorName { get; set; }

        /// <summary>
        /// OEmbed data
        /// </summary>
        [JsonProperty("author_url")]
        public string? AuthorUrl { get; set; }

        /// <summary>
        /// OEmbed data
        /// </summary>
        [JsonProperty("provider_name")]
        public string? ProviderName { get; set; }

        /// <summary>
        /// OEmbed data
        /// </summary>
        [JsonProperty("provider_url")]
        public string? ProviderUrl { get; set; }

        /// <summary>
        /// OEmbed data
        /// </summary>
        [JsonProperty("html")]
        public string? Html { get; set; }

        /// <summary>
        /// OEmbed data
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; set; }

        /// <summary>
        /// OEmbed data
        /// </summary>
        [JsonProperty("height")]
        public int? Height { get; set; }
    }
}
