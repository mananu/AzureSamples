using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.Interfaces;
using MultiTenant.Repositories;
using MultiTenant.Utils;
using System;
using static MultiTenant.Utils.AppConfig;

namespace MultiTenant.Web
{
    public class Startup
    {
        #region Private fields

        private IUtilities _utilities;
        private ITenantRepository _tenantRepository;

        #endregion

        #region Public Properties

        public static DatabaseConfig DatabaseConfig { get; set; }
        public static TenantServerConfig TenantServerConfig { get; set; }
        public IConfiguration Configuration { get; }

        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ReadAppConfig();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            services.AddSession();

            //Add Application services
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddSingleton<ITenantRepository>(p => new TenantRepository(GetTenantConnectionString()));

            //create instance of utilities class
            services.AddTransient<IUtilities, Utilities>();
            var provider = services.BuildServiceProvider();
            _utilities = provider.GetService<IUtilities>();
            _tenantRepository = provider.GetService<ITenantRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
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

        #region Private methods

        /// <summary>
        ///  Gets the tenant connection string using the app settings
        /// </summary>
        /// <param name="tenantConfig">The tenant server configuration.</param>
        /// <param name="databaseConfig">The database configuration.</param>
        /// <returns></returns>
        private string GetTenantConnectionString()
        {
            return $"Server=tcp:{TenantServerConfig.TenantServer},1433;Database={TenantServerConfig.TenantDatabase};User ID={DatabaseConfig.DatabaseUser};Password={DatabaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=True;";

        }

        /// <summary>
        /// Reads the application settings from appsettings.json
        /// </summary>
        private void ReadAppConfig()
        {
            DatabaseConfig = new DatabaseConfig
            {
                DatabasePassword = Configuration["DatabasePassword"],
                DatabaseUser = Configuration["DatabaseUser"],
                DatabaseServerPort = Convert.ToInt32(Configuration["DatabaseServerPort"]),
                ConnectionTimeOut = Convert.ToInt32(Configuration["ConnectionTimeOut"]),
                LearnHowFooterUrl = Configuration["LearnHowFooterUrl"]
            };

            TenantServerConfig = new TenantServerConfig
            {
                TenantServer = Configuration["TenantServer"] + ".database.windows.net",
                TenantDatabase = Configuration["TenantDatabase"],
            };
            bool isResetEventDatesEnabled = false;
            if (bool.TryParse(Configuration["ResetEventDates"], out isResetEventDatesEnabled))
            {
                TenantServerConfig.ResetEventDates = isResetEventDatesEnabled;
            }
        }

        #endregion
    }
}
