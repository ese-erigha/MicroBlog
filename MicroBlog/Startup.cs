using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using MicroBlog.Core;
using MicroBlog.Core.Interfaces;
using MicroBlog.Helpers;
using MicroBlog.Helpers.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NetCore.AutoRegisterDi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MicroBlog
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                                            .SetBasePath(env.ContentRootPath)
                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                                            .AddEnvironmentVariables();

            Configuration = builder.Build();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddSingleton(Configuration);

            //Required to inject IOptions
            services.AddOptions();
            //Configure strongly typed settings objects
            var sendGridSettingsSection = Configuration.GetSection("SendGridSettings");
            services.Configure<SendGridSettings>(sendGridSettingsSection);

            var smtpSettingsSection = Configuration.GetSection("SmtpSettings");
            services.Configure<SmtpSettings>(smtpSettingsSection);


            //Get App Settings
            //var appSettings = emailSettingsSection.Get<EmailSettings>();

            services.AddDistributedMemoryCache();


            services.AddSingleton<IConfiguration>(Configuration);

            var assemblyToScan = Assembly.Load("MicroBlog");

            //Register Services
            services.RegisterAssemblyPublicNonGenericClasses(assemblyToScan)
                     .Where(x => x.Name.EndsWith("Service"))
                     .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);


            //Register Repositories
            services.RegisterAssemblyPublicNonGenericClasses(assemblyToScan)
                     .Where(x => x.Name.EndsWith("Repository"))
                     .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);


            //Register Helper Classes
            services.RegisterAssemblyPublicNonGenericClasses(assemblyToScan)
                     .Where(x => x.Name.EndsWith("Helper"))
                     .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            //Add AutoMapper
            services.AddAutoMapper();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = "Server = localhost,1433; Database = MicroBlog; User ID=sa; Password = lexEME5195..";
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

            var hangfireConnectionString = "Server = localhost,1433; Database = MicroBlogHangfireDB; User ID=sa; Password = lexEME5195..";


            var hangfireOptions = new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true,
                QueuePollInterval = TimeSpan.FromSeconds(60)  //Default polling of 15 seconds
            };

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(hangfireConnectionString,hangfireOptions);

            });

            services.AddIdentity<Entities.ApplicationUser, IdentityRole<long>>(options =>
            {
                //Password Options
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;


                //Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;


                //User settings
                options.User.RequireUniqueEmail = true;

            })
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options => {

                //Cookie Settings

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                options.LoginPath = "/auth/login";
                options.AccessDeniedPath = "/auth/login";
                options.SlidingExpiration = true;

            });

            services.AddSession();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddMvc(
                        options =>
                        {
                            options.ReturnHttpNotAcceptable = true;
                            options.RespectBrowserAcceptHeader = true;
                        }
                     )
                    .AddXmlSerializerFormatters()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; //Ignores cycles in related data serialization
                        if (options.SerializerSettings.ContractResolver != null)
                        {
                            //var castedResolver = options.SerializerSettings.ContractResolver as DefaultContractResolver;
                            //castedResolver.NamingStrategy = null;

                            // Force Camel Case to JSON
                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        }
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseStaticFiles();   //For the wwwroot folder

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/StaticFiles"
            });
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes => {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Register}"
                );
            });
            app.UseCookiePolicy(); //Should always come after app.UseMvc
        }
    }
}
