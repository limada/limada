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
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.UI;
using Limaki.Winform.Widgets;
using Limaki.Drawing.GDI;

namespace Limaki.Winform.Displays {
    public class WidgetDisplay : DisplayBase<Scene> {
        public WidgetDisplay():base() {
            if (!this.DesignMode) {
                LayoutControler.Enabled = true;
                WidgetSelector.Enabled = true;
                WidgetChanger.Enabled = true;
                SelectAction.Enabled = false;
                EdgeWidgetChanger.Enabled = true;
                AddEdgeAction.Enabled = false;
                AddWidgetAction.Enabled = false;
                WidgetDeleter.Enabled = true;
                WidgetTextEditor.Enabled = true;
                WidgetDragDrop.Enabled = true;
                WidgetFolding.Enabled = true;
            }
        }

		WidgetKit _kit = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DisplayKit<Scene> DisplayKit {
            get {
                if (_kit == null) {
                    _kit = new WidgetKit();
                }
                return _kit;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SceneId { get; set; }


        public override System.Drawing.Color BackColor {
            get {
                if (DataLayout != null && DataLayout.StyleSheet != null) {
                    return GDIConverter.Convert(DataLayout.StyleSheet.BackColor);
                }
                return base.BackColor;

            }
            set {
                base.BackColor = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ILayout<Scene,IWidget> DataLayout {
            get { return ( (WidgetKit) DisplayKit ).Layout;}
            set {
                if (((WidgetKit)DisplayKit).Layout != value) {
                    base.BackColor = GDIConverter.Convert(value.StyleSheet.BackColor);
                    ( (WidgetKit) DisplayKit ).Layout = value;
                    ( (WidgetLayerBase) this.DataLayer ).Layout = value;
                    this.AddEdgeAction.Layout = value;
                    this.LayoutControler.Layout = value;
                    this.AddWidgetAction.Layout = value;
                    this.WidgetTextEditor.Layout = value;
                    this.WidgetDragDrop.Layout = value;
                    this.WidgetFolding.Layout = value;
                }

            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ZoomAction ZoomAction {
            get {
                if (_zoomAction == null) {
                    EventControler.Add(_zoomAction =
                        new WidgetLayerZoomAction(this.DataHandler, this, this, this.Camera));
                    ( (WidgetLayerZoomAction) _zoomAction ).HitSize = DisplayKit.HitSize;
                }
                return _zoomAction;
            }
            set { EventControler.Add(value, ref _zoomAction); }
        }

        protected WidgetChanger _widgetChanger = null;
        [Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetChanger WidgetChanger {
            get {
                if (_widgetChanger == null) {
                    EventControler.Add (_widgetChanger = 
                        _kit.WidgetChanger (this, this.Camera));
                }
                return _widgetChanger;
            }
            set { EventControler.Add(value, ref _widgetChanger); }
        }

        protected AddWidgetAction _addWidgetAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual AddWidgetAction AddWidgetAction {
            get {
                if (_addWidgetAction == null) {
                    EventControler.Add(_addWidgetAction = 
                        _kit.AddWidgetAction(this, this.Camera));
                }
                return _addWidgetAction;
            }
            set { EventControler.Add(value, ref _addWidgetAction); }
        }

        protected WidgetDeleter _widgetDeleter = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetDeleter WidgetDeleter {
            get {
                if (_widgetDeleter == null) {
                    EventControler.Add(_widgetDeleter = _kit.WidgetDeleter(this, this.Camera));
                }
                return _widgetDeleter;
            }
            set { EventControler.Add(value, ref _widgetDeleter); }
        }

		protected WidgetSelector _widgetSelector = null;
        
        [Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetSelector WidgetSelector {
            get {
                if (_widgetSelector == null) {
                    EventControler.Add(_widgetSelector = _kit.WidgetSelector(this,this.Camera));
                }
                return _widgetSelector;
            }
            set { EventControler.Add(value, ref _widgetSelector); }
        }


        protected WidgetTextEditor _widgetTextEditor = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetTextEditor WidgetTextEditor {
            get {
                if (_widgetTextEditor == null) {
                    EventControler.Add(_widgetTextEditor = _kit.WidgetEditor(this, this.Camera));
                }
                return _widgetTextEditor;
            }
            set { EventControler.Add(value, ref _widgetTextEditor); }
        }

        protected AddEdgeAction _addEdgeAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual AddEdgeAction AddEdgeAction {
            get {
                if (_addEdgeAction == null) {
                    EventControler.Add(_addEdgeAction = _kit.AddLinkAction(this, this.Camera));
                }
                return _addEdgeAction;
            }
            set { EventControler.Add(value, ref _addEdgeAction); }
        }

        protected EdgeWidgetChanger _edgeWidgetChanger = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual EdgeWidgetChanger EdgeWidgetChanger {
            get {
                if (_edgeWidgetChanger == null) {
                    EventControler.Add(_edgeWidgetChanger =
                        _kit.LinkWidgetChanger(this, this.Camera));
                }
                return _edgeWidgetChanger;
            }
            set { EventControler.Add(value, ref _edgeWidgetChanger); }
        }

        protected ILayoutControler _layoutControler = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ILayoutControler LayoutControler {
            get {
                if (_layoutControler == null) {
                    EventControler.Add(_layoutControler = _kit.LayoutControler(this, this,this.Camera));
                }
                return _layoutControler;
            }
            set { EventControler.Add(value, ref _layoutControler); }
        }

        protected WidgetDragDrop _widgetDragDrop = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetDragDrop WidgetDragDrop {
            get {
                if (_widgetDragDrop == null) {
                    EventControler.Add(_widgetDragDrop = _kit.WidgetDragDrop(this, this.Camera));
                }
                return _widgetDragDrop;
            }
            set { EventControler.Add(value, ref _widgetDragDrop); }
        }

        protected WidgetFolding _widgetFolding = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetFolding WidgetFolding {
            get {
                if (_widgetFolding == null) {
                    EventControler.Add(_widgetFolding = _kit.WidgetFolding(this));
                }
                return _widgetFolding;
            }
            set { EventControler.Add(value, ref _widgetFolding); }
        }

        public event EventHandler<SceneEventArgs> SceneFocusChanged=null;

        SceneEventArgs focusChangedEventArgs = null;
        public virtual void SceneFocusChangedCallback(Scene scene, IWidget widget) {
            if (SceneFocusChanged != null) {
                focusChangedEventArgs = new SceneEventArgs (scene, widget);
            }
        }

        protected override void DataChanged() {
            base.DataChanged();
            if (this.Data != null) {
                this.Data.FocusChanged += SceneFocusChangedCallback;
            }
        }

        public virtual void OnSceneFocusChanged() {
            if (SceneFocusChanged != null && focusChangedEventArgs != null) {
                int start = Environment.TickCount;

                SceneFocusChanged(this, focusChangedEventArgs);
                focusChangedEventArgs = null;

                //int now = Environment.TickCount;
                //System.Console.Out.WriteLine("Start/Elapsed FocusChanged:\t" + start+"/"+(now - start));

                foreach (KeyValuePair<Type, IAction> action in this.EventControler.Actions) {
                    if (action.Value is MouseTimerActionBase) {
                        ((MouseTimerActionBase)action.Value).LastMouseTime = 0;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            
            base.OnMouseDown(e);
            OnSceneFocusChanged ();

        }
    }

    public class SceneEventArgs : EventArgs {
        public SceneEventArgs (Scene scene, IWidget widget) {
            this.Scene = scene;
            this.Widget = widget;
        }
        public Scene Scene;
        public IWidget Widget;
    }


}
