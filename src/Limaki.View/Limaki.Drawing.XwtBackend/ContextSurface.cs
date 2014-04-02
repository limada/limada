using Limaki.Drawing;
using Xwt.Drawing;

namespace Limaki.Drawing.XwtBackend {

    /// <summary>
    /// a Surface providing a Xwt.Drawing.Context
    /// </summary>
    public class ContextSurface : ISurface {

        public virtual Context Context { get; set; }

        private Matrix _matrix = null;
        public virtual Matrix Matrix {
            get { return _matrix ?? (_matrix = new Matrix()); }
            set { _matrix = value; }
        }
    }
}