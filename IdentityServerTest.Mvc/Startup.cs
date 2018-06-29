﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace IdentityServerTest.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddOpenIdConnect("oidc", options => {

                    // Set all your OIDC options...

                    options.SignInScheme = "Cookies";
                    options.Authority = "http://localhost:8080/";
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "identity.server.test.mvc";
                    options.ClientSecret = "secret";

                    // and then set SaveTokens to save tokens to the AuthenticationProperties

                    options.ResponseType = "code id_token";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    options.Scope.Add("identity.server.test.mvc.api");
                    options.Scope.Add("offline_access");


                    options.SaveTokens = true;
                });

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "Cookies";
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options =>
            //    {
            //        options.SignInScheme = "Cookies";
            //        options.Authority = "http://localhost:8080/";
            //        options.RequireHttpsMetadata = false;
            //        options.ClientId = "identity.server.test.mvc";
            //        options.ClientSecret = "secret";
            //        options.ResponseType = "code id_token";
            //        options.GetClaimsFromUserInfoEndpoint = true;
            //        options.SaveTokens = true;

            //        options.Scope.Add("identity.server.test.mvc.api");
            //        options.Scope.Add("offline_access");
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
