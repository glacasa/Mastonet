using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet;

public enum GranularScope
{
    Read,
    Read__Accounts,
    Read__Blocks,
    Read__Bookmarks,
    Read__Favourites,
    Read__Filters,
    Read__Follows,
    Read__Lists,
    Read__Mutes,
    Read__Notifications,
    Read__Search,
    Read__Statuses,

    Write,
    Write__Accounts,
    Write__Blocks,
    Write__Bookmarks,
    Write__Conversations,
    Write__Favourites,
    Write__Filters,
    Write__Follows,
    Write__Lists,
    Write__Media,
    Write__Mutes,
    Write__Notifications,
    Write__Reports,
    Write__Statuses,

    Push,

    Admin__Read,
    Admin__Read__Accounts,
    Admin__Read__Reports,
    Admin__Read__Domain_Allows,
    Admin__Read__Domain_Blocks,
    Admin__Read__Ip_Blocks,
    Admin__Read__Email_Domain_Blocks,
    Admin__Read__Canonical_Email_Blocks,
         
    Admin__Write,
    Admin__Write__Accounts,
    Admin__Write__Reports,
    Admin__Write__Domain_Allows,
    Admin__Write__Domain_Blocks,
    Admin__Write__Ip_Blocks,
    Admin__Write__Email_Domain_Blocks,
    Admin__Write__Canonical_Email_Blocks,

}