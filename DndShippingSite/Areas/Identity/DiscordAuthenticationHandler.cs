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
using Microsoft.AspNet.Http.Features.Authentication;

namespace DndShippingSite.Areas.Identity
{
    public class DiscordAuthenticationHandler : OAuthHandler<OAuthOptions>
    {

        public DiscordAuthenticationHandler(
           [NotNull] IOptionsMonitor<OAuthOptions> options,
           [NotNull] ILoggerFactory logger,
           [NotNull] UrlEncoder encoder,
           [NotNull] ISystemClock clock)
           : base(options, logger, encoder, clock)
        {


        }



        /// <summary>
        /// Presumably creates the "challenge" url for the user to authenticate at
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            return base.BuildChallengeUrl(properties, redirectUri);
        }
       


        protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint); //discord api for @me
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //accept json
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken); //send auth token

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
