using Xwt.Drawing;
namespace Limaki.Drawing {
    public class ContextSurface : ISurface {
        public virtual Context Context { get; protected set; }
        public virtual Matrice Matrix { get; protected set; }
    }
}