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
    #region Follow

    /// <summary>
    /// Following an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="reblogs">Whether the followed account’s reblogs will show up in the home timeline</param>
    /// <returns>Returns the target Account</returns>
    public Task<Relationship> Follow(string accountId, bool reblogs = true, bool notify = false)
    {
        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("reblogs", reblogs.ToString()),
            new KeyValuePair<string, string>("notify", notify.ToString().ToLowerInvariant())
        };
        return this.Post<Relationship>($"/api/v1/accounts/{accountId}/follow", data);
    }

    /// <summary>
    /// Unfollowing an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the target Account</returns>
    public Task<Relationship> Unfollow(string accountId)
    {
        return this.Post<Relationship>($"/api/v1/accounts/{accountId}/unfollow");
    }

    /// <summary>
    /// Following a remote user
    /// </summary>
    /// <param name="uri">username@domain of the person you want to follow</param>
    /// <returns>Returns the local representation of the followed account, as an Account</returns>
    public Task<Account> Follow(string uri)
    {
        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("uri", uri)
        };
        return this.Post<Account>($"/api/v1/follows", data);
    }

    #endregion

    #region Block

    /// <summary>
    /// Blocking an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the target Account</returns>
    public Task<Relationship> Block(string accountId)
    {
        return Post<Relationship>($"/api/v1/accounts/{accountId}/block");
    }

    /// <summary>
    /// Unblocking an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the target Account</returns>
    public Task<Relationship> Unblock(string accountId)
    {
        return Post<Relationship>($"/api/v1/accounts/{accountId}/unblock");
    }

    /// <summary>
    /// Fetching a user's blocks
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
    public Task<MastodonList<Account>> GetBlocks(ArrayOptions? options = null)
    {
        var url = "/api/v1/blocks";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }

        return GetMastodonList<Account>(url);
    }

    #endregion

    #region Mutes

    /// <summary>
    /// Muting an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="notifications">Whether the mute will mute notifications or not</param>
    /// <returns>Returns the target Account</returns>
    public Task<Relationship> Mute(string accountId, bool notifications = true)
    {
        var data = notifications ? null : new[] { new KeyValuePair<string, string>("notifications", "false") };
        return Post<Relationship>($"/api/v1/accounts/{accountId}/mute", data);
    }

    /// <summary>
    /// Unmuting an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the target Account</returns>
    public Task<Relationship> Unmute(string accountId)
    {
        return Post<Relationship>($"/api/v1/accounts/{accountId}/unmute");
    }

    /// <summary>
    /// Fetching a user's mutes
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
    public Task<MastodonList<Account>> GetMutes(ArrayOptions? options = null)
    {
        var url = "/api/v1/mutes";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }

        return GetMastodonList<Account>(url);
    }

    #endregion

    #region Endorsements

    /// <summary>
    /// Getting accounts the user chose to endorse
    /// </summary>
    /// <returns>Returns an array of Accounts endorsed by the authenticated user</returns>
    public Task<MastodonList<Account>> GetEndorsements()
    {
        return GetMastodonList<Account>("/api/v1/endorsements");
    }

    /// <summary>
    /// Endorsing an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the updated Relationships with the target Account</returns>
    public Task<Relationship> Endorse(string accountId)
    {
        return Post<Relationship>($"/api/v1/accounts/{accountId}/pin");
    }

    /// <summary>
    /// Undoing endorse of an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the updated Relationships with the target Account</returns>
    public Task<Relationship> Unendorse(string accountId)
    {
        return Post<Relationship>($"/api/v1/accounts/{accountId}/unpin");
    }

    /// <summary>
    /// Get saved timeline position
    /// </summary>
    /// <param name="home"></param>
    /// <param name="notifications"></param>
    /// <returns></returns>
    public Task<Marker> GetMarkers(bool home = false, bool notifications = false)
    {
        var data = new List<KeyValuePair<string, string>>();

        if (home)
        {
            data.Add(new KeyValuePair<string, string>("timeline[]", "home"));
        }

        if (notifications)
        {
            data.Add(new KeyValuePair<string, string>("timeline[]", "notifications"));
        }

        return Get<Marker>("/api/v1/markers", data);
    }

    /// <summary>
    /// Save position in timeline
    /// </summary>
    /// <param name="homeLastReadId"></param>
    /// <param name="notificationLastReadId"></param>
    /// <returns></returns>
    public Task<Marker> SetMarkers(string? homeLastReadId = null, string? notificationLastReadId = null)
    {
        var data = new List<KeyValuePair<string, string>>();

        if (!string.IsNullOrEmpty(homeLastReadId))
        {
            data.Add(new KeyValuePair<string, string>("home[last_read_id]", homeLastReadId!));
        }

        if (!string.IsNullOrEmpty(notificationLastReadId))
        {
            data.Add(new KeyValuePair<string, string>("notifications[last_read_id]", notificationLastReadId!));
        }

        return Post<Marker>("/api/v1/markers", data);
    }

    #endregion

    #region Domain blocks

    /// <summary>
    /// Fetching a user's blocked domains
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of strings</returns>
    public Task<MastodonList<string>> GetDomainBlocks(ArrayOptions? options = null)
    {
        var url = "/api/v1/domain_blocks";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }

        return GetMastodonList<string>(url);
    }

    /// <summary>
    /// Block a domain
    /// </summary>
    /// <param name="domain">Domain to block</param>
    public Task BlockDomain(string domain)
    {
        var url = "/api/v1/domain_blocks?domain=" + Uri.EscapeDataString(domain);
        return Post(url);
    }

    /// <summary>
    /// Unblock a domain
    /// </summary>
    /// <param name="domain">Domain to block</param>
    public Task UnblockDomain(string domain)
    {
        var url = "/api/v1/domain_blocks?domain=" + Uri.EscapeDataString(domain);
        return Delete(url);
    }

    #endregion

    #region Tags

    /// <summary>
    /// View information about a single tag
    /// </summary>
    /// <param name="tag">The name of the hashtag</param>
    /// <returns></returns>
    public Task<Tag> GetTagInfo(string tag)
    {
        return Get<Tag>("/api/v1/tags/" + tag);
    }

    /// <summary>
    /// Follow a hashtag
    /// </summary>
    /// <param name="tag">The name of the hashtag</param>
    /// <returns></returns>
    public Task<Tag> FollowTag(string tag)
    {
        return Post<Tag>($"/api/v1/tags/{tag}/follow");
    }

    /// <summary>
    /// Unfollow a hashtag
    /// </summary>
    /// <param name="tag">The name of the hashtag</param>
    /// <returns></returns>
    public Task<Tag> UnfollowTag(string tag)
    {
        return Post<Tag>($"/api/v1/tags/{tag}/unfollow");
    }

    /// <summary>
    /// View all followed tags
    /// </summary>
    /// <returns></returns>
    public Task<MastodonList<Tag>> ViewFollowedTags(ArrayOptions? options = null)
    {
        string url = "/api/v1/followed_tags";

        var queryParams = "";
        if (options != null)
        {
            queryParams = "?" + options.ToQueryString();
        }

        return GetMastodonList<Tag>(url + queryParams);
    }

    #endregion
}