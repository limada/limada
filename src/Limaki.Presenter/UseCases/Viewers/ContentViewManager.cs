/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 */


using System;
using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.UseCases.Viewers.StreamViewers;
using Limaki.Presenter.Visuals;
using Limaki.UseCases.Viewers.StreamViewers;
using Limaki.Visuals;
using Limaki.Presenter.UI;
using System.Linq;
using Limaki.Graphs;

namespace Limaki.UseCases.Viewers {
    public class ContentViewManager:IDisposable {
        
        public VisualsDisplay SheetControl {get;set;}
        public ISheetManager SheetManager { get; set; }

        public Color BackColor = KnownColors.FromKnownColor(KnownColor.Control);
        public object Parent = null;

        public event Action<object, Action> AfterStreamLoaded = null;
        public event Action<object> Attach = null;
        public event Action<object> DeAttach = null;

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler> (); }
        }

       

        IThing currentThing = null;
        bool IsStreamOwner = true;
        private ContentViewProviders _providers = null;
        public ContentViewProviders Providers { get { return _providers ?? (_providers = Registry.Pool.TryGetCreate<ContentViewProviders>()); } }
        public ThingContentViewProviders ThingContentViewProviders { get { return Providers as ThingContentViewProviders; } }

        void AttachController(ViewerController controller, IGraph<IVisual, IVisualEdge> graph, IVisual thing) {
            controller.BackColor = this.BackColor;
            controller.Parent = this.Parent;
            if (this.Attach != null) {
                controller.Attach -= this.Attach;
                controller.Attach += this.Attach;
            }
            DoAfterStreamLoaded(controller.Control, () => { controller.OnShow(); });
           
        }

        void LoadStreamThing(StreamViewerController controller, IThingGraph graph, IStreamThing thing) {
            try {
                
                if (controller.CurrentThingId != thing.Id) {
                    SaveStream(graph, controller);
                }

                controller.IsStreamOwner = IsStreamOwner;

                var info = ThingStreamFacade.GetStreamInfo(graph, thing);

                if (controller is SheetViewerController) {
                    var sheetView = (SheetViewerController)controller;
                    sheetView.SheetControl = this.SheetControl;
                    sheetView.SheetManager = this.SheetManager;
                    info.Source = thing.Id;
                }

                if (controller is HTMLViewerController) {
                    var htmlViewr = (HTMLViewerController)controller;
                    htmlViewr.ContentThing = thing;
                    htmlViewr.ThingGraph = graph;
                }

                if (controller.CurrentThingId != thing.Id) {
                    controller.SetContent(info);
                }

                currentThing = thing;

            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                thing.ClearRealSubject(!IsStreamOwner);
            }
        }

        void LoadThing(ThingViewerController controller, IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            try {
                //if (controller.CurrentThingId != thing.Id) {
                //    //SaveStream(graph, controller);
                //}
                var thing = graph.ThingOf(visual);
                if (controller.CurrentThingId != thing.Id) {
                    controller.CurrentThingId = thing.Id;
                    controller.SetContent(graph, visual);
                }

                currentThing = thing;

            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                
            }
        }


        void LoadThing(IGraph<IVisual,IVisualEdge> visualGraph, IVisual visual) {
            var graph = GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(visualGraph);

            if (visual != null && graph != null) {
                 var thing = graph.ThingOf(visual);
                 if (thing != null) {
                     var controller = ThingContentViewProviders.Supports(visualGraph, visual);
                     if (controller != null) {
                         AttachController(controller, graph, visual);
                         LoadThing(controller, visualGraph, visual);
                     }
                     var streamThing = thing as IStreamThing;
                     if (streamThing != null) {
                         var streamController = Providers.Supports(streamThing.StreamType);
                         
                         if (streamController != null) {
                             AttachController(streamController, graph, visual);
                             LoadStreamThing(streamController, graph.Two as IThingGraph, streamThing);
                         }
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

            foreach (var controller in Providers.Viewers.OfType<StreamViewerController>()) {
                SaveStream (graph, controller);
            }
        }

        public void ChangeViewer(object sender, GraphSceneEventArgs<IVisual, IVisualEdge> e) {
            if (e.Item != null) {
                LoadThing(e.Scene.Graph,e.Item);
            }
        }

        public void Clear() {
            foreach (var controller in Providers.Viewers) {
                controller.Clear();
            }
            currentThing = null;
        }

        public void Dispose() {
            foreach (var controller in Providers.Viewers) {
                controller.Dispose ();
            }
            

        }
    }
}