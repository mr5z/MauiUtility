using System.Text.Json;
using AuthClient.Models;
using AuthClient.Models.Requests;
using AuthClient.Services.Implementation;
using CrossUtility.Services;

namespace MauiUtility.Services.Implementation;

public class MauiIdentityClient(IHttpService httpService) : CrossIdentityClient(httpService)
{
    public override async Task<AuthorizationResponse> Authorize(AuthorizationRequest request)
    {
        var authUrl = request.BuildUrl();
        var callbackUrl = request.RedirectUri;
        var result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
        var serialized = JsonSerializer.Serialize(result.Properties);
        var authorizationResponse = 
            JsonSerializer.Deserialize<AuthorizationResponse>(serialized)
            ?? throw new InvalidOperationException("Unable to deserialize data from response");
        return authorizationResponse;
    }
}
