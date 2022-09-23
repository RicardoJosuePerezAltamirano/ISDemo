using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Runtime.Intrinsics.Arm;

namespace AuthenticationsProvider
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                     Name="role",
                     UserClaims=new List<string>{"role"}
                }
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("weatherapi.read"),
                new ApiScope("weatherapi.write"),
                new ApiScope("api1"),

            };
        public static IEnumerable<ApiResource> ApiResources =>
            new[]{
                new ApiResource("weatherapi")
                {
                     Scopes=new List<string>{ "weatherapi.read", "weatherapi.write" },
                      ApiSecrets= new List<Secret>{new Secret("secreto".Sha256()) },
                      UserClaims= new List<string> {"role"}
                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // para aprender 
                // machine to machine client -flijo clientCrednetials
                new Client
                {
                    ClientId = "client",
                    ClientName="Client Credentials",
                    
                
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    // scopes that client has access to
                    AllowedScopes = { "weatherapi.read", "weatherapi.write" }
                },
                
                // interactive ASP.NET Core MVC client, using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                
                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:7293/signin-oidc" },// aplicacion cliente
                
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:7293/signout-callback-oidc" },// aplicacion cliente
                    FrontChannelLogoutUri= "https://localhost:7293/signout-oidc" ,// aplicacion cliente
                
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                        "weatherapi.read"
                    }
                
                    ,AllowOfflineAccess = true
                    ,RequireConsent = true
                },
                // para aprender 

                // demo maui
                new Client
            {
                ClientId = "mauiidentity",
                ClientSecrets = { new Secret("SuperSecretPassword".Sha256()) }, //Change it for real usage
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "mauiidentity://" }, // where to redirect to after login
                PostLogoutRedirectUris = { "mauiidentity://" }, // where to redirect to after logout
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1",
                    "weatherapi.read"
                }
            }
                // demo maui
            };
    }
}