using Duende.IdentityServer.Models;

namespace MisterS.Services.Identity.Resources
{
    public static class IdentitySource
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };
    }
}
