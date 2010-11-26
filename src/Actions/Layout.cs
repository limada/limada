using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.Actions {
    public abstract class Layout<TData, TItem> : ILayout<TData, TItem> {
        private Handler<TData> _dataHandler = null;
        public Handler<TData> DataHandler {
            get { return _dataHandler; }
            set { _dataHandler = value; }
        }

        public virtual TData Data {
            get { return DataHandler(); }
        }

        protected IStyleSheet styleSheet = null;

        public Layout(Handler<TData> dataHandler,IStyleSheet styleSheet) {
            this.DataHandler = dataHandler;
            this.styleSheet = styleSheet;
        }

        /// <summary>
        /// performs a layout on the whole data by calling algorithm
        /// </summary>
        public abstract void Invoke();

        /// <summary>
        /// performs a layout on a single item
        /// </summary>
        /// <param name="item"></param>
        public abstract void Invoke(TItem item);

        /// <summary>
        /// Sets position of item
        /// </summary>
        public abstract void Justify(TItem item);

        /// <summary>
        /// Sets Shape and Style of item
        /// </summary>
        public abstract void Perform(TItem item);
        
        public abstract IStyle GetStyle(TItem item);

    }
}