using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class NotificationTests : MastodonClientTests
    {
        [Fact]
        public async Task GetNotifications()
        {
            var testClient = GetTestClient();
            var privClient = GetPrivateClient();

            // Have 1 notif
            await testClient.ClearNotifications();
            await privClient.PostStatus("@TestAccount hello", Visibility.Direct);

            // Get notif
            var notifications = await testClient.GetNotifications();
            Assert.True(notifications.Any());
        }

        [Fact]
        public async Task GetNotification()
        {
            var testClient = GetTestClient();
            var privClient = GetPrivateClient();

            // Have 1 notif
            await testClient.ClearNotifications();
            await privClient.PostStatus("@TestAccount hello", Visibility.Direct);

            // Get notif
            var notifications = await testClient.GetNotifications();
            var notifId = notifications.First(n => n.Account.Id == 11).Id;

            var notification = await testClient.GetNotification(notifId);
            Assert.NotNull(notification);
            Assert.Equal(11, notification.Account.Id);
        }

        [Fact]
        public async Task ClearNotifications()
        {
            var testClient = GetTestClient();
            var privClient = GetPrivateClient();

            // Have notifs
            await privClient.PostStatus("@TestAccount hello", Visibility.Direct);
            var notifications = await testClient.GetNotifications();
            Assert.True(notifications.Any());

            // Clear notifs
            await testClient.ClearNotifications();

            notifications = await testClient.GetNotifications();
            Assert.False(notifications.Any());
        }
    }
}
