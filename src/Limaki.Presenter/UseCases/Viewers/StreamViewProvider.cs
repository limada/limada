/*
 * Limada 
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


using System;
using System.Collections.Generic;
using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.UseCases.Viewers.StreamViewers;
using Limaki.Presenter.Widgets;
using Limaki.UseCases.Viewers.StreamViewers;
using Limaki.Widgets;

namespace Limaki.UseCases.Viewers {
    public class StreamViewProvider:IDisposable {
        
        public WidgetDisplay sheetControl = null;
        public ISheetManager sheetManager = null;

        public Color BackColor = KnownColors.FromKnownColor(KnownColor.Control);
        public object Parent = null;

        public ICollection<StreamViewerController> StreamViewers = new List<StreamViewerController> ();

        public event Action<object, Action> AfterStreamLoaded = null;
        public event Action<object> Attach = null;
        public event Action<object> DeAttach = null;

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler> (); }
        }

        IStreamThing currentThing = null;
        bool IsStreamOwner = true;

        void LoadStream(IThingGraph graph, IStreamThing thing) {
            if (thing != null) {
                foreach(var controller in StreamViewers) {
                    if (controller.CanView(thing.StreamType)) {
                        try {
                            if (controller.CurrentThingId != thing.Id) {
                                SaveStream (graph, controller);
                            }
                            
                            controller.IsStreamOwner = IsStreamOwner;

                            var info = ThingStreamFacade.GetStreamInfo(graph, thing);
                            controller.BackColor = this.BackColor;
                            controller.Parent = this.Parent;
                            if (this.Attach != null) {
                                controller.Attach -= this.Attach;
                                controller.Attach += this.Attach;
                            }

                            if (controller is SheetViewerController) {
                                var sheetView = (SheetViewerController)controller;
                                sheetView.sheetControl = this.sheetControl;
                                sheetView.sheetManager = this.sheetManager;
                                info.Source = thing.Id;
                            }

                            if (controller is HTMLViewerController) {
                                var htmlViewr = (HTMLViewerController)controller;
                                htmlViewr.ContentThing = thing;
                                htmlViewr.ThingGraph = graph;
                            }

                            if (controller.CurrentThingId != thing.Id) {
                                controller.SetContent (info);
                                controller.CurrentThingId = thing.Id;
                            }

                            DoAfterStreamLoaded (controller.Control,()=>{controller.OnShow();});
                            
                            currentThing = thing;

                        } catch (Exception ex) {
                            ExceptionHandler.Catch (ex, MessageType.OK);
                        } finally {
                            thing.ClearRealSubject (!IsStreamOwner);
                        }
                        break;
                    }
                }
            }
        }


        protected void DoAfterStreamLoaded(object control, Action onShowAction) {
            if (AfterStreamLoaded != null) {
                AfterStreamLoaded (control, onShowAction);
            }
        }


        public void SaveStream(IThingGraph graph, StreamViewerController controller) {
            if (graph != null && controller.CanSave() && controller.CurrentThingId != 0){
                var thing = graph.GetById(controller.CurrentThingId) as IStreamThing;
                if (thing != null) {
                    var info = new StreamInfo<Stream> ();
                    controller.Save (info);
                    new ThingStreamFacade ().SetStream (graph, thing, info);
                    info.Data.Dispose ();
                    info.Data = null;
                    info = null;
                }
            }
        }

        public void SaveStream(IThingGraph graph) {
            if (graph == null)
                return;

            foreach (var controller in StreamViewers) {
                SaveStream (graph, controller);
            }
        }

        public void ChangeViewer(object sender, SceneEventArgs e) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>()
                .Source<IThing, ILink>(e.Scene.Graph);


            if (e.Widget != null && graph != null) {

                WidgetThingGraph widgetThingGraph = (WidgetThingGraph)graph;
                IStreamThing thing = widgetThingGraph.Get(e.Widget) as IStreamThing;
                LoadStream(widgetThingGraph.Two as IThingGraph,thing);

            }
        }

        public void Clear() {
            foreach (var controller in StreamViewers) {
                controller.Clear();
            }
        }

        public void Dispose() {
            foreach (var controller in StreamViewers) {
                controller.Dispose ();
            }
        }
    }
}