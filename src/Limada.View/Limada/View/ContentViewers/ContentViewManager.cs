/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2017 Lytico
 *
 * http://www.limada.org
 */


using System;
using System.IO;
using System.Linq;
using Limada.Model;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mapping;
using Limaki.View.Viz.UI.GraphScene;
using Xwt;
using Xwt.Drawing;

namespace Limada.View.ContentViewers {

    public class ContentViewManager:IContentViewManager {

        public ContentViewManager() {
            IsProviderOwner = true;
            IsStreamOwner = true;
        }

        public IGraphSceneDisplay<IVisual, IVisualEdge> SheetViewer { get; set; }
        public IVisualSceneStoreInteractor StoreInteractor { get; set; }

        public Color BackColor { get; set; } = SystemColors.Background;

        [Obsolete]
        /// <summary>
        /// called in OnAttachViewer
        /// </summary>
        public Action<IVidget, Action> AttachViewer { get; set; }

        [Obsolete]
        /// <summary>
        /// delegated to Viewer.AttachBackend
        /// </summary>
        public Action<IVidgetBackend> AttachViewerBackend { get; set; }

        [Obsolete]
        /// <summary>
        /// delegated to Viewer.DetachBackend
        /// </summary>
        public Action<IVidgetBackend> ViewersDetachBackend { get; set; }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pooled<IExceptionHandler> (); }
        }

        public bool IsStreamOwner {get;set;}
        public bool IsProviderOwner { get; set; }

        private ContentViewerProvider _providers = null;
        public ContentViewerProvider ContentViewerProvider { get { return _providers ?? (_providers = Registry.Pooled<ContentViewerProvider>()); } }
        public ContentVisualViewerProvider ContentVisualViewerProvider { get { return ContentViewerProvider as ContentVisualViewerProvider; } }

        protected void LoadStreamThing (ContentStreamViewer viewer, IThingGraph graph, IStreamThing thing) {
            try {
                viewer.IsStreamOwner = IsStreamOwner;
                if (viewer.ContentId != thing.Id) {
                    SaveStream(graph, viewer);

                    var content = graph.ContentOf(thing);
                    if (viewer is SheetViewer) {
                        content.Source = thing.Id;
                    }

                    if (viewer is HtmlContentViewer) {
                        var htmlViewr = (HtmlContentViewer)viewer;
                        htmlViewr.ContentThing = thing;
                        htmlViewr.ThingGraph = graph;
                    }


                    viewer.SetContent(content);
                    viewer.ContentId = thing.Id;
                }


            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                thing.ClearRealSubject(!IsStreamOwner);
            }
        }

        protected void LoadThing (ContentVisualViewer viewer, IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            try {
                //if (viewer.CurrentThingId != thing.Id) {
                //    //SaveContentOfViewers(graph, viewer);
                //}
                var thing = graph.ThingOf(visual);
                if (viewer.ContentId != thing.Id) {
                    viewer.ContentId = thing.Id;
                    viewer.SetContent(graph, visual);
                }


            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                
            }
        }

        public bool IsContent (IGraph<IVisual, IVisualEdge> visualGraph, IVisual visual) {
             var graph = visualGraph.Source<IVisual, IVisualEdge,IThing, ILink>();

             if (visual != null && graph != null) {
                 return graph.ThingOf (visual) is IStreamThing;
             }
             return false;
        }

        ContentViewer _currentViewer = null;
        public ContentViewer CurrentViewer {
            get { return _currentViewer; }
            set {
                if (value != _currentViewer && _currentViewer!=null) {
                    DetachCurrentViewer (_currentViewer);
                }
                _currentViewer = value;
                OnAttachCurrentViewer (_currentViewer);
            }
        }

        public Action<ContentViewer> AttachCurrentViewer { get; set; }
        public Action<ContentViewer> DetachCurrentViewer { get; set; }
        protected void OnAttachCurrentViewer (ContentViewer viewer) {
            if (viewer == null)
                return;
            if (viewer is SheetViewer) {
                var sheetViewer = viewer as SheetViewer;
                sheetViewer.SheetDisplay = this.SheetViewer;
                sheetViewer.SceneManager = this.StoreInteractor;
            }

            viewer.BackColor = this.BackColor;

            AttachCurrentViewer?.Invoke (viewer);

        }

        protected void LoadThing (IGraph<IVisual, IVisualEdge> visualGraph, IVisual visual) {
            var graph = visualGraph.Source<IVisual, IVisualEdge,IThing, ILink>();

            if (visual != null && graph != null) {
                 var thing = graph.ThingOf(visual);
                 if (thing != null) {
                     var viewer = ContentVisualViewerProvider.Supports(visualGraph, visual);
                     if (viewer != null) {
                         CurrentViewer = viewer;
                         LoadThing(viewer, visualGraph, visual);
                     }
                     var streamThing = thing as IStreamThing;
                     if (streamThing != null) {
                         
                         var streamViewer = ContentViewerProvider.Supports(streamThing.StreamType);
                         
                         if (streamViewer != null) {
                             CurrentViewer = streamViewer;
                             LoadStreamThing(streamViewer, graph.Source as IThingGraph, streamThing);
                         }
                     }
                 }
             }
        }


		[TODO("Refactor this to use State")]
		protected void SaveStream(IThingGraph thingGraph, ContentStreamViewer viewer) {
            if (thingGraph == null || !viewer.CanSave ())
                return;

			var thing = thingGraph.GetById(viewer.ContentId) as IStreamThing;
            if (thing != null) {
                var content = new Content<Stream> ();
                viewer.Save (content);
                new ThingContentFacade ().AssignContent (thingGraph, thing, content);
                if (content.Data != null) {
                    content.Data.Dispose ();
                }
                content.Data = null;
				content = null;
				thing.State.Clean = true;
            }
		}

		public void SaveStream(IGraph<IVisual, IVisualEdge> graph, ContentStreamViewer viewer) {

			if (viewer == null || graph == null || viewer.ContentId == 0 || !viewer.CanSave())
				return;

			SaveStream(graph.ThingGraph(), viewer);
		}

        private IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;
        protected IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer {
            get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); }
        }

        public void SaveContentOfViewers() {
            var graph = Organizer.Displays.Select (d => d.Data.Graph.ThingGraph ()).FirstOrDefault (t => t != null);
            if (graph == null)
                return;

            foreach (var viewer in ContentViewerProvider.Viewers.OfType<ContentStreamViewer>()) {
                SaveStream (graph, viewer);
            }
        }

        public void ShowViewer(object sender, GraphSceneEventArgs<IVisual, IVisualEdge> e) {
            if (e.Item != null) {
                LoadThing(e.Scene.Graph,e.Item);
            }
        }

        public void Clear() {
            if (IsProviderOwner) {
                foreach (var viewer in ContentViewerProvider.Viewers) {
                    viewer.Clear();
                }
            }
        }

        public void Dispose () {
            if (IsProviderOwner) {
                ContentViewerProvider.Dispose();
            }
        }

        
    }
}