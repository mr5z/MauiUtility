using AuthClient.Services.Identity;
using AuthClient.Services.Identity.Requests;
using System.Text.Json;

namespace MauiUtility.Services.Implementation;

public class MauiIdentityClient(HttpClient httpClient) : CrossIdentityClient(httpClient)
{
    public override async Task<AuthorizationResponse> Authorize(AuthorizationRequest request)
    {
        var authUrl = request.BuildUrl();
        var callbackUrl = new Uri(request.RedirectUri);
        var result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
        var serialized = JsonSerializer.Serialize(result.Properties);
        var authorizationResponse = JsonSerializer.Deserialize<AuthorizationResponse>(serialized);
        return authorizationResponse!;
    }
}
