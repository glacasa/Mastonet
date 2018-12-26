using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#filter
    public class Filter
    {
        /// <summary>
        /// The ID of the filter
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Keyword or phrase
        /// </summary>
        [JsonProperty("phrase")]
        public string Phrase { get; set; } = string.Empty;

        /// <summary>
        /// Array of strings that indicate filter context. each string is one of 'home', 'notifications', 'public', 'thread'
        /// </summary>
        [JsonProperty("context")]
        public IEnumerable<string> Context { get; set; } = new string[0];

        /// <summary>
        /// Date that indicates when this filter is expired.
        /// </summary>
        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Boolean that indicates irreversible server side filtering.
        /// </summary>
        [JsonProperty("irreversible")]
        public bool irreversible { get; set; }

        /// <summary>
        /// Boolean that indicates word match.
        /// If WholeWord is true , client app should do:
        /// - Define 'Word constituent character' for your app. In official implementation, it's [A-Za-z0-9_] for JavaScript, it's [[:word:]] for Ruby. In Ruby case it's POSIX character class (Letter | Mark | Decimal_Number | Connector_Punctuation).
        /// - If the phrase starts with word character, and if the previous character before matched range is word character, its matched range should treat to not match.
        /// - If the phrase ends with word character, and if the next character after matched range is word character, its matched range should treat to not match.
        /// </summary>
        [JsonProperty("whole_word")]
        public bool WholeWord { get; set; }
    }
}
