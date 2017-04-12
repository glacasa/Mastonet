# Masto.NET

Masto.NET is a .net standard library for Mastodon written in C#.

Work In Progress. You can try it, but be aware there's probably gonna be breaking changes until a stable version is available. Also, don't send Pull Request before opening an issue.

## How to use

You need to obtain a ClientId and a ClientSecret for your app, directly from the client, on the target Mastodon instance.
Call the static `CreateApp` method :

	  var appRegistration = await MastodonClient.CreateApp("instanceUrl", "Your app name", Scope.Read | Scope.Write | Scope.Follow);

The `appRegistration` object must be saved.

Now you can create a client, and connect the user (OAuth login to come soon) :

	var client = var client = new MastodonClient("instanceUrl", appRegistration);
	var auth = await client.Connect("email", "password");

Save this `auth` object, you will need it when you restart the app. You can create a client with the `auth` object :

	var client = var client = new MastodonClient("instanceUrl", appRegistration, auth);

Now you can call all the API methods. [See Mastodon API overview](https://github.com/tootsuite/mastodon/blob/master/docs/Using-the-API/API.md)

## Streaming

You can use the `TimelineStreaming` to be notified for every status, notification and deletion on a timeline.

	var client =  new MastodonClient("instance", appRegistration, auth);
	var streaming = client.GetUserStreaming();

	// Register events
	streaming.OnUpdate = OnStatusReceived;
	streaming.OnNotification = OnNotificationReceived;
	streaming.OnDelete = OnDeleteReceived;

	// Start streaming
	streaming.Start();


	// ...

	// Stop streaming
	streaming.Stop();

## Connection issues with .net framework

Some instances only accept TLS 1.2 requests, but .net Framework only support TLS 1.2 by default on version 4.6 and above
If you are on version 4.5.2 or earlier, you should force using TLS 1.2 by this line of code before any request :

	ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;