using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mastonet.Entities;

[JsonSerializable(typeof(IEnumerable<string>))]
[JsonSerializable(typeof(List<string>))]
internal partial class EntitiesContext : JsonSerializerContext
{
}