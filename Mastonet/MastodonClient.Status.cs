using System;
using System.Threading.Tasks;
using Mastonet.Entities;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace Mastonet;

partial class MastodonClient
{
    /// <summary>
    /// Fetching a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns a Status</returns>
    public Task<Status> GetStatus(string statusId)
    {
        return Get<Status>($"/api/v1/statuses/{statusId}");
    }

    /// <summary>
    /// Getting status context
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns a Context</returns>
    public Task<Context> GetStatusContext(string statusId)
    {
        return Get<Context>($"/api/v1/statuses/{statusId}/context");
    }

    /// <summary>
    /// Getting a card associated with a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns a Card</returns>
    public Task<Card> GetStatusCard(string statusId)
    {
        return Get<Card>($"/api/v1/statuses/{statusId}/card");
    }

    /// <summary>
    /// Getting who reblogged a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    public Task<MastodonList<Account>> GetRebloggedBy(string statusId, ArrayOptions? options = null)
    {
        var url = $"/api/v1/statuses/{statusId}/reblogged_by";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }

        return GetMastodonList<Account>(url);
    }

    /// <summary>
    /// Getting who favourited a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    public Task<MastodonList<Account>> GetFavouritedBy(string statusId, ArrayOptions? options = null)
    {
        var url = $"/api/v1/statuses/{statusId}/favourited_by";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }

        return GetMastodonList<Account>(url);
    }

    /// <summary>
    /// Posting a new status
    /// </summary>
    /// <param name="status">The text of the status</param>
    /// <param name="visibility">Either "direct", "private", "unlisted" or "public"</param>
    /// <param name="replyStatusId">Local ID of the status you want to reply to</param>
    /// <param name="mediaIds">Array of media IDs to attach to the status (maximum 4)</param>
    /// <param name="sensitive">Set this to mark the media of the status as NSFW</param>
    /// <param name="spoilerText">Text to be shown as a warning before the actual content</param>
    /// <param name="scheduledAt">DateTime to schedule posting of status</param>
    /// <param name="language">Override language code of the toot (ISO 639-2)</param>
    /// <param name="poll">Nested parameters to attach a poll to the status</param>
    /// <returns>Returns Status</returns>
    public Task<Status> PublishStatus(string status, Visibility? visibility = null, string? replyStatusId = null,
        IEnumerable<string>? mediaIds = null, bool sensitive = false, string? spoilerText = null,
        DateTime? scheduledAt = null, string? language = null, PollParameters? pollParameters = null)
    {
        if (string.IsNullOrEmpty(status) && (mediaIds == null || !mediaIds.Any()))
        {
            throw new ArgumentException("A status must have either text (status) or media (mediaIds)", nameof(status));
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("status", status),
        };

        if (!string.IsNullOrEmpty(replyStatusId))
        {
            data.Add(new KeyValuePair<string, string>("in_reply_to_id", replyStatusId!));
        }

        if (mediaIds != null)
        {
            foreach (var mediaId in mediaIds)
            {
                data.Add(new KeyValuePair<string, string>("media_ids[]", mediaId));
            }
        }

        if (sensitive)
        {
            data.Add(new KeyValuePair<string, string>("sensitive", "true"));
        }

        if (spoilerText != null)
        {
            data.Add(new KeyValuePair<string, string>("spoiler_text", spoilerText));
        }

        if (visibility.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("visibility", visibility.Value.ToString().ToLowerInvariant()));
        }

        if (scheduledAt.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("scheduled_at", scheduledAt.Value.ToString("o")));
        }

        if (language != null)
        {
            data.Add(new KeyValuePair<string, string>("language", language));
        }

        if (pollParameters != null)
        {
            data.AddRange(pollParameters.Options.Select(option => new KeyValuePair<string, string>("poll[options][]", option)));
            data.Add(new KeyValuePair<string, string>("poll[expires_in]", pollParameters.ExpiresIn.TotalSeconds.ToString()));
            if (pollParameters.Multiple.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[multiple]", pollParameters.Multiple.Value.ToString().ToLowerInvariant()));
            }

            if (pollParameters.HideTotals.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[hide_totals]", pollParameters.HideTotals.Value.ToString().ToLowerInvariant()));
            }
        }

        return Post<Status>("/api/v1/statuses", data);
    }

    /// <summary>
    /// Posting a new status
    /// </summary>
    /// <param name="statusParameters">A set of parameters to upload the status</param>
    /// <returns>Returns Status</returns>
    public Task<Status> PublishStatus(StatusParameters statusParameters)
    {
        if (string.IsNullOrEmpty(statusParameters.Status) && (statusParameters.MediaIds == null || !statusParameters.MediaIds.Any()))
        {
            throw new ArgumentException("A status must have either text (status) or media (mediaIds)", nameof(statusParameters.Status));
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("status", statusParameters.Status!),
        };

        if (!string.IsNullOrEmpty(statusParameters.ReplyStatusId))
        {
            data.Add(new KeyValuePair<string, string>("in_reply_to_id", statusParameters.ReplyStatusId!));
        }

        if (statusParameters.MediaIds != null)
        {
            foreach (var mediaId in statusParameters.MediaIds)
            {
                data.Add(new KeyValuePair<string, string>("media_ids[]", mediaId));
            }
        }

        if (statusParameters.Sensitive)
        {
            data.Add(new KeyValuePair<string, string>("sensitive", "true"));
        }

        if (statusParameters.SpoilerText != null)
        {
            data.Add(new KeyValuePair<string, string>("spoiler_text", statusParameters.SpoilerText));
        }

        if (statusParameters.Visibility.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("visibility", statusParameters.Visibility.Value.ToString().ToLowerInvariant()));
        }

        if (statusParameters.ScheduledAt.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("scheduled_at", statusParameters.ScheduledAt.Value.ToString("o")));
        }

        if (statusParameters.Language != null)
        {
            data.Add(new KeyValuePair<string, string>("language", statusParameters.Language));
        }

        if (statusParameters.PollParameters != null)
        {
            data.AddRange(statusParameters.PollParameters.Options.Select(option => new KeyValuePair<string, string>("poll[options][]", option)));
            data.Add(new KeyValuePair<string, string>("poll[expires_in]", statusParameters.PollParameters.ExpiresIn.TotalSeconds.ToString()));
            if (statusParameters.PollParameters.Multiple.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[multiple]", statusParameters.PollParameters.Multiple.Value.ToString().ToLowerInvariant()));
            }

            if (statusParameters.PollParameters.HideTotals.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[hide_totals]", statusParameters.PollParameters.HideTotals.Value.ToString().ToLowerInvariant()));
            }
        }

        return Post<Status>("/api/v1/statuses", data);
    }

    /// <summary>
    /// Edit a status
    /// </summary>
    /// <param name="statusId">The Id of the status you want to edit</param>
    /// <param name="status">The text of the status</param>
    /// <param name="mediaIds">Array of media IDs to attach to the status (maximum 4)</param>
    /// <param name="sensitive">Set this to mark the media of the status as NSFW</param>
    /// <param name="spoilerText">Text to be shown as a warning before the actual content</param>
    /// <param name="language">Override language code of the toot (ISO 639-2)</param>
    /// <param name="poll">Nested parameters to attach a poll to the status</param>
    /// <returns>Returns Status</returns>
    public Task<Status> EditStatus(string statusId, string status, IEnumerable<string>? mediaIds = null,
        bool sensitive = false, string? spoilerText = null, string? language = null, PollParameters? poll = null)
    {
        if (string.IsNullOrEmpty(status) && (mediaIds == null || !mediaIds.Any()))
        {
            throw new ArgumentException("A status must have either text (status) or media (mediaIds)", nameof(status));
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("status", status),
        };

        if (mediaIds != null)
        {
            foreach (var mediaId in mediaIds)
            {
                data.Add(new KeyValuePair<string, string>("media_ids[]", mediaId.ToString()));
            }
        }

        if (sensitive)
        {
            data.Add(new KeyValuePair<string, string>("sensitive", "true"));
        }

        if (spoilerText != null)
        {
            data.Add(new KeyValuePair<string, string>("spoiler_text", spoilerText));
        }

        if (language != null)
        {
            data.Add(new KeyValuePair<string, string>("language", language));
        }

        if (poll != null)
        {
            data.AddRange(poll.Options.Select(option => new KeyValuePair<string, string>("poll[options][]", option)));
            data.Add(new KeyValuePair<string, string>("poll[expires_in]", poll.ExpiresIn.TotalSeconds.ToString()));
            if (poll.Multiple.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[multiple]", poll.Multiple.Value.ToString()));
            }

            if (poll.HideTotals.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[hide_totals]", poll.HideTotals.Value.ToString()));
            }
        }

        return Put<Status>("/api/v1/statuses/" + statusId, data);
    }

    /// <summary>
    /// Edit a status
    /// </summary>
    /// <param name="statusId">The Id of the status you want to edit</param>
    /// <param name="statusParameters">A set of parameters to edit the status</param>
    /// <returns>Returns Status</returns>
    public Task<Status> EditStatus(string statusId, StatusParameters statusParameters)
    {
        if (string.IsNullOrEmpty(statusParameters.Status) && (statusParameters.MediaIds == null || !statusParameters.MediaIds.Any()))
        {
            throw new ArgumentException("A status must have either text (status) or media (mediaIds)", nameof(statusParameters.Status));
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("status", statusParameters.Status!),
        };

        if (statusParameters.MediaIds != null)
        {
            foreach (var mediaId in statusParameters.MediaIds)
            {
                data.Add(new KeyValuePair<string, string>("media_ids[]", mediaId.ToString()));
            }
        }

        if (statusParameters.Sensitive)
        {
            data.Add(new KeyValuePair<string, string>("sensitive", "true"));
        }

        if (statusParameters.SpoilerText != null)
        {
            data.Add(new KeyValuePair<string, string>("spoiler_text", statusParameters.SpoilerText));
        }

        if (statusParameters.Language != null)
        {
            data.Add(new KeyValuePair<string, string>("language", statusParameters.Language));
        }

        if (statusParameters.PollParameters != null)
        {
            data.AddRange(statusParameters.PollParameters.Options.Select(option => new KeyValuePair<string, string>("poll[options][]", option)));
            data.Add(new KeyValuePair<string, string>("poll[expires_in]", statusParameters.PollParameters.ExpiresIn.TotalSeconds.ToString()));
            if (statusParameters.PollParameters.Multiple.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[multiple]", statusParameters.PollParameters.Multiple.Value.ToString()));
            }

            if (statusParameters.PollParameters.HideTotals.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("poll[hide_totals]", statusParameters.PollParameters.HideTotals.Value.ToString()));
            }
        }

        return Put<Status>("/api/v1/statuses/" + statusId, data);
    }


    /// <summary>
    /// Deleting a status
    /// </summary>
    /// <param name="statusId"></param>
    public Task DeleteStatus(string statusId)
    {
        return Delete($"/api/v1/statuses/{statusId}");
    }

    /// <summary>
    /// Get scheduled statuses.
    /// </summary>
    /// <returns>Returns array of ScheduledStatus</returns>
    public Task<IEnumerable<ScheduledStatus>> GetScheduledStatuses()
    {
        return Get<IEnumerable<ScheduledStatus>>("/api/v1/scheduled_statuses");
    }

    /// <summary>
    /// Get scheduled status.
    /// </summary>
    /// <param name="scheduledStatusId"></param>
    /// <returns>Returns ScheduledStatus</returns>
    public Task<ScheduledStatus> GetScheduledStatus(string scheduledStatusId)
    {
        return Get<ScheduledStatus>("/api/v1/scheduled_statuses/" + scheduledStatusId);
    }

    /// <summary>
    /// Update Scheduled status. Only scheduled_at can be changed. To change the content, delete it and post a new status.
    /// </summary>
    /// <param name="scheduledStatusId"></param>
    /// <param name="scheduledAt">DateTime to schedule posting of status</param>
    /// <returns>Returns ScheduledStatus</returns>
    public Task<ScheduledStatus> UpdateScheduledStatus(string scheduledStatusId, DateTime? scheduledAt)
    {
        var data = new List<KeyValuePair<string, string>>();
        if (scheduledAt.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("scheduled_at", scheduledAt.Value.ToString()));
        }

        return Put<ScheduledStatus>("/api/v1/scheduled_statuses/" + scheduledStatusId, data);
    }

    /// <summary>
    /// Remove Scheduled status.
    /// </summary>
    /// <param name="scheduledStatusId"></param>
    public Task DeleteScheduledStatus(string scheduledStatusId)
    {
        return Delete("/api/v1/scheduled_statuses/" + scheduledStatusId);
    }

    /// <summary>
    /// Reblogging a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> Reblog(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/reblog");
    }

    /// <summary>
    /// Unreblogging a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> Unreblog(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/unreblog");
    }

    /// <summary>
    /// Favouriting a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> Favourite(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/favourite");
    }

    /// <summary>
    /// Unfavouriting a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> Unfavourite(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/unfavourite");
    }

    /// <summary>
    /// Privately bookmark a status
    /// </summary>
    /// <param name="statusId">The ID of the Status in the database</param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> Bookmark(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/bookmark");
    }

    /// <summary>
    /// Remove a status from your private bookmarks
    /// </summary>
    /// <param name="statusId">The ID of the Status in the database</param>
    /// <returns>Returns the target Status</returns>
    public  Task<Status> Unbookmark(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/unbookmark");
    }

    /// <summary>
    /// Muting a conversation of a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> MuteConversation(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/mute");
    }

    /// <summary>
    /// Unmuting a conversation of a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> UnmuteConversation(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/unmute");
    }

    /// <summary>
    /// Pinning a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> Pin(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/pin");
    }

    /// <summary>
    /// Unpinning a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    public Task<Status> Unpin(string statusId)
    {
        return Post<Status>($"/api/v1/statuses/{statusId}/unpin");
    }
}