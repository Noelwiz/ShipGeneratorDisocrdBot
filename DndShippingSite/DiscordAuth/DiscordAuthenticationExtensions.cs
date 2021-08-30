/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;

namespace DndShippingSite.DiscordAuth
{
    public static class DiscordAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="DiscordAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Discord authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddDiscord([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddDiscord(DiscordAuthDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="DiscordAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Discord authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddDiscord(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<DiscordAuthenticationOptions> configuration)
        {
            return builder.AddDiscord(DiscordAuthDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="DiscordAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Discord authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Discord options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDiscord(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<DiscordAuthenticationOptions> configuration)
        {
            return builder.AddDiscord(scheme, DiscordAuthDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="DiscordAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Discord authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Discord options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDiscord(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            //[CanBeNull]
            string caption,
            [NotNull] Action<DiscordAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<DiscordAuthenticationOptions, DiscordAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
