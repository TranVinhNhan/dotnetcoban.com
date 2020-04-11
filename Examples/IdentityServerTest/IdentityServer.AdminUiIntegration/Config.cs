﻿using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer.AdminUiIntegration
{
    public static class Config
    {
        // OpenID Connect allowed scopes
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // APIs to be protected
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("ResourceApi", "Example Resource API")
            };
        }

        // Clients allowed to request for tokens
        public static IEnumerable<Client> GetClients()
        {
            string swaggerClientUrl = "http://localhost:5001";
            string mvcClientUrl = "http://localhost:5002";
            string spaClientUrl = "http://localhost:5003";
            return new List<Client>
            {
                // ConsoleApp client
                new Client
                {
                    ClientId = "ConsoleAppClient",
                    ClientName = "ConsoleApp Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "ResourceApi" },
                },

                // Resource owner password grant client
                new Client
                {
                    ClientId = "ResourceOwnerClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "ResourceApi" }
                },
                // MVC client support PKCE
                new Client
                {
                    ClientId = "MvcClient",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Code,

                    RequirePkce = true,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = { $"{mvcClientUrl}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{mvcClientUrl}/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ResourceApi"
                    },

                    AllowOfflineAccess = true
                },
                // Resource API Swagger UI
                new Client
                {
                    ClientId = "resourcesswaggerui",
                    ClientName = "Resource Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{swaggerClientUrl}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{swaggerClientUrl}/swagger/" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ResourceApi"
                    }
                },

                // Postman
                new Client
                {
                    ClientId = "postman",
                    ClientName = "Postman Client",

                    AllowedGrantTypes = GrantTypes.Code,
                    //AllowAccessTokensViaBrowser = true,
                    //RequireConsent = false,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AccessTokenLifetime = 3600,

                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    FrontChannelLogoutUri = "https://www.getpostman.com/oauth2/callback/",
                    PostLogoutRedirectUris = { "https://www.getpostman.com/oauth2/callback/" },
                    AllowedCorsOrigins = { "https://www.getpostman.com" },

                    AllowOfflineAccess = true,
                    //EnableLocalLogin = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ResourceApi"
                    }
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "JsClient",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { $"{spaClientUrl}/callback.html" },
                    PostLogoutRedirectUris = { $"{spaClientUrl}/index.html" },
                    AllowedCorsOrigins =     { $"{spaClientUrl}" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ResourceApi"
                    }
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "demo",
                    Password = "demo".Sha256()
                }
            };
        }
    }
}
