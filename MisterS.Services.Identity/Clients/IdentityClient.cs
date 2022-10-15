using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace MisterS.Services.Identity.Clients
{
    public static class IdentityClient
    {
        public static IEnumerable<Client> clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client",
                    ClientSecrets = { new Secret("SahilSeceret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes ={"read","write","profile"}
                },
                new Client
                {
                    ClientId="MisterS",
                    ClientSecrets = { new Secret("MisterSSeceret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.Code,
                    RedirectUris = {"https://localhost:7292/signin-oidc"},
                    PostLogoutRedirectUris = { "https://localhost:7292/signout-callback-oidc" },
                    AllowedScopes= new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "misterS"
                    }
                }
            };
    }
}
