using System;
using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.UseCases.Viewers {
    public abstract class ViewerController : IDisposable {
        public Int64 CurrentThingId { get; set; }
        
        public virtual void OnShow() { }

        public abstract object Control { get; }

        public Color BackColor = KnownColors.FromKnownColor(KnownColor.Control);
        public object Parent { get; set; }

        public event Action<object> Attach = null;
        public event Action<object> DeAttach = null;

        protected virtual void OnAttach(object control) {
            if (Attach != null) {
                Attach(control);
            }
        }

        public abstract void Dispose();
        public virtual void Clear() {
            CurrentThingId = 0;
        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }
    }
}