﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource
            {
                Name = "roles",
                UserClaims = new List<string>{"roles"}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("IDP_api.read", "IDP API Read Scope"),
                new ApiScope("IDP_api.write", "IDP API Write Scope"),
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new ApiResource("IDP_api", "IDP API")
                {
                    Scopes = { "IDP_api.read", "IDP_api.write" },
                    UserClaims = { "roles" }
                }
            };

    public static IEnumerable<Client> Clients =>
        new Client[]
            {
                new Client
                {
                    ClientName = "IDP Swagger Client",
                    ClientId = "idp_swagger",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 60 * 60 * 2,
                    RedirectUris =
                    {
                        "https://localhost:5000/swagger/oauth2-redirect.html",
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:5000/swagger/oauth2-redirect.html",
                    },
                    AllowedCorsOrigins = { "https://localhost:5000" },
                    AllowedScopes = 
                    { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "IDP_api.read",
                        "IDP_api.write",
                        "roles" 
                    }
                }
            };
}