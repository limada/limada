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

using System.Drawing;
using System.ComponentModel;
using Limaki.Winform;
using Limaki.Widgets;
using Limaki.Winform.Widgets;
using Limaki.Actions;

namespace Limaki.Displays {
    public class WidgetDisplay : DisplayBase<Scene> {
        public WidgetDisplay():base() {
            CommandAction.Enabled = true;
            
            WidgetSelector.Enabled = true;
            WidgetChanger.Enabled = true;
            SelectAction.Enabled = false;
            LinkWidgetChanger.Enabled = true;
            AddLinkAction.Enabled = false;
            AddWidgetAction.Enabled = false;
            WidgetDeleter.Enabled = true;
            WidgetTextEditor.Enabled = true;
            WidgetDragDrop.Enabled = true;

            
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
                    ActionDispatcher.Add(_zoomAction =
                        new WidgetLayerZoomAction(this.DataHandler, this, this, DataLayer.Transformer));
                }
                return _zoomAction;
            }
            set { ActionDispatcher.Add(value, ref _zoomAction); }
        }

        protected WidgetChanger _widgetChanger = null;
        [Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetChanger WidgetChanger {
            get {
                if (_widgetChanger == null) {
                    ActionDispatcher.Add (_widgetChanger = 
                        _kit.WidgetChanger (this, this.DataLayer.Transformer));
                }
                return _widgetChanger;
            }
            set { ActionDispatcher.Add(value, ref _widgetChanger); }
        }

        protected AddWidgetAction _addWidgetAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual AddWidgetAction AddWidgetAction {
            get {
                if (_addWidgetAction == null) {
                    ActionDispatcher.Add(_addWidgetAction = 
                        _kit.AddWidgetAction(this, this.DataLayer.Transformer));
                }
                return _addWidgetAction;
            }
            set { ActionDispatcher.Add(value, ref _addWidgetAction); }
        }

        protected WidgetDeleter _widgetDeleter = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetDeleter WidgetDeleter {
            get {
                if (_widgetDeleter == null) {
                    ActionDispatcher.Add(_widgetDeleter = _kit.WidgetDeleter(this, DataLayer.Transformer));
                }
                return _widgetDeleter;
            }
            set { ActionDispatcher.Add(value, ref _widgetDeleter); }
        }

		protected WidgetSelector _widgetSelector = null;
        [Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetSelector WidgetSelector {
            get {
                if (_widgetSelector == null) {
                    ActionDispatcher.Add(_widgetSelector = _kit.WidgetSelector(this,DataLayer.Transformer));
                }
                return _widgetSelector;
            }
            set { ActionDispatcher.Add(value, ref _widgetSelector); }
        }


        protected WidgetTextEditor _widgetTextEditor = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetTextEditor WidgetTextEditor {
            get {
                if (_widgetTextEditor == null) {
                    ActionDispatcher.Add(_widgetTextEditor = _kit.WidgetEditor(this, DataLayer.Transformer));
                }
                return _widgetTextEditor;
            }
            set { ActionDispatcher.Add(value, ref _widgetTextEditor); }
        }

        protected AddLinkAction _addLinkAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual AddLinkAction AddLinkAction {
            get {
                if (_addLinkAction == null) {
                    ActionDispatcher.Add(_addLinkAction = _kit.AddLinkAction(this, DataLayer.Transformer));
                }
                return _addLinkAction;
            }
            set { ActionDispatcher.Add(value, ref _addLinkAction); }
        }

        protected LinkWidgetChanger _linkWidgetChanger = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual LinkWidgetChanger LinkWidgetChanger {
            get {
                if (_linkWidgetChanger == null) {
                    ActionDispatcher.Add(_linkWidgetChanger =
                        _kit.LinkWidgetChanger(this, DataLayer.Transformer));
                }
                return _linkWidgetChanger;
            }
            set { ActionDispatcher.Add(value, ref _linkWidgetChanger); }
        }

        protected ICommandAction _commandAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ICommandAction CommandAction {
            get {
                if (_commandAction == null) {
                    ActionDispatcher.Add(_commandAction = _kit.CommandsAction(this, this,DataLayer.Transformer));
                }
                return _commandAction;
            }
            set { ActionDispatcher.Add(value, ref _commandAction); }
        }

        protected WidgetDragDrop _widgetDragDrop = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WidgetDragDrop WidgetDragDrop {
            get {
                if (_widgetDragDrop == null) {
                    ActionDispatcher.Add(_widgetDragDrop = _kit.WidgetDragDrop(this, DataLayer.Transformer));
                }
                return _widgetDragDrop;
            }
            set { ActionDispatcher.Add(value, ref _widgetDragDrop); }
        }
    }
}
