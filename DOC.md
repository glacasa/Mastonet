# How to use

## App registration

You need to obtain a ClientId and a ClientSecret for your app, directly from the client, on the target Mastodon instance.
Call the `CreateApp` method :
```cs
var instance = "example.org";
var authClient = new AuthenticationClient(instance);
var appRegistration = await authClient.CreateApp("Your app name", Scope.Read | Scope.Write | Scope.Follow);
```
The `appRegistration` object must be saved.

## User login, using e-mail and password

Now you can connect the user (not recommended, prefer OAuth when you can) :
```cs
var auth = await authClient.ConnectWithPassword("email", "password");
```
## User login, using OAuth

The recommended way to login is to use OAuth. You open a web browser and let the user log himself on his instance. 
```cs
var url = authClient.OAuthUrl();
OpenBrowser(url);
```
You can either embed a WebView in your app, or open an external browser. When the user allowed your app to access their account, he is redirected to a web page with an auth code.

You have several option to get the code :

  - Ask the user to copy and paste it in your app (easy for you, but not user-friendly)
  - If you have embedded a WebView in your app, you can read the final page. The code is in the url, and in the webpage embedded in a `<code>` tag  
	![OAuth result](oauth.png)

If you are in a web context, you can set the final page url, and the user will be redirected directly to your server with the code. Just add your url to the `OAuthUrl` method.
```cs
var url = authClient.OAuthUrl(myRedirectPage);
```
Now this code will let you get the access token for the user
```cs
var auth = await authClient.ConnectWithCode(authCode);
```
## Connect with existing authentication token

When you have the access token, you should save it in the app, and use it every time you restart the app. You just need to add it to the MastodonClient constructor.
```cs
var accessToken = auth.AccessToken;
var client = new MastodonClient(instance, accessToken);
```
Now you can call all the API methods. [Mastonet API](https://github.com/glacasa/Mastonet/blob/master/API.md) [Mastodon API overview](https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md)

# Streaming

You can use the `TimelineStreaming` to be notified for every status, notification and deletion on a timeline.
```cs
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
```

# Share a single HttpClient in your entire app

You should share a single instance of HttpClient in your entire app (c.f. [You're using HttpClient wrong and it is destabilizing your software](https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/)). If you have HttpClient in other parts of your app, you can inject it from constructors:
```cs
    var httpClient = new HttpClient();
    var authClient = new AuthenticationClient(instance, httpClient);
    var mastodonClient = new MastodonClient(instance, accessToken, httpClient);
```
