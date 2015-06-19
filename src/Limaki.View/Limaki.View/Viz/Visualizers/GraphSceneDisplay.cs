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
 */

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.Common.Linqish;
using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Actions;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.UI.GraphScene;
using Xwt.Drawing;

namespace Limaki.View.Viz.Visualizers {

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

        SceneInfo _info = default(SceneInfo);
        public virtual SceneInfo Info {
            get {
                if(_info == null) {
                    _info = new SceneInfo ();
                }
                _info.Id = this.DataId;
                _info.Name = this.Text;
                this.State.CopyTo(_info.State);
                return _info;
            }
            set {
                if (_info != value) {
                    _info = value;
                }
                this.DataId = _info.Id;
                _info.State.CopyTo(this.State);
                this.Text = _info.Name;
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

        IGraphSceneLayout<TItem, TEdge> _layout = null;
        public virtual IGraphSceneLayout<TItem, TEdge> Layout {
            get { return _layout; }
            set {
                if (_layout != value) {
                    _layout = value;
                    _layout.StyleSheet = this.StyleSheet;
                    LayoutChanged ();
                }
            }
        }

        public virtual IGraphSceneModeller<TItem, TEdge> GraphSceneModeller { get; set; }
        public virtual ICommandModeller<TItem> CommandModeller { get; set; }

        public virtual IGraphItemRenderer<TItem, TEdge> GraphItemRenderer { get; set; }
        
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

        public event EventHandler<GraphSceneEventArgs<TItem, TEdge>> SceneFocusChanged = null;
        protected GraphSceneEventArgs<TItem, TEdge> focusChangedEventArgs = null;
        public virtual void SceneFocusChangedCallback(IGraphScene<TItem, TEdge> scene, TItem item) {
            if (SceneFocusChanged != null) {
                focusChangedEventArgs = new GraphSceneEventArgs<TItem, TEdge>(scene, item);
            }
        }

        public virtual void OnSceneFocusChanged() {
            if (SceneFocusChanged != null && focusChangedEventArgs != null) {

                SceneFocusChanged(this, focusChangedEventArgs);
                focusChangedEventArgs = null;

                ActionDispatcher.Actions.OfType<MouseTimerActionBase> ()
                    .ForEach (a => a.LastMouseTime = 0);

            }
        }

        public override void BeforeDataChange (IGraphScene<TItem, TEdge> old) {
            base.BeforeDataChange (old);
            if (old != null)
                old.FocusChanged -= SceneFocusChangedCallback;
        }

        public override void DataChanged() {
            this.Text = "";
            this.DataId = 0;
            this._info = null;

            base.DataChanged();

            if (this.Data != null) {
                this.Data.FocusChanged -= SceneFocusChangedCallback;
                this.Data.FocusChanged += SceneFocusChangedCallback;
            }
        }

        public override bool Check() {
            if (this.Layout == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphSceneLayout<TItem, TEdge>));
            }

            if (ModelFactory == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphModelFactory<TItem, TEdge>));
            }
            return base.Check();
        }
        
    }
}