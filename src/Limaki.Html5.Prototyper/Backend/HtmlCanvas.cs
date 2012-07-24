using Xwt.Engine;
namespace Xwt.Html5.Backend {

    public class HtmlCanvas : HtmlChunk {

        public string ElementId { get; set; }

        public HtmlCanvas () : base ("canvas") { }

        Xwt.Drawing.Context _context = null;
        public Xwt.Drawing.Context Context {
            get { return _context ?? (_context = new Xwt.Drawing.Context (new Html5Context ())); }
        }

        public override string Render () {
            var ctx = ((Html5Context) WidgetRegistry.GetBackend (Context));
            return string.Format (@"function() {{
                var {0} = document.getElementById(""{1}"");
                var {2} = {0}.getContext(""2d"");
                {3} }};",
             this.Target, this.ElementId, ctx.Context.Target, ctx.Context.Render ());
        }
    }
}