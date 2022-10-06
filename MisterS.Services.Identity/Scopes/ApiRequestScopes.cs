using Duende.IdentityServer.Models;

namespace MisterS.Services.Identity.Scopes
{
    public static class ApiRequestScopes
    {
        public static IEnumerable<ApiScope> apiScopes =>
            new List<ApiScope>
            {
                new ApiScope(name: "misterS",displayName:"MisterS Server"),
                new ApiScope(name: "Read",displayName:"Read Data"),
                new ApiScope(name: "Write",displayName:"Write Data"),
                new ApiScope(name: "Delete",displayName:"Delete Data"),
            };
    }
}
