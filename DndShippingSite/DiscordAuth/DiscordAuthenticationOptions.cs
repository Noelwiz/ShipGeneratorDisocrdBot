/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;


namespace DndShippingSite.DiscordAuth
{
    public class DiscordAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Gets or sets a value which controls how the authorization flow handles existing authorizations.
        /// The default value of this property is <see langword="null"/> and the <c>prompt</c> query string
        /// parameter will not be appended to the <see cref="OAuthOptions.AuthorizationEndpoint"/> value.
        /// Assigning this property any value other than <see langword="null"/> or an empty string will
        /// automatically append the <c>prompt</c> query string parameter to the <see cref="OAuthOptions.AuthorizationEndpoint"/>
        /// value, with the specified value.
        /// </summary>
        public string? Prompt { get; set; }

        public DiscordAuthenticationOptions()
        {
            ClaimsIssuer = DiscordAuthDefaults.Issuer;
            CallbackPath = DiscordAuthDefaults.CallbackPath;
            AuthorizationEndpoint = DiscordAuthDefaults.AuthorizationEndpoint;
            TokenEndpoint = DiscordAuthDefaults.TokenEndpoint;
            UserInformationEndpoint = DiscordAuthDefaults.UserInformationEndpoint;
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");

            Scope.Add("identify");
        }
    }
}
