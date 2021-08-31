﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to that project and this repository.
 */
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;


namespace DndShippingSite.DiscordAuth
{
    public class DiscordAuthenticationHandler : OAuthHandler<OAuthOptions> //<OAuthOptions> => allow me to write this for my purposes using addOAuth in config
    {
        public DiscordAuthenticationHandler(
           [NotNull] IOptionsMonitor<DiscordAuthenticationOptions> options,
           [NotNull] ILoggerFactory logger,
           [NotNull] UrlEncoder encoder,
           [NotNull] ISystemClock clock)
           : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override string BuildChallengeUrl(
            [NotNull] AuthenticationProperties properties,
            [NotNull] string redirectUri)
        {
            string challengeUrl = base.BuildChallengeUrl(properties, redirectUri);

            /*
            if (!string.IsNullOrEmpty(Options.Prompt))
            {
                challengeUrl = QueryHelpers.AddQueryString(challengeUrl, "prompt", Options.Prompt);
            }
            */

            return challengeUrl;
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }
    }
}