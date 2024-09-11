using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mastonet.Entities
{
    public class CardAuthor
    {
        /// <summary>
        /// The original resource author’s name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A link to the author of the original resource.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// The fediverse account of the author.
        /// </summary>
        [JsonPropertyName("account")]
        public Account? Account { get; set; }
    }
}
