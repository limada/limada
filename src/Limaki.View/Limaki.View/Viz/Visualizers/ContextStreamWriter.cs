using System;
using Xwt;
using Xwt.Drawing;
using System.IO;

namespace Limaki.View.Viz.Visualizers {

    public interface IContextWriter {
        /// <summary>
        /// draw is added to an internal delegate
        /// the delegates are called in Flush()
        /// </summary>
        /// <param name="draw"></param>
        void PushPaint (Action<Context> draw);

        Size CanvasSize { get; set; }
        void ClearPaint ();

        /// <summary>
        /// makes the concrete Paint:
        /// executes the internal multicast delegate with the Painters Context as parameter 
        /// </summary>
        void Flush ();

        void Write (Stream file);

        object Switch();
        void Restore (object saved);

    }

    public abstract class ContextWriter : IContextWriter {

        protected Action<Context> paintStack = null;

        /// <summary>
        /// draw is added to an internal delegate
        /// the delegates are called in Flush()
        /// </summary>
        /// <param name="draw"></param>
        public virtual void PushPaint (Action<Context> draw) {
            this.paintStack += draw;
        }

        public virtual Size CanvasSize { get; set; }

        public virtual void ClearPaint () { paintStack = null; }

        /// <summary>
        /// makes the concrete Paint:
        /// executes the internal multicast delegate with the Painters Context as parameter 
        /// </summary>
        public abstract void Flush ();

        public abstract void Write (Stream stream);
        
        public abstract object Switch();

        public abstract void Restore (object saved);
    }
}