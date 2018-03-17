using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Fault_handling_system.Services;

namespace Fault_handling_system
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AzureConnection"))); //DefaultConnection

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            ThreadPool.QueueUserWorkItem(delegate {
                while (true) {
                    _logger.LogInformation("Checking mailbox...");

                    using (var client = new ImapClient()) {
                        // For demo-purposes, accept all SSL certificates
                        client.ServerCertificateValidationCallback = (s,c,h,e) => true;

                        client.Connect("imap.poczta.onet.pl", 993, true);

                        client.Authenticate("pwr.fhs@onet.pl", "FaultHandlingSystem1");

                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);

                        _logger.LogInformation("Total messages: {0}", inbox.Count);
                        _logger.LogInformation("Recent messages: {0}", inbox.Recent);

                        for (int i = 0; i < inbox.Count; ++i) {
                            var message = inbox.GetMessage(i);
                            _logger.LogInformation("Subject: {0}", message.Subject);
                        }

                        client.Disconnect(true);
                    }

                    _logger.LogInformation("Finished checking. Next check after a while...");
                    Thread.Sleep(15000);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
