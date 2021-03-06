﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Flashcards.Api.Configuration;
using Flashcards.Api.Middleware;
using Flashcards.Infrastructure.ContainerModules;
using Flashcards.Infrastructure.DataAccess;
using Flashcards.Infrastructure.Extensions;
using Flashcards.Infrastructure.Services;
using Flashcards.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Flashcards.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
        public ILifetimeScope Container { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver());

            services.AddMemoryCache();
            services.AddCors();

            services.AddDbContext<EFContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSettings<SqlSettings>().ConnectionString);
            });
            services.AddHostedService<QueueListener>();
            
            services.AddJwtTokenAuthentication(Configuration);

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flashcards API", Version = "v1" }));
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ServicesModule(Configuration));
            builder.RegisterModule(new SettingsModule(Configuration));
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule(new MediatorModule(Configuration));
            builder.RegisterModule<MongoModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            Container = app.ApplicationServices.GetAutofacRoot();
            
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            
            app.UseRouting();
            app.UseCors(cors =>
            {
                cors.AllowAnyOrigin();
                cors.AllowAnyMethod();
                cors.AllowAnyHeader();
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flashcards API V1"));

            app.UseExceptionHandlerMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Flashcards is working on '{HostingEnvironment.EnvironmentName}'...");
                });
            });
        }
    }
}
