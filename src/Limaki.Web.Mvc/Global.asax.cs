using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Limada.Usecases.Cms;
using Limaki.Common;
using Limaki.Common.IOC;
using Limada.UseCases.Cms;

namespace Limaki.Web.Mvc {

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcAppFactory : AppFactory<Limada.Usecases.LimadaResourceLoader> {
        public MvcAppFactory() {
            // TODO: find out how to get bin-directory, and not asp....temp....
            this.Create(new WebCmsContextResourceLoader());
        }
    }

    public class MvcApplication : System.Web.HttpApplication {

        static MvcApplication () {
            appfactory = new MvcAppFactory();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "BrowseCompatible", // Route name 
                "Default.aspx", // URL with parameters
                new { controller = "Home", action = "AspxReqest", id = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
               "StreamContent", // Route name 
               "Content/{id}", // URL with parameters
               new { controller = "Home", action = "StreamContent", id = UrlParameter.Optional } // Parameter defaults
               );

            routes.MapRoute(
                "Browse", // Route name
                "{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );

            
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            AppController.OpenBackend ();
        }

        protected virtual void Application_End (Object sender, EventArgs e) {
            AppController.Close ();
        }

        protected void Application_BeginRequest () {
          
        }

        protected void Application_EndRequest () {
            if (Context.Response.StatusCode == 404) {

            }
        }

        AppController _appController;
        private static MvcAppFactory appfactory;
        public AppController AppController {
            get {
                if (_appController == null) {
                    _appController = Registry.Pooled<AppController> ();
                    _appController.ApplicationPhysicalPath = this.Server.MapPath ("");
                }
                return _appController;
            }
        }
    }
}