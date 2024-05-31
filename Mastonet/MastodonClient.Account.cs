using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mastonet;

partial class MastodonClient
{
    /// <summary>
    /// View information about a profile.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns an Account</returns>
    public Task<Account> GetAccount(string accountId)
    {
        return Get<Account>($"/api/v1/accounts/{accountId}");
    }

    /// <summary>
    /// Get the user's own Account with Source
    /// </summary>
    /// <returns>Returns the user's own Account with Source</returns>
    public Task<Account> GetCurrentUser()
    {
        return Get<Account>($"/api/v1/accounts/verify_credentials");
    }

    public Task<IdentityProof> GetIdentityProof()
    {
        return Get<IdentityProof>($"/api/proofs");
    }

    /// <summary>
    /// Update the user's display and preferences.
    /// </summary> 
    /// <param name="discoverable">Whether the account should be shown in the profile directory</param>
    /// <param name="bot">Whether the account has a bot flag</param>
    /// <param name="display_name">The display name to use for the profile</param>
    /// <param name="note">The account bio</param>
    /// <param name="avatar">Avatar image</param>
    /// <param name="header">Header image</param>
    /// <param name="locked">Whether manual approval of follow requests is required</param>
    /// <param name="source_privacy">Default post privacy for authored statuses</param>
    /// <param name="source_sensitive">Whether to mark authored statuses as sensitive by default</param>
    /// <param name="source_language">Default language to use for authored statuses (ISO6391)</param>
    /// <param name="fields_attributes">Profile metadata name and value. (By default, max 4 fields and 255 characters per property/value)</param>
    /// <returns>Returns the user's own Account with Source</returns>
    public Task<Account> UpdateCredentials(
        bool? discoverable = null,
        bool? bot = null,
        string? display_name = null,
        string? note = null,
        MediaDefinition? avatar = null,
        MediaDefinition? header = null,
        bool? locked = null,
        Visibility? source_privacy = null,
        bool? source_sensitive = null,
        string? source_language = null,
        IEnumerable<Field>? fields_attributes = null)
    {
        if (fields_attributes?.Count() > 4)
        {
            throw new ArgumentException("Number of fields must be 4 or fewer.", nameof(fields_attributes));
        }

        var data = new List<KeyValuePair<string, string>>();
        var media = new List<MediaDefinition>();

        if (discoverable != null)
        {
            data.Add(new KeyValuePair<string, string>("discoverable", discoverable.Value.ToString()));
        }

        if (bot != null)
        {
            data.Add(new KeyValuePair<string, string>("bot", bot.Value.ToString()));
        }

        if (display_name != null)
        {
            data.Add(new KeyValuePair<string, string>("display_name", display_name));
        }
        if (note != null)
        {
            data.Add(new KeyValuePair<string, string>("note", note));
        }

        if (avatar != null)
        {
            avatar.ParamName = "avatar";
            media.Add(avatar);
        }
        if (header != null)
        {
            header.ParamName = "header";
            media.Add(header);
        }
        if (locked.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("locked", locked.Value.ToString().ToLowerInvariant()));
        }
        if (source_privacy.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("source[privacy]", source_privacy.Value.ToString().ToLowerInvariant()));
        }
        if (source_sensitive.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("source[sensitive]", source_sensitive.Value.ToString().ToLowerInvariant()));
        }
        if (source_language != null)
        {
            data.Add(new KeyValuePair<string, string>("source[language]", source_language));
        }
        if (fields_attributes != null)
        {
            foreach (var item in fields_attributes.Select((f, i) => new { f, i }))
            {
                data.Add(new KeyValuePair<string, string>($"fields_attributes[{item.i}][name]", item.f.Name));
                data.Add(new KeyValuePair<string, string>($"fields_attributes[{item.i}][value]", item.f.Value));
            }
        }

        return Patch<Account>($"/api/v1/accounts/update_credentials", data, media);
    }

    /// <summary>
    /// Getting an account's relationships
    /// </summary>
    /// <param name="id">Account ID</param>
    /// <returns>Returns an array of Relationships of the current user to a given account</returns>
    public Task<IEnumerable<Relationship>> GetAccountRelationships(string id)
    {
        return GetAccountRelationships(new string[] { id });
    }

    /// <summary>
    /// Getting an account's relationships
    /// </summary>
    /// <param name="id">Account IDs</param>
    /// <returns>Returns an array of Relationships of the current user to a list of given accounts</returns>
    public Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<string> ids)
    {
        var data = new List<KeyValuePair<string, string>>();
        foreach (var id in ids)
        {
            data.Add(new KeyValuePair<string, string>("id[]", id.ToString()));
        }
        return Get<IEnumerable<Relationship>>("/api/v1/accounts/relationships", data);
    }

    /// <summary>
    /// Getting an account's followers
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    public Task<MastodonList<Account>> GetAccountFollowers(string accountId, ArrayOptions? options = null)
    {
        var url = $"/api/v1/accounts/{accountId}/followers";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }
        return GetMastodonList<Account>(url);
    }

    /// <summary>
    /// Getting who account is following
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    public Task<MastodonList<Account>> GetAccountFollowing(string accountId, ArrayOptions? options = null)
    {
        var url = $"/api/v1/accounts/{accountId}/following";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }
        return GetMastodonList<Account>(url);
    }

    /// <summary>
    /// Getting an account's statuses
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="onlyMedia">Only return statuses that have media attachments</param>
    /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
    /// <param name="pinned">Only return statuses that have been pinned</param>
    /// <param name="excludeReblogs">Skip statuses that are reblogs of other statuses</param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Statuses</returns>
    public Task<MastodonList<Status>> GetAccountStatuses(string accountId, ArrayOptions? options = null, bool onlyMedia = false, bool excludeReplies = false, bool pinned = false, bool excludeReblogs = false)
    {
        var url = $"/api/v1/accounts/{accountId}/statuses";

        string queryParams = "";
        if (onlyMedia)
        {
            queryParams = "?only_media=true";
        }
        if (pinned)
        {
            if (queryParams != "")
            {
                queryParams += "&";
            }
            else
            {
                queryParams += "?";
            }
            queryParams += "pinned=true";
        }
        if (excludeReplies)
        {
            if (queryParams != "")
            {
                queryParams += "&";
            }
            else
            {
                queryParams += "?";
            }
            queryParams += "exclude_replies=true";
        }
        if (excludeReblogs)
        {
            if (queryParams != "")
            {
                queryParams += "&";
            }
            else
            {
                queryParams += "?";
            }
            queryParams += "exclude_reblogs=true";
        }
        if (options != null)
        {
            if (queryParams != "")
            {
                queryParams += "&";
            }
            else
            {
                queryParams += "?";
            }
            queryParams += options.ToQueryString();
        }

        return GetMastodonList<Status>(url + queryParams);
    }


    #region Follow Requests

    /// <summary>
    /// Fetching a list of follow requests
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
    public Task<MastodonList<Account>> GetFollowRequests(ArrayOptions? options = null)
    {
        var url = "/api/v1/follow_requests";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }
        return this.GetMastodonList<Account>(url);
    }

    /// <summary>
    /// Authorizing follow requests
    /// </summary>
    /// <param name="accountId">The id of the account to authorize</param>
    public Task AuthorizeRequest(string accountId)
    {
        return this.Post($"/api/v1/follow_requests/{accountId}/authorize");
    }

    /// <summary>
    /// Rejecting follow requests
    /// </summary>
    /// <param name="accountId">The id of the account to reject</param>
    public Task RejectRequest(string accountId)
    {
        return this.Post($"/api/v1/follow_requests/{accountId}/reject");
    }

    #endregion

    #region Follow Suggestions
    /// <summary>
    /// Listing accounts the user had past positive interactions with, but is not following yet
    /// </summary>
    /// <returns>Returns array of Account</returns>
    public Task<IEnumerable<Account>> GetFollowSuggestions()
    {
        return Get<IEnumerable<Account>>("/api/v1/suggestions");
    }

    /// <summary>
    /// Removing account from suggestions
    /// </summary>
    /// <param name="accountId">The account ID to remove</param>
    public Task DeleteFollowSuggestion(string accountId)
    {
        return Delete($"/api/v1/suggestions/{accountId}");
    }
    #endregion

    #region Favourites

    /// <summary>
    /// Fetching a user's favourites
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
    public Task<MastodonList<Status>> GetFavourites(ArrayOptions? options = null)
    {
        var url = "/api/v1/favourites";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }
        return GetMastodonList<Status>(url);
    }

    #endregion


    #region Bookmarks 

    /// <summary>
    /// View your bookmarks.
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Statuses the user has bookmarked</returns>
    public Task<MastodonList<Status>> GetBookmarks(ArrayOptions? options = null)
    {
        var url = "/api/v1/bookmarks";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }
        return GetMastodonList<Status>(url);
    }

    #endregion

    #region Featured tags

    /// <summary>
    /// View your featured tags
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<FeaturedTag>> GetFeaturedTags()
    {
        return Get<IEnumerable<FeaturedTag>>("/api/v1/featured_tags");
    }

    /// <summary>
    /// Tags featured by this account
    /// </summary>
    /// <param name="id">The ID of the Account in the database</param>
    /// <returns></returns>
    public Task<IEnumerable<FeaturedTag>> GetAccountFeaturedTags(string id)
    {
        return Get<IEnumerable<FeaturedTag>>($"/api/v1/accounts/{id}/featured_tags");
    }

    /// <summary>
    /// Feature a tag
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<FeaturedTag> FeatureTag(string name)
    {
        var data = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("name",name)
        };

        return Post<FeaturedTag>("/api/v1/featured_tags", data);
    }

    /// <summary>
    /// Unfeature a tag
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task UnfeatureTag(string id)
    {
        return Delete("/api/v1/featured_tags/" + id);
    }

    /// <summary>
    /// Shows your 10 most-used tags, with usage history for the past week.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Tag>> GetFeaturedTagsSuggestions()
    {
        return Get<IEnumerable<Tag>>("/api/v1/featured_tags/suggestions");
    }

    #endregion
}
