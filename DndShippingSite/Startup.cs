using DndShippingSite.Areas.Identity;
using DndShippingSite.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;
using System.Text.Json.Nodes;
using DndShippingSite.DiscordAuth;
using static System.Net.WebRequestMethods;
using Humanizer;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;

namespace DndShippingSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Read only string to name the CORS policy
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-5.0"/>
        readonly private static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {

                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost",
                                            "127.0.0.1",
                                            "http://localhost:5000",
                                            "http://localhost:5001",
                                            "https://discord.com/api/",
                                            "https://discord.com/api/oauth2/token",
                                            "https://discord.com/api/oauth2/authorize"
                                            );
                                      builder.AllowAnyHeader();
                                  });
            });


            // see https://httpd.apache.org/docs/2.4/mod/mod_proxy.html#x-headers
            // see https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-6.0#apache-configuration
            // see https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-6.0
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownProxies.Add(IPAddress.Parse("127.0.0.1")); //TODO: replace with proxy, maybe from config
            });


            /*
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            */

            /*
             *  'client_id': CLIENT_ID,
                'client_secret': CLIENT_SECRET,
                'grant_type': 'authorization_code',
                'code': code,
                'redirect_uri': REDIRECT_URI
             */

            //TODO: https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/adding-aspnet-identity-to-an-empty-or-existing-web-forms-project

            //I think this would let us override the user claims types
            //https://youtu.be/egITMrwMOPU?t=333 looks like we could use this to have a special discord user type and assign them roles
            //using a db if we want to set one up for that
            //services.AddIdentity()

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/additional-claims?view=aspnetcore-5.0
            //see https://mauridb.medium.com/using-oauth2-middleware-with-asp-net-core-2-0-b31ffef58cd0
            services.AddAuthentication()
                //consider: maybe just using the default oauth handler?
                .AddOAuth<OAuthOptions, Areas.Identity.DiscordAuthenticationHandler> ("Discord", options =>
                {
                    options.ClaimsIssuer = DiscordAuthDefaults.Issuer;
                    options.ClientId = Configuration["client_id"];
                    options.ClientSecret = Configuration["client_secret"];
                    options.AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
                    options.UserInformationEndpoint = "https://discord.com/api/users/@me";
                    options.TokenEndpoint = DiscordAuthDefaults.TokenEndpoint;
                    options.CallbackPath = new PathString("/signin-custom"); //todo: update this

                    int hours;
                    if (!int.TryParse(Configuration["discord_timeout_hours"], out hours))
                    {
                        hours = 0;
                    }

                    int minutes;
                    if(! int.TryParse(Configuration["discord_timeout_minutes"], out minutes))
                    {
                        minutes = 30;
                    }

                    options.RemoteAuthenticationTimeout = new TimeSpan(hours, minutes, 0);

                    options.TokenEndpoint = Configuration["discord_token_endpoint"];

                    options.Scope.Add("identify");

                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");

                    options.SaveTokens = true;

                    //copied then adapted from https://mauridb.medium.com/using-oauth2-middleware-with-asp-net-core-2-0-b31ffef58cd0
                    //register events apparently
                    options.Events = new OAuthEvents
                    {
                        //the main authentication event it seems like from the handler. Basically, if we want to handle this part of auth, we kinda can? or at least sub to the event.
                        OnCreatingTicket = async context =>
                        {
                            // Create the request message to get user data via the backchannel
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            // Query for user data via backchannel
                            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            // Parse user data into an object //only use of newtonsoft so far
                            var resoponse = await response.Content.ReadAsStringAsync();

                            var user = JObject.Parse(resoponse);

                            // Store the received authentication token somewhere. In a cookie for example
                            context.HttpContext.Response.Cookies.Append("token", context.AccessToken);

                            // Execute defined mapping action to create the claims from the received user object
                            using (JsonDocument document = JsonDocument.Parse(resoponse))
                            {
                                JsonElement root = document.RootElement;
                                context.RunClaimActions(root);
                            }

                            List<AuthenticationToken> tokens = context.Properties.GetTokens().ToList();

                            tokens.Add(new AuthenticationToken()
                            {
                                Name = "TicketCreated",
                                Value = DateTime.UtcNow.ToString()
                            });

                            context.Properties.StoreTokens(tokens);
                        }
                    };
                }
            );

            //get user info from https://discord.com/api/users/@me


            services.AddRazorPages();
            services.AddServerSideBlazor();
            //services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddScoped<AuthenticationStateProvider, FakeAuthProvider>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseForwardedHeaders();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
