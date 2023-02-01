using Mastonet.Entities;
using System.Threading.Tasks;

namespace Mastonet;

public enum AdminAccountOrigin
{
    Local,
    Remote
}

public enum AdminAccountStatus
{
    Active,
    Pending,
    Disabled,
    Silenced,
    Suspended
}

public partial class MastodonClient
{
    public Task<MastodonList<AdminAccount>> GetAdminAccounts(ArrayOptions? options = null, AdminAccountOrigin? origin = null,
        AdminAccountStatus? status = null, string? permissions = null, string? invitedBy = null, string? username = null,
        string? displayName = null, string? byDomain = null, string? email = null, string? userIp = null)
    {
        const string url = "/api/v2/admin/accounts";
        var queryParams = "";
        queryParams = AddQueryStringParam(queryParams, "origin", origin?.ToString().ToLowerInvariant());
        queryParams = AddQueryStringParam(queryParams, "status", status?.ToString().ToLowerInvariant());
        queryParams = AddQueryStringParam(queryParams, "permissions", permissions);
        queryParams = AddQueryStringParam(queryParams, "invited_by", invitedBy);
        queryParams = AddQueryStringParam(queryParams, "username", username);
        queryParams = AddQueryStringParam(queryParams, "display_name", displayName);
        queryParams = AddQueryStringParam(queryParams, "by_domain", byDomain);
        queryParams = AddQueryStringParam(queryParams, "email", email);
        queryParams = AddQueryStringParam(queryParams, "ip", userIp);
        if (options != null)
        {
            var concatChar = GetQueryStringConcatChar(queryParams);
            queryParams += concatChar + options.ToQueryString();
        }

        return GetMastodonList<AdminAccount>(url + queryParams);
    }


}
