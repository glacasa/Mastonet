using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mastonet;


[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(Status))]
[JsonSerializable(typeof(MastodonList<Status>))]
[JsonSerializable(typeof(Card))]
[JsonSerializable(typeof(Context))]
[JsonSerializable(typeof(Poll))]
[JsonSerializable(typeof(Account))]
[JsonSerializable(typeof(Conversation))]
[JsonSerializable(typeof(Filter))]
[JsonSerializable(typeof(IdentityProof))]
[JsonSerializable(typeof(Instance))]
[JsonSerializable(typeof(InstanceV2))]
[JsonSerializable(typeof(List))]
[JsonSerializable(typeof(List<Account>))]
[JsonSerializable(typeof(Marker))]
[JsonSerializable(typeof(Notification))]
[JsonSerializable(typeof(ScheduledStatus))]
[JsonSerializable(typeof(SearchResults))]
[JsonSerializable(typeof(AppRegistration))]
[JsonSerializable(typeof(Auth))]
[JsonSerializable(typeof(IEnumerable<Account>))]
[JsonSerializable(typeof(IEnumerable<Activity>))]
[JsonSerializable(typeof(IEnumerable<Announcement>))]
[JsonSerializable(typeof(IEnumerable<Emoji>))]
[JsonSerializable(typeof(IEnumerable<FeaturedTag>))]
[JsonSerializable(typeof(IEnumerable<Filter>))]
[JsonSerializable(typeof(IEnumerable<List>))]
[JsonSerializable(typeof(IEnumerable<Relationship>))]
[JsonSerializable(typeof(IEnumerable<ScheduledStatus>))]
[JsonSerializable(typeof(IEnumerable<string>))]
[JsonSerializable(typeof(IEnumerable<Tag>))]
internal partial class TryDeserializeContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(Error), GenerationMode = JsonSourceGenerationMode.Metadata)]
internal partial class ErrorContext : JsonSerializerContext
{
}