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
using Limaki.Actions;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.Widgets;
using Id = System.Int64;

namespace Limaki.Presenter.Widgets {
    public class WidgetDisplay: GraphSceneDisplay<IWidget,IEdgeWidget> {
        
        public virtual new Scene Data {
            get { return base.Data as Scene; }
            set { base.Data = value; }
        }

        public Id SceneId { get; set; }
        public string Text { get; set; }

        public event EventHandler<SceneEventArgs> SceneFocusChanged = null;

        SceneEventArgs focusChangedEventArgs = null;
        public virtual void SceneFocusChangedCallback(IGraphScene<IWidget, IEdgeWidget> scene, IWidget widget) {
            if (SceneFocusChanged != null) {
                focusChangedEventArgs = new SceneEventArgs(scene, widget);
            }
        }

        public override void DataChanged() {
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

        

    }

    public class WidgetRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<IGraphModelFactory<IWidget, IEdgeWidget>, WidgetFactory>();
        }
    }

    public class SceneEventArgs : EventArgs {
        public SceneEventArgs(IGraphScene<IWidget, IEdgeWidget> scene, IWidget widget) {
            this.Scene = scene;
            this.Widget = widget;
        }
        public IGraphScene<IWidget, IEdgeWidget> Scene;
        public IWidget Widget;
    }


}