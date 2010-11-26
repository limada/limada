/*
 * Limaki 
 * Version 0.063
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
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Actions;

namespace Limaki.Widgets {

    public class Renderer {

		///<label>Parent</label>
		Renderer Parent = null;
        public Renderer(Renderer parent) {
            this.Parent = parent;
        }

        ///<directed>True</directed>
        Scene _scene = null;
        public virtual Scene Scene {
            get {
                if ((_scene == null) && (Parent != null)) {
                    return Parent.Scene;
                }
                return _scene;
            }
            set { _scene = value; }
        }

        private ILayout<Scene,IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get {
                if ((_layout == null) && (Parent != null)) {
                    return Parent.Layout;
                }
                return _layout;
            }
            set { _layout = value; }
        }

        private IStyleSheet _styleSheet = null;
        public virtual IStyleSheet StyleSheet {
            get {
                if ((_styleSheet == null) && (Parent != null)) {
                    return Parent.StyleSheet;
                }
                return _styleSheet;
            }
            set { _styleSheet = value; }
        }

        private PainterFactory _painterFactory = null;
        public virtual PainterFactory PainterFactory {
            get {
                if ((_painterFactory == null) && (Parent != null)) {
                    return Parent.PainterFactory;
                }
                return _painterFactory;
            }
            set { _painterFactory = value; }
        }

        private Dictionary<Type, IPainter> _painterCache = null;
        protected Dictionary<Type, IPainter> PainterCache {
            get {
                if (_painterCache == null) {
                    if (Parent != null) {
                        return Parent.PainterCache;
                    } else {
                        _painterCache = new Dictionary<Type, IPainter>();
                    }
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
                painterCache.Add(type, result);
            }
            return result;
        }
    }
}