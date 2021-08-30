/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Common;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz.Modelling {

    public abstract class Layout : ILayout {

        public Layout(IStyleSheet styleSheet) {
            this.StyleSheet = styleSheet;
            this.Distance = new Size(30, 15);
            this.Border = this.Distance;
        }
        
        public virtual IStyleSheet StyleSheet { get; set; }

        private IShapeFactory _shapeFactory = null;
        public virtual IShapeFactory ShapeFactory {
            get {
                if (_shapeFactory == null) {
                    _shapeFactory = Registry.Factory.Create<IShapeFactory>();
                }
                return _shapeFactory;
            }
            set { _shapeFactory = value; }
        }

        #region Painter-Handling
        private IPainterFactory _painterFactory = null;
        public virtual IPainterFactory PainterFactory {
            get {
                if (_painterFactory == null) {
                    _painterFactory = Registry.Pooled<IPainterFactory>();
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
                if (result != null)
                    painterCache.Add(type, result);
                else {
                    painterCache.Add(type, PainterFactory.CreatePainter(typeof(string)));
                }
            }
            return result;
        }
        #endregion

        #region Layout-Settings
        
        public Size Distance { get; set; }
        public Size Border { get; set; }
        #endregion

        public abstract void Reset();
    }

    public abstract class Layout<TItem> : Layout, ILayout<TItem> {
        
        public Layout ( IStyleSheet sheet ) : base (sheet){}

        #region Style-Handling

        public abstract IStyle GetStyle(TItem item);
        public abstract IStyle GetStyle(TItem item, UiState uiState);

        #endregion

        #region Layout-Methods

        /// <summary>
        /// performs a layout on a single item
        /// </summary>
        /// <param name="item"></param>
        public abstract bool Perform(TItem item);

        public abstract bool Perform(TItem item, IShape shape);

        /// <summary>
        /// Sets position of item
        /// </summary>
        public abstract void Justify(TItem item);

        public abstract void Justify(TItem item, IShape shape);

        /// <summary>
        /// Sets Shape and Style of item
        /// </summary>
        public abstract void Refresh(TItem item);

        #endregion

        #region Bounds-Handling

        public abstract void BoundsChanged(TItem item);
        public abstract Point[] GetDataHull(TItem item, Matrix matrix, int delta, bool extend);
        public abstract Point[] GetDataHull(TItem item, UiState uiState, Matrix matrix, int delta, bool extend);
        public abstract Point[] GetDataHull(TItem item, int delta, bool extend);
        public abstract Point[] GetDataHull(TItem item, UiState uiState, int delta, bool extend);

        #endregion

        #region Shape-Handling

        public abstract IShape CreateShape(TItem item);
        public abstract IShape GetShape(TItem item);

        #endregion

        public virtual IComparer<TItem> Comparer { get; set; }

    }


   


}