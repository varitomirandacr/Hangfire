using Hangfire;
using Hangfire.Mongo;
using HangfireAPI.Contract;
using HangfireAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace HangfireAPI
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
            services.AddControllers();

            var mongoConnection = Configuration.GetConnectionString("HangfireConnection");
            var migrationOptions = new MongoMigrationOptions
            {
                Strategy = MongoMigrationStrategy.Drop,
                BackupStrategy = MongoBackupStrategy.Collections
            };

            // Add Hangfire services.
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseMongoStorage(new MongoClient(mongoConnection), "Hangfire", new MongoStorageOptions 
                { 
                    MigrationOptions = migrationOptions,
                    CheckConnection = false, 
                });
            });

            // SQL settings
            //.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //.UseSimpleAssemblyNameTypeSerializer()
            //.UseRecommendedSerializerSettings()
            //.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            //{
            //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //    QueuePollInterval = TimeSpan.Zero,
            //    UseRecommendedIsolationLevel = true,
            //    UsePageLocksOnDequeue = true,
            //    DisableGlobalLocks = true
            //}));

            services.AddTransient<IProcessService, ProcessService>();

            // https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-2.2&tabs=visual-studio#jsonnet-support
            services.AddControllers().AddNewtonsoftJson();

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHangfireDashboard();
            backgroundJobs.Enqueue(() => Console.WriteLine("Access the Dashboard at: https://localhost:5001/hangfire"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
