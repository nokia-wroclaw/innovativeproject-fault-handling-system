using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Fault_handling_system.Services;

namespace Fault_handling_system
{
    /// <summary>
    /// Class responsible for creating the application's request processing pipeline
    /// and configuring the application services.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Application logger
        /// </summary>
        private readonly ILogger<Startup> _logger;
        
        /// <summary>
        /// Constructor accepting dependencies configured by the host.
        /// </summary>
        /// <param name="logger">Application logger</param>
        /// <param name="configuration">Dependency to configure the application during startup</param>
        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        /// <summary>
        /// Application configuration properties
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. 
        /// Used to add services to the container.
        /// </summary>
        /// <param name="services">Collection to add the services to</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailboxFetcherSettings>(Configuration);
            services.Configure<EmailSenderSettings>(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AzureConnection"))); //DefaultConnection

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IHostedService, MailboxFetcherService>();
            services.AddSingleton<IMailboxFetcher, MailboxFetcher>();
            services.AddSingleton<IReportParser, ReportParser>();

            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. 
        /// Used to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
