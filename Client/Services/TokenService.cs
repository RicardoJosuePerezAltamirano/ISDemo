using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Client.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<IdentityServerSettings> identityServerSettings;
        private readonly DiscoveryDocumentResponse discoveryDocumentResponse;

        public TokenService(IOptions<IdentityServerSettings> identityServerSettings
            )
        {
            this.identityServerSettings = identityServerSettings;
            //this.discoveryDocumentResponse = discoveryDocumentResponse;

            using var httpClient = new HttpClient();
            discoveryDocumentResponse = httpClient.GetDiscoveryDocumentAsync(identityServerSettings.Value.DiscoveryUrl).Result;
            if(discoveryDocumentResponse.IsError)
            {
                throw new Exception("error no hay discpvery document");
            }
        }
        public async Task<TokenResponse> GetToken(string scope)
        {
            using var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = identityServerSettings.Value.ClientName,
                ClientSecret = identityServerSettings.Value.ClientPassword,
                Scope = scope
            });
            if(tokenResponse.IsError)
            {
                throw new Exception("error al obtener token");
            }
            return tokenResponse;
            
                
        }
    }
}
