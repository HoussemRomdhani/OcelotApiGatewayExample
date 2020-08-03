using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Authorisation;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ocelot.gateway.api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string textKey = "THIS_IS_A_TOP_SECRET_KEY";

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(textKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = "api.factory",
                ValidateAudience = true,
                ValidAudiences = new string[] { "user.management", "products"},
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(
                x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
              .AddJwtBearer("TestKey", x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.TokenValidationParameters = tokenValidationParameters;
              });

          
            services.AddOcelot(Configuration);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var configration = new OcelotPipelineConfiguration
            {
                AuthorisationMiddleware = async (ctx, next) =>
                {
                    if (Authorize(ctx))
                    {
                        await next.Invoke();
                    }
                    else
                    {
                        ctx.Items.SetError(new UnauthorisedError($"Fail to authorize"));
                    }
                }
            };
            //your code here
            app.UseOcelot(configration).Wait();

        }

        private bool Authorize(HttpContext ctx)
        {
            if (ctx.Items.DownstreamRoute().AuthenticationOptions.AuthenticationProviderKey == null) return true;
            else
            {
                bool auth = false;
                Claim[] claims = ctx.User.Claims.ToArray();
                Dictionary<string, string> required = ctx.Items.DownstreamRoute().RouteClaimsRequirement;
                Regex reor = new Regex(@"[^,\s+$ ][^\,]*[^,\s+$ ]");
                MatchCollection matches;

                Regex reand = new Regex(@"[^&\s+$ ][^\&]*[^&\s+$ ]");
                MatchCollection matchesand;
                int cont = 0;
                foreach (KeyValuePair<string, string> claim in required)
                {
                    matches = reor.Matches(claim.Value);
                    foreach (Match match in matches)
                    {
                        matchesand = reand.Matches(match.Value);
                        cont = 0;
                        foreach (Match m in matchesand)
                        {
                            foreach (Claim cl in claims)
                            {
                                if (cl.Type == claim.Key)
                                {
                                    if (cl.Value == m.Value)
                                        cont++;
                                }
                            }
                        }
                        if (cont == matchesand.Count)
                        {
                            auth = true;
                            break;
                        }
                    }
                }
                return auth;
            }
        }
    }
}
