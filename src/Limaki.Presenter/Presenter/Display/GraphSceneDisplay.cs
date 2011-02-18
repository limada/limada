/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */


using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.Presenter.UI;


namespace Limaki.Presenter.Display {
    public class GraphSceneDisplay<TItem, TEdge> : Display<IGraphScene<TItem, TEdge>>, IGraphSceneDisplay<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public override State State {
            get {
                if (Data != null)
                    return Data.State;
                else {
                    base.State.Hollow = true;
                    return base.State;
                }
            }
        }

        public virtual IGraphModelFactory<TItem, TEdge> ModelFactory { get; set; }

        public override IStyleSheet StyleSheet {
            get { return base.StyleSheet; }
            set {
                if (base.StyleSheet != value) {
                    base.StyleSheet = value;
                    if (this.Layout != null) {
                        this.Layout.StyleSheet = value;
                    }
                    LayoutChanged ();
                }
            }
        }

        IGraphLayout<TItem, TEdge> _layout = null;
        public virtual IGraphLayout<TItem, TEdge> Layout {
            get { return _layout; }
            set {
                if (_layout != value) {
                    _layout = value;
                    _layout.StyleSheet = this.StyleSheet;
                    LayoutChanged ();
                }
            }
        }

        public override Color BackColor {
            get {
                if (Layout != null && Layout.StyleSheet != null) {
                    return  Layout.StyleSheet.BackColor;
                }
                return base.BackColor;
            }
            set {
                if (Layout != null && Layout.StyleSheet != null) {
                    base.BackColor = Layout.StyleSheet.BackColor;
                } else {
                    base.BackColor = value;
                }
            }
        }

        public virtual void LayoutChanged() {
            if (Layout != null && Layout.StyleSheet != null) {
                base.BackColor = Layout.StyleSheet.BackColor;
            }
        }

        public virtual ISceneReceiver<TItem, TEdge> SceneReceiver { get; set; }
        public virtual IModelReceiver<TItem> ModelReceiver { get; set; }

        public virtual IGraphItemRenderer<TItem, TEdge> GraphItemRenderer { get; set; }

        public override bool Check() {
            if (this.Layout == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphLayout<TItem, TEdge>));
            }

            if (ModelFactory == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphModelFactory<TItem, TEdge>));
            }
            return base.Check();
        }
        
    }
}