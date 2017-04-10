# Masto.NET

Masto.NET is a .et standard library for Mastodon written in C#.

Work In Progress. You can try it, but be aware there's probably gonna be breaking changes until a stable version is available. Also, don't send Pull Request before opening an issue.

## How to use

You need to obtain a ClientId and a ClientSecret for your app, directly from the client, on the target Mastodon instance.
Call the static `CreateApp` method :

	  var appRegistration = await MastodonClient.CreateApp("instanceUrl", "Your app name", Scope.Read | Scope.Write | Scope.Follow);

The `appRegistration` object must be saved.

Now you can create a client, and connect the user (OAuth login to come soon) :

	var client = var client = new MastodonClient("instanceUrl", appRegistration);
	var auth = await client.Connect("email", "password");

Save this `auth` object, you will need it when you restart the app. You can create a client with the auth object :

	var client = var client = new MastodonClient("instanceUrl", appRegistration, auth);

Now you can call all the API methods. [See Mastodon API overview](https://github.com/tootsuite/mastodon/blob/master/docs/Using-the-API/API.md)