using Limaki.Common;
using Limaki.Drawing;
using System.Collections.Generic;
using Limaki.Drawing.Painters;
using System;
using System.Drawing;

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

        private IStyleSheet _styleSheet = null;
        public virtual IStyleSheet StyleSheet {
            get { return _styleSheet; }
            set { _styleSheet = value; }
        }

        public Layout(Handler<TData> dataHandler,IStyleSheet styleSheet) {
            this.DataHandler = dataHandler;
            this.StyleSheet = styleSheet;
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
        public abstract IStyle GetStyle(TItem item, UiState uiState);
        public abstract Point[] GetDataHull ( TItem item, Matrice matrix, int delta, bool extend );
        public abstract Point[] GetDataHull(TItem item, UiState uiState, Matrice matrix, int delta, bool extend);

        private PainterFactory _painterFactory = null;
        public virtual PainterFactory PainterFactory {
            get {
                if (_painterFactory == null) {
                    return new PainterFactory();
                }
                return _painterFactory;
            }
            set { _painterFactory = value; }
        }

        private Dictionary<Type, IPainter> _painterCache = null;
        protected Dictionary<Type, IPainter> PainterCache {
            get {
                if (_painterCache == null) {
                    _painterCache = new Dictionary<Type, IPainter>();
                }
                return _painterCache;
            }
            set { _painterCache = value; }
        }



        public virtual IPainter GetPainter(Type type) {
            IPainter result = null;
            Dictionary<Type, IPainter> painterCache = PainterCache;
            painterCache.TryGetValue(type, out result);
            if (result == null) {
                result = PainterFactory.CreatePainter(type);
                if (result!=null)
                    painterCache.Add(type, result);
                else {
                    painterCache.Add (type, new StringPainter ());
                }
            }
            return result;
        }
    }

    public class Row<TItem> {
        public IList<TItem> Items = new List<TItem>();
        public Point Location;
        public Size biggestWidgetSize;
        public Size Size;

        public Row() { }
    }
}