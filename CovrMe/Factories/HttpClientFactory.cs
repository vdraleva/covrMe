using CovrMe.Handlers;

namespace CovrMe.Factories;

public static class HttpClientFactory
{
    public static HttpClient Create()
    {
#if DEBUG
        if (App.IsLocalhost)
        {
            return new HttpClient(HttpsClientHandlerService.GetPlatformMessageHandler());
        }
        else
        {
            return new HttpClient();
        }
#else
        return new HttpClient();
#endif
    }
}
