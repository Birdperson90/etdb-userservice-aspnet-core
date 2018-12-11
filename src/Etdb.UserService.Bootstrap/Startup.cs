﻿using System;
using Autofac;
using Etdb.ServiceBase.Constants;
using Etdb.UserService.Bootstrap.Config;
using Etdb.UserService.Bootstrap.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace Etdb.UserService.Bootstrap
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;

        private const string SwaggerDocDescription = "Etdb " + ServiceNames.UserService + " V1";
        private const string SwaggerDocVersion = "v1";
        private const string SwaggerDocJsonUri = "/swagger/v1/swagger.json";
        private const string CorsPolicyName = "AllowAll";
        private const string AuthenticationSchema = "Bearer";

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var allowedOrigins = this.configuration
                .GetSection(nameof(AllowedOriginsOptions))
                .GetSection("Origins")
                .Get<string[]>();

            var identityConfig = this.configuration
                .GetSection("IdentityConfig");

            var clientId = identityConfig
                .GetSection("WebClient")
                .GetValue<string>("Name");

            var clientSecret = identityConfig
                .GetSection("WebClient")
                .GetValue<string>("Secret");

            var authority = identityConfig
                .GetValue<string>("Authority");

            services.ConfigureMvc()
                .ConfigureCors(this.environment, allowedOrigins, Startup.CorsPolicyName)
                .ConfigureAllowedOriginsOptions(this.configuration)
                .ConfigureIdentityServerAuthorization(allowedOrigins, clientId, clientSecret)
                .ConfigureIdentityServerAuthentication(this.environment, Startup.AuthenticationSchema,
                    ServiceNames.UserService, authority)
                .ConfigureAuthorizationPolicies()
                .ConfigureRedisCache(this.configuration)
                .ConfigureDocumentDbContextOptions(this.configuration)
                .ConfigureFileStoreOptions(this.configuration, this.environment)
                .ConfigureCompression()
                .ConfigureHttpClients()
                .ConfigureSwaggerGen(this.environment, new Info
                {
                    Title = Startup.SwaggerDocDescription,
                    Version = Startup.SwaggerDocVersion
                }, Startup.SwaggerDocVersion);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.SetupSwagger(this.environment, Startup.SwaggerDocJsonUri, Startup.SwaggerDocDescription);

            app.SetupHsts(this.environment);

            app.SetupForwarding(this.environment);

            app.UseCors(Startup.CorsPolicyName);

            app.UseIdentityServer();

            app.SetupMvc();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.SetupDependencies(this.environment, this.configuration);
        }
    }
}