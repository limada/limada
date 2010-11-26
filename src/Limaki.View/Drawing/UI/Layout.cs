/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.Drawing.UI {
    public abstract class Layout<TData, TItem> : ILayout<TData, TItem> {
        public Layout(Func<TData> dataHandler, IStyleSheet styleSheet) {
            this.DataHandler = dataHandler;
            this.StyleSheet = styleSheet;
        }

        #region Data
        private Func<TData> _dataHandler = null;
        public Func<TData> DataHandler {
            get { return _dataHandler; }
            set { _dataHandler = value; }
        }

        public virtual TData Data {
            get { return DataHandler(); }
        }
        #endregion 

        #region Style-Handling
        private IStyleSheet _styleSheet = null;
        public virtual IStyleSheet StyleSheet {
            get { return _styleSheet; }
            set { _styleSheet = value; }
        }
        public abstract IStyle GetStyle(TItem item);
        public abstract IStyle GetStyle(TItem item, UiState uiState);
        #endregion 

        
        #region Layout-Methods
        public abstract void Invoke();
        /// <summary>
        /// performs a layout on a single item
        /// </summary>
        /// <param name="item"></param>
        public abstract bool Invoke(TItem item);

        public abstract bool Invoke(TItem item, IShape shape);

        /// <summary>
        /// Sets position of item
        /// </summary>
        public abstract void Justify(TItem item);

        public abstract void Justify(TItem item, IShape shape);

        /// <summary>
        /// Sets Shape and Style of item
        /// </summary>
        public abstract void Perform(TItem item);

        

        #endregion

        #region Bounds-Handling

        public abstract void AddBounds(TItem item);
        public abstract PointI[] GetDataHull(TItem item, Matrice matrix, int delta, bool extend);
        public abstract PointI[] GetDataHull(TItem item, UiState uiState, Matrice matrix, int delta, bool extend);
        
        #endregion 

        #region Shape-Handling
        private IShapeFactory _shapeFactory = null;
        public virtual IShapeFactory ShapeFactory {
            get {
                if (_shapeFactory == null) {
                    _shapeFactory = Registry.Factory.One<IShapeFactory>();
                }
                return _shapeFactory;
            }
            set { _shapeFactory = value; }
        }

        public abstract IShape CreateShape ( TItem item );
        #endregion 
        
        #region Painter-Handling
        private IPainterFactory _painterFactory = null;
        public virtual IPainterFactory PainterFactory {
            get {
                if (_painterFactory == null) {
                    _painterFactory =  Registry.Pool.TryGetCreate<IPainterFactory>();
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
                    painterCache.Add(type, PainterFactory.CreatePainter(typeof(string)));
                }
            }
            return result;
        }
        #endregion

        #region Layout-Settings
        private SizeI _distance = new SizeI(30, 15);
        public SizeI Distance {
            get { return _distance; }
            set { _distance = value; }
        }
        #endregion
    }

    public class Row<TItem> {
        public IList<TItem> Items = new List<TItem>();
        public PointI Location;
        public SizeI biggestItemSize;
        public SizeI Size;

        public Row() { }
    }
}