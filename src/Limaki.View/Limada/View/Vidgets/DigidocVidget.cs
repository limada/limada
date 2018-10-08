/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using System.Linq;
using Limada.Schemata;
using Limada.View.ContentViewers;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limada.View.VisualThings;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mapping;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.UI.GraphScene;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using Xwt;
using Xwt.Backends;
using Xwt.Drawing;
using Limaki.View.GraphScene;
using Limaki.Contents.IO;
using Limaki.View.Common;

namespace Limada.View.Vidgets {

    public interface IDigidocViewerBackend : IVidgetBackend { }

    [BackendType (typeof(IDigidocViewerBackend))]
    public class DigidocVidget : Vidget, IZoomTarget {

        IDigidocViewerBackend _backend = null;
        public virtual new IDigidocViewerBackend Backend {
            get { return _backend ?? (_backend = base.Backend as IDigidocViewerBackend); }
        }

        IGraphSceneDisplay<IVisual, IVisualEdge> _pagesDisplay = null;

        public IGraphSceneDisplay<IVisual, IVisualEdge> PagesDisplay {
            get {
                if (_pagesDisplay == null) {
                    _pagesDisplay = new VisualsDisplay ();
                    Organizer.AddDisplay (_pagesDisplay);
                }
                return _pagesDisplay;
            }
        }

        public virtual ContentStreamViewer ContentViewer { get; set; }

        public Action<ContentStreamViewer> AttachContentViewer { get; set; }

        Content<Stream> _pageContent = null;

        protected virtual Content<Stream> PageContent {
            set {
                _pageContent = value;
                SetContentViewer (_pageContent);
            }
        }

        public void SetContentViewer (Content<Stream> pageContent) {

            if (pageContent == null) {
                ContentViewer = null;
                return;
            }

            var viewerProvider = Registry.Pooled<ContentViewerProvider> ();
            var viewer = viewerProvider.Supports (pageContent.ContentType);
            if (viewer == null) {
                var io = Registry.Pooled<StreamContentIoPool> ().Find (pageContent.Data, IoMode.Read);
                if (io != null) {
                    var info = io.Detector.Use (pageContent.Data);
                    pageContent.ContentType = info.ContentType;
                }
            }

            if (viewer != null) {
                viewer.SetContent (pageContent);
                viewer.BackColor = Colors.White;
            }

            if (ContentViewer != viewer) {
                ContentViewer = viewer;

                AttachContentViewer?.Invoke (viewer);

                var display = viewer?.Frontend as IDisplay;
                if (display == null)
                    return;

                display.ZoomState = ZoomState.FitToWidth;
                display.ActionDispatcher.Remove (display.ActionDispatcher.GetAction<KeyScrollAction> ());

                var scroller = display.ActionDispatcher.GetAction<DigidocKeyScrollAction> ();
                if (scroller == null) {
                    scroller = new DigidocKeyScrollAction ();
                    scroller.Viewport = () => display.Viewport;
                    display.ActionDispatcher.Add (scroller);
                }
            }
        }

        public virtual void OnShow () {
            if (ContentViewer != null)
                AttachContentViewer?.Invoke (ContentViewer);
        }

        public virtual void Compose () {
            ComposePagesDisplay (PagesDisplay);
        }

        protected int padding = 4;

        public virtual double GetDefaultWidth () {
            var utils = Registry.Pooled<IDrawingUtils> ();
            var size = utils.GetTextDimension ("".PadLeft (padding, '9'), DefaultStyleSheet.BaseStyle);
            return (size.Width + 32);
        }

        IStyleSheet _defaultStyleSheet = null;

        public IStyleSheet DefaultStyleSheet {
            get {
                if (_defaultStyleSheet == null) {
                    var styleSheets = Registry.Pooled<StyleSheets> ();
                    _defaultStyleSheet = styleSheets ["WhiteGlass"];
                }
                return _defaultStyleSheet;
            }
        }

        protected virtual Size Border { get; set; }

        protected virtual void ComposePagesDisplay (IGraphSceneDisplay<IVisual, IVisualEdge> display) {

            display.SceneFocusChanged += (s, e) => {
                var docMan = new DigidocViz ();
                var pageContent = docMan.PageContent (e.Scene.Graph, e.Item);
                PageContent = pageContent;
                if (pageContent != null && ContentViewer != null) {
                    AttachScroller (display, ContentViewer.Frontend as IDisplay);
                    var thing = e.Scene.Graph.ThingOf (e.Item);
                    ContentViewer.ContentId = thing.Id;
                }
            };

            var layout = display.Layout;
            Border = new Size (0, -5);
            layout.StyleSheet = DefaultStyleSheet;

            var focusAction = display.ActionDispatcher.GetAction<GraphSceneFocusAction<IVisual, IVisualEdge>> ();
            if (focusAction != null)
                focusAction.HitSize = -1;

            var folding = display.Folding ();
            folding.Folder.RemoveOrphans = false;
        }

        protected virtual void AttachScroller (IGraphSceneDisplay<IVisual, IVisualEdge> pagesDisplay, IDisplay contentDisplay) {
            if (contentDisplay == null)
                return;

            var scroller = contentDisplay.ActionDispatcher.GetAction<DigidocKeyScrollAction> ();
            var scene = pagesDisplay.Data;
            var pages = scene.Elements.Where (e => !(e is IVisualEdge)).OrderBy (e => e.Location.Y).ToList ();
            if (scroller != null) {
                scroller.KeyProcessed = (r) => {
                    var inc = (int)(r.X + r.Y + r.Bottom + r.Right);
                    if (scene.Focused != null && inc != 0) {
                        var iPage = pages.IndexOf (scene.Focused);
                        if (iPage != -1 && pages.Count > iPage + inc && iPage + inc >= 0) {
                            scene.SetFocused (pages [iPage + inc]);
                            scene.ClearSelection ();
                            pagesDisplay.Perform ();
                            pagesDisplay.OnSceneFocusChanged ();
                        }

                    }
                };
            }
        }

        public virtual IVisual DocumentVisual { get; set; }

        IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;

        IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer { get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); } }

        public virtual void SetDocument (GraphCursor<IVisual, IVisualEdge> source) {

            var pagesDisplay = this.PagesDisplay;

            // bring the docpages into view:
            var docManager = new DigidocViz ();
            var pageScene = new Scene ();
            pagesDisplay.Data = pageScene;

            var doc = source.Graph.ThingOf (source.Cursor);
            IGraph<IVisual, IVisualEdge> targetGraph = null;
            targetGraph = Organizer.CreateSinkGraph (source.Graph);

            pageScene.Graph = targetGraph;

            Organizer.AddScene (pageScene);

            var targetDocument = targetGraph.VisualOf (doc);
            this.DocumentVisual = targetDocument;

            // get the pages and add them to scene:
            var pages = docManager.Pages (targetGraph, targetDocument).OrderBy (e => e, new VisualComparer ()).ToList ();
            pages.ForEach (page => pagesDisplay.Data.Add (page));

            var distance = pagesDisplay.Layout.Distance;
            pagesDisplay.Layout.Border = this.Border;

            var aligner = new Aligner<IVisual, IVisualEdge> (pagesDisplay.Data, pagesDisplay.Layout);
            var dd = this.Border.Height;
            var options = new AlignerOptions {
                Distance = new Size (dd, dd),
                AlignX = Alignment.End,
                AlignY = Alignment.Start,
                Dimension = Dimension.X,
                PointOrderDelta = 1
            };

            aligner.OneColumn (pages, (Point)this.Border, options);
            aligner.Locator.Commit (aligner.GraphScene.Requests);

            new State { Hollow = true }.CopyTo (pagesDisplay.State);
            pagesDisplay.Text = source.Cursor.Data == null ? CommonSchema.NullString : source.Cursor.Data.ToString ();
            pagesDisplay.Viewport.Reset ();
            pagesDisplay.BackendRenderer.Render ();

            // show first page:
            var firstPage = pages.FirstOrDefault ();
            if (firstPage != null) {
                pageScene.Focused = firstPage;
                pagesDisplay.OnSceneFocusChanged ();
            }
            pagesDisplay.Perform ();

            var pageCache = new Set<IVisual> (pages);
            var moveResize = pagesDisplay.ActionDispatcher.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>> ();
            moveResize.FocusFilter = e => pageCache.Contains (e) ? null : e;

        }

        public override void Dispose () {
            Clear ();
        }

        public virtual void Clear () {
            if (PagesDisplay != null) {
                PagesDisplay.Data = null;
                PageContent = null;
            }
        }

        #region IZoomTarget Member

        private ZoomState _zoomState = ZoomState.FitToWidth;

        public ZoomState ZoomState {
            get { return _zoomState; }
            set {
                _zoomState = value;
                var zoom = ContentViewer.Frontend as IZoomTarget;
                if (zoom != null)
                    zoom.ZoomState = value;

            }
        }

        private double _zoomFactor = 0;

        public double ZoomFactor {
            get { return _zoomFactor; }
            set {
                _zoomFactor = value;
                var zoom = ContentViewer.Frontend as IZoomTarget;
                if (zoom != null)
                    zoom.ZoomFactor = value;
            }
        }

        public void UpdateZoom () {
            var zoom = ContentViewer.Frontend as IZoomTarget;
            if (zoom != null)
                zoom.UpdateZoom ();
        }

        #endregion

        
    }
}