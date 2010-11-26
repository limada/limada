/*
 * Limaki 
 * Version 0.071
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

using System.Drawing;
using System.ComponentModel;
using Limaki.Winform;
using Limaki.Widgets;
using Limaki.Winform.Widgets;
using Limaki.Actions;

namespace Limaki.Winform.Displays {
    public class WidgetDisplay : DisplayBase<Scene> {
        public WidgetDisplay():base() {
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

		///<directed>True</directed>
		WidgetKit _kit = null;
        public override DisplayKit<Scene> displayKit {
            get {
                if (_kit == null) {
                    _kit = new WidgetKit();
                }
                return _kit;
            }
        }

        public override ZoomAction ZoomAction {
            get {
                if (_zoomAction == null) {
                    EventControler.Add(_zoomAction =
                        new WidgetLayerZoomAction(this.DataHandler, this, this, DataLayer.Camera));
                    ( (WidgetLayerZoomAction) _zoomAction ).HitSize = displayKit.HitSize;
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
                        _kit.WidgetChanger (this, this.DataLayer.Camera));
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
                        _kit.AddWidgetAction(this, this.DataLayer.Camera));
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
                    EventControler.Add(_widgetDeleter = _kit.WidgetDeleter(this, DataLayer.Camera));
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
                    EventControler.Add(_widgetSelector = _kit.WidgetSelector(this,DataLayer.Camera));
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
                    EventControler.Add(_widgetTextEditor = _kit.WidgetEditor(this, DataLayer.Camera));
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
                    EventControler.Add(_addEdgeAction = _kit.AddLinkAction(this, DataLayer.Camera));
                }
                return _addEdgeAction;
            }
            set { EventControler.Add(value, ref _addEdgeAction); }
        }

        protected EdgeWidgetChanger _linkWidgetChanger = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual EdgeWidgetChanger EdgeWidgetChanger {
            get {
                if (_linkWidgetChanger == null) {
                    EventControler.Add(_linkWidgetChanger =
                        _kit.LinkWidgetChanger(this, DataLayer.Camera));
                }
                return _linkWidgetChanger;
            }
            set { EventControler.Add(value, ref _linkWidgetChanger); }
        }

        protected ILayoutControler _layoutControler = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ILayoutControler LayoutControler {
            get {
                if (_layoutControler == null) {
                    EventControler.Add(_layoutControler = _kit.CommandsAction(this, this,DataLayer.Camera));
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
                    EventControler.Add(_widgetDragDrop = _kit.WidgetDragDrop(this, DataLayer.Camera));
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
    }
}
