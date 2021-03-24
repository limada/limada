using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Contents;
using Limada.Usecases.Cms;
using Limada.Usecases.Cms.Models;
using Limaki.Web.MvcCore.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.IO;
using Limaki.Contents.Text;

namespace Limaki.Web.MvcCore.Controllers {

    public class HomeController : Controller {

        AppController _appController;
        public AppController AppController => _appController ??= Registry.Pooled<AppController> ();

        public BackendController Backend => AppController.Backend;

        //protected override void Initialize (System.Web.Routing.RequestContext requestContext) {
        //    base.Initialize (requestContext);
        //    this.ViewBag.SiteName = AppController.SiteName;
        //}

        public class Views {
            public const string Hrefs = "Hrefs";
            public const string ContentAndLinks = "ContentAndLinks";
            public const string HeaderWithHrefs = "HeaderWithHrefs";
            public const string HtmlContent = "HtmlContent";
            public const string DigiDoc = "DigiDocContent";
            public const string SheetContent = "SheetContent";
            public const string About = "About";
            public const string Error = "Error";
        }

        public ActionResult Index (string id) {
            Trace.WriteLine ($"HomeController.Index({id})");

            ActionResult result = default;
            
            ViewBag.Message = id ?? "<null>";
            ViewBag.Id = id;
            if (id == nameof(About))
                return About ();
            if (id == nameof(Views.About))
                return Error ();

            var things = Backend.ResolveRequest (id);
            if (things == null) {
                return View ("About", new HtmlContent {
                    Description = "not found",
                    Data = $"{id}<br/>{"not found"}"
                });
            }

            void setViewBag (Content content) {
                ViewBag.Title = content.Description;
                ViewBag.Id = content.Source;
            }

            ActionResult linksOfThings (IEnumerable<IThing> ts) => View (Views.Hrefs, new Hrefs { Refs = Backend.HrefsOfThings (ts) });

            ActionResult contentAndLinks (IThing thing, Content c) {
                var leafs = Backend.HrefsOfThings (Backend.Leafs (thing)
                .OrderBy (t => Backend.ThingDataToDisplay (t)))
                .ToArray ();
                var roots = Backend.HrefsOfThings (Backend.Roots (thing)
                .OrderBy (t => Backend.ThingDataToDisplay (t)))
                .ToArray ();
                return View (Views.ContentAndLinks, new ContentAndLinks { Content = c, Leafs = leafs, Roots = roots });
            }

            ActionResult digidoc (IThing thing, Content c) {
                var doc = new DigidocSchema (Backend.ThingGraph, thing);
                var leafs = Backend.HrefsOfThings (doc.OrderedPages ());
                var roots = Backend.HrefsOfThings (Backend.Roots (thing).OrderBy (t => Backend.ThingDataToDisplay (t)));
                // TODO: what if pages are no images, but text or something else?
                return View (Views.DigiDoc, new ContentAndLinks { Content = c, Leafs = leafs, Roots = roots });
            }

            ActionResult sheet (IThing thing, Content c) {
                var hrefsOf = Backend.VisualHrefsOf (thing as IStreamThing);
                var leafs = Backend.HrefsOfThings (Backend.Leafs (thing)
                .OrderBy (t => Backend.ThingDataToDisplay (t)))
                .ToArray ();
                var roots = Backend.HrefsOfThings (Backend.Roots (thing)
                .OrderBy (t => Backend.ThingDataToDisplay (t)))
                .ToArray ();
                return View (Views.SheetContent, new VisualHrefContent { Hrefs = hrefsOf, Roots = roots, Description = c.Description, Leafs = leafs });
            }

            if (things.Count () == 1) {
                var thing = things.First ();
                if (thing is IStreamThing) {
                    if (Backend.IsConvertibleToHtml (thing)) {
                        var content = Backend.HtmlContent (thing as IStreamThing);
                        setViewBag (content);
                        result = contentAndLinks (thing, content);
                    } else if (((StreamThing)thing).StreamType == ContentTypes.LimadaSheet) {
                        var content = Backend.StreamContent (thing as StreamThing);
                        setViewBag (content);
                        result = sheet (thing, content);
                    } else {
                        var content = Backend.StreamContent (thing as StreamThing);
                        setViewBag (content);
                        result = File (content.Data, content.MimeType);
                    }
                } else if (thing != null) {
                    var content = new Content {
                        Description = Backend.ThingToDisplay (thing).ToString (),
                        Source = thing.Id.ToString ("X16"),
                    };
                    setViewBag (content);
                    var doc = new DigidocSchema (Backend.ThingGraph, thing);
                    if (doc.HasPages ()) {
                        result = digidoc (thing, content);
                    } else {
                        result = contentAndLinks (thing, content);
                    }

                }
            } else if (things.Count () > 1) {
                var content = new Content {
                    Description = id + " ...",
                    Source = "",
                };
                setViewBag (content);
                result = View (Views.ContentAndLinks,
                    new ContentAndLinks {
                        Content = content,
                        Leafs = Backend.HrefsOfThings (things).OrderBy (l => l.Text),
                        Roots = new Href[0]
                    });

            }

            return result ?? Error ();
        }

        /// <summary>
        /// resolves requests with aspx-style: xxx.aspx?id
        /// </summary>
        /// <returns></returns>
        public ActionResult AspxReqest () {
            var r = QueryHelpers.ParseQuery (Request.QueryString.ToString());
            return Index (r.Keys.FirstOrDefault());
        }

        public ActionResult About () {
            var content = new HtmlContent {
                Description = "About",
            };

            var mdFile = Path.Combine (AppController.ContentRootPath, "about.md");
            if (System.IO.File.Exists (mdFile)) {
                using var reader = new FileStream (mdFile, FileMode.Open);
                content.Data = new MarkDownHtmlConverter ().ToHtml (reader);
            } else {
                content.Data = "<p>Use this area to provide additional information.</p>";
            }

            return View (Views.About, content);
        }

//        public ActionResult Sandbox () {
//            var content = new HtmlContent {
//                Description = "HtmlContent-Example",
//                Data = string.Format ("SiteName: {0}<br/>DataBase: {1}",
//                    AppController.SiteName,
//                    AppController.Backend.Iori.ToString ()
//                )
//            };
//            return View (content);
//        }

        public ActionResult Error () {
            return View (Views.Error, new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier}
            );
        }
    }

}
