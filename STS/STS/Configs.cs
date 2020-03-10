/// Mohamed Ali NOUIRA
/// http://www.mohamedalinouira.com
/// https://github.com/medalinouira
/// Copyright © Mohamed Ali NOUIRA. All rights reserved.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace STS
{
    public class Configs
    {
        #region GETTERS
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "your-client-app-id",
                    ClientName = "YOUR CLIENT APP NAME",

                    RequirePkce = true,
                    RequireConsent = false,
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Code,

                    AllowedCorsOrigins =     { "YOUR-CLIENT-APP-URI" },
                    PostLogoutRedirectUris = { "YOUR-CLIENT-APP-POST-LOGOUT-REDIRECT-URI" },
                    RedirectUris =           { "YOUR-CLIENT-APP-SIGNIN-REDIRECT-URI", "YOUR-CLIENT-APP-SILENT-RENEW-TOKRN-CALLBACK-URI" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "your-api-scope"
                    },
                    AccessTokenLifetime = 600,
                }
            };

        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("your-api-name", "YOUR-API-DISPLAY-NAME")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
        #endregion
    }
}
