using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DndShippingSite.Areas.Identity
{
    public class DiscordAuthenticationProvider<TUser> : RevalidatingServerAuthenticationStateProvider where TUser : class
    {
        private ClaimsPrincipal UserClaims;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;

        public DiscordAuthenticationProvider(ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor)
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = optionsAccessor.Value;

            var identity = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, "mrfibuli"),
            }, "Discord");
            UserClaims = new ClaimsPrincipal(identity);
        }

        //todo: allow config for this?
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(UserClaims));
        }

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
