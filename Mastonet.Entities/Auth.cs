using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

public class Auth
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;
}
