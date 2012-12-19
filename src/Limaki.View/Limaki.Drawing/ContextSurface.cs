using Xwt.Drawing;
namespace Limaki.Drawing {
    /// <summary>
    /// a Surface providing a Xwt.Context
    /// </summary>
    public class ContextSurface : ISurface {
        public virtual Context Context { get; set; }
        public virtual Matrix Matrix { get; set; }
    }
}