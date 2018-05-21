using System;
using System.IO;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AWSServerlessCoreApp.DynamoDbLibs;

namespace AWSServerlessCoreApp
{
    public class Startup
    {
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "AWS:AccessKey");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "AWS:SecretKey");
            Environment.SetEnvironmentVariable("AWS_REGION", "AWS:Region");
            services.AddAWSService<IAmazonDynamoDB>();
            //services.AddSingleton<ICreateTable, CreateTechnologiesTable>();
            //services.AddSingleton<ICreateTable, CreateRequirementsTable>();
            //services.AddSingleton<ICreateTable, CreateTrainingProgramsTable>();
            services.AddSingleton<PutTechnologiesItem>();
            services.AddSingleton<PutRequirementsItem>();
            services.AddSingleton<PutTrainingProgramItem>();
            services.AddSingleton<GetTecnologiesItem>();
            services.AddSingleton<GetRequirementsItem>();
            services.AddSingleton<GetTrainingProgramsItem>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
