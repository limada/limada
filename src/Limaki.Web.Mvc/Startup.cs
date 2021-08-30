using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Limaki.Common;
using Limaki.Common.IOC;
using Limada.Usecases.Cms;
using Limada.Usecases.Cms;
using System.Collections.Specialized;
using Microsoft.AspNetCore.HttpOverrides;
using Limaki.Web.MvcCore.Controllers;

namespace Limaki.Web.MvcCore
{

    public class MvcAppFactory : AppFactory<Limada.Usecases.LimadaResourceLoader> {
        public MvcAppFactory () {
            this.Configure (new WebCmsContextResourceLoader ());
        }
    }

    public class Startup
    {

        AppController _appController;
        private static MvcAppFactory appfactory;
        public AppController AppController {
            get {
                if (_appController == null) {
                    _appController = Registry.Pooled<AppController> ();
                    //  _appController.ApplicationPhysicalPath = this.Server.MapPath ("");
                }
                return _appController;
            }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            appfactory = new MvcAppFactory ();
            var settings = new NameValueCollection ();
            foreach (var item in Configuration.GetSection ("appsettings").GetChildren ()) {
                settings.Add (item.Key, item.Value);
            }
            Registry.Pooled<AppController> ().AppSettingsGetter = () => settings;
            AppController.OpenBackend ();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseForwardedHeaders (new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            //app.UseMvc();
            var nullId = default (string);
            app.UseMvc (routes => {

                // routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

                routes.MapRoute (
                    "BrowseCompatible", // Route name 
                    "Default.aspx", // URL with parameters
                    new { controller = "Home", action = nameof(HomeController.AspxReqest) });

                routes.MapRoute (
                   "StreamContent", // Route name 
                   "Content/{id?}", // URL with parameters
                   new { controller = "Home", action = "StreamContent" });

                routes.MapRoute (
                    "browse", // Route name
                    "{id?}", // URL with parameters
                    new { controller = "Home", action = nameof (HomeController.Index) });
                routes.MapRoute (
                     nameof (HomeController.Index), // Route name
                    "{id?}", // URL with parameters
                    new { controller = "Home", action = nameof (HomeController.Index) });
                
                routes.MapRoute (
                    nameof (HomeController.About), // Route name
                    "{id?}", // URL with parameters
                    new { controller = "Home", action = nameof (HomeController.About) });
                
                routes.MapRoute (
                    "default", // Route name
                    "{id?}", // URL with parameters
                    new { controller = "Home", action = nameof (HomeController.Index) });

            });
        }
    }
}
