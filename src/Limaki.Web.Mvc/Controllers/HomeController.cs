using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Contents;
using Limada.UseCases.Cms;
using Limada.UseCases.Cms.Models;


namespace Limaki.Web.Mvc.Controllers {

    public class HomeController : Controller {

        AppController _appController;
        public AppController AppController {
            get { return _appController ?? (_appController = Registry.Pooled<AppController> ()); }
        }

        public BackendController Backend {
            get { return AppController.Backend; }
        }

        protected override void Initialize (System.Web.Routing.RequestContext requestContext) {
            base.Initialize (requestContext);
            this.ViewBag.SiteName = AppController.SiteName;
        }

        public class Views {
            public const string Hrefs = "Hrefs";
            public const string ContentAndLinks = "ContentAndLinks";
            public const string HeaderWithHrefs = "HeaderWithHrefs";
            public const string HtmlContent = "HtmlContent";
            public const string DigiDoc = "DigiDocContent";
            public const string SheetContent = "SheetContent";
        }

        public ActionResult Index (string id) {
            Trace.WriteLine ("HomeController.Index({0})", id);
            ActionResult result = base.HttpNotFound ("not found");

            ViewBag.Message = id ?? "<null>";
            ViewBag.Id = id;

            var things = Backend.ResolveRequest (id);
            if (things == null) {
                return View ("About", new HtmlContent {
                    Description = "not found",
                    Data = string.Format ("{0}<br/>{1}", id, "not found")
                });
            }

            Action<Content> setViewBag = content => {
                ViewBag.Title = content.Description;
                ViewBag.Id = content.Source;
            };

            Func<IEnumerable<IThing>, ActionResult> linksOfThings =
                ts => View (Views.Hrefs, new Hrefs { Refs = Backend.HrefsOfThings (ts) });

            Func<IThing, Content, ActionResult> contentAndLinks = (thing, c) => {
                var leafs = Backend.HrefsOfThings (Backend.Leafs (thing)
                    .OrderBy (t => Backend.ThingDataToDisplay (t)))
                    .ToArray ();
                var roots = Backend.HrefsOfThings (Backend.Roots (thing)
                    .OrderBy (t => Backend.ThingDataToDisplay (t)))
                    .ToArray ();
                return View (Views.ContentAndLinks, new ContentAndLinks { Content = c, Leafs = leafs, Roots = roots });
            };

            Func<IThing, Content, ActionResult> digidoc = (thing, c) => {
                var doc = new DigidocSchema (Backend.ThingGraph, thing);
                var leafs = Backend.HrefsOfThings (doc.OrderedPages ());
                var roots = Backend.HrefsOfThings (Backend.Roots (thing).OrderBy (t => Backend.ThingDataToDisplay (t)));
                // TODO: what if pages are no images, but text or something else?
                return View (Views.DigiDoc, new ContentAndLinks { Content = c, Leafs = leafs, Roots = roots });
            };

            Func<IThing, Content, ActionResult> sheet = (thing, c) => {
                var hrefsOf = Backend.VisualHrefsOf (thing as IStreamThing);
                var leafs = Backend.HrefsOfThings (Backend.Leafs (thing)
                    .OrderBy (t => Backend.ThingDataToDisplay (t)))
                    .ToArray ();
                var roots = Backend.HrefsOfThings (Backend.Roots (thing)
                     .OrderBy (t => Backend.ThingDataToDisplay (t)))
                     .ToArray ();
                return View (Views.SheetContent, new VisualHrefContent { Hrefs = hrefsOf, Roots = roots, Description = c.Description, Leafs =leafs});
            };

            if (things.Count () == 1) {
                var thing = things.First ();
                if (thing is IStreamThing) {
                    if (Backend.IsConvertibleToHtml (thing)) {
                        var content = Backend.HtmlContent (thing as IStreamThing);
                        setViewBag (content);
                        result = contentAndLinks (thing, content);
                    } else if (((StreamThing) thing).StreamType == ContentTypes.LimadaSheet) {
                        var content = Backend.StreamContent (thing as StreamThing);
                        setViewBag (content);
                        result = sheet(thing, content);
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
            return result;
        }

        public ActionResult StreamContent (string id) {
            // disabled by now
            return base.HttpNotFound (id);
        }

        /// <summary>
        /// resolves requests with aspx-style: xxx.aspx?id
        /// </summary>
        /// <returns></returns>
        public ActionResult AspxReqest () {
            return Index ((Request.QueryString.Count > 0) ?
                Request.QueryString.Get (0) : null);
        }

        public ActionResult About () {
            var content = new HtmlContent {
                Description = "Settings",
                Data = string.Format ("{0}<br/>{1}",
                    AppController.SiteName,
                    AppController.Backend.Iori.ToString ()
                )
            };
            return View (content);
        }

        public ActionResult Sandbox () {
            var content = new HtmlContent {
                Description = "HtmlContent-Example",
                Data = string.Format ("SiteName: {0}<br/>DataBase: {1}",
                    AppController.SiteName,
                    AppController.Backend.Iori.ToString ()
                )
            };
            return View (content);
        }
    }
}
