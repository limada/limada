using Xwt;
using Xwt.Html5.Backend;
using Xwt.Backends;

namespace Limaki.View.Html5 {

    public class Html5Canvas : HtmlChunk {

        public string ElementId { get; set; }

        public Html5Canvas () : base ("canvas") { }

        Xwt.Drawing.Context _context = null;
        public Xwt.Drawing.Context Context {
            get { return _context ?? (_context = new Xwt.Drawing.Context (new Html5Context (),Toolkit.Engine<Html5Engine>())); }
        }

        public override string Html () {

            var ctx = (Html5Context)Context.GetBackend();

            return string.Format (@"function() {{
                var {0} = document.getElementById(""{1}"");
                var {2} = {0}.getContext(""2d"");
                {3} }};",
             this.Target, this.ElementId, ctx.Context.Target, ctx.Context.Html ());
        }
    }
}