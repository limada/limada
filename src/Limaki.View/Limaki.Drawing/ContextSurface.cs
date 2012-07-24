using Xwt.Drawing;
namespace Limaki.Drawing {
    public class ContextSurface : ISurface {
        public virtual Context Context { get; set; }
        public virtual Matrice Matrix { get; set; }
    }
}