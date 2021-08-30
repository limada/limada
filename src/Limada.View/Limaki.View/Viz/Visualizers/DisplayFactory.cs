using Limaki.Common;

namespace Limaki.View.Viz.Visualizers {

    public class DisplayFactory<TData> {

        public virtual Display<TData> Create() {
            var result = new Display<TData>();
            return result;
        }

        protected IComposer<Display<TData>> _displayComposer = null;
        public virtual IComposer<Display<TData>> DisplayComposer {
            get {
                if (_displayComposer == null) {
                    _displayComposer = new DisplayComposer<TData>();
                }
                return _displayComposer;
            }
            set { _displayComposer = value; }
        }

        public virtual IComposer<Display<TData>> BackendComposer { get; set; }

        public virtual void Compose(Display<TData> display) {
            DisplayComposer.Factor(display);
            BackendComposer.Factor(display);

            BackendComposer.Compose(display);
            DisplayComposer.Compose(display);

        }
    }
}