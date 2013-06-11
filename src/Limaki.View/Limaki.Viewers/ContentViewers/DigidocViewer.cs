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

using Limada.Schemata;
using Limada.Usecases;
using Limada.VisualThings;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limaki.Model.Content;
using Limaki.View;
using Limaki.View.Layout;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.Visuals;
using System;
using System.IO;
using System.Linq;
using Xwt;
using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.Viewers.StreamViewers {

    public interface IDocumentSchemaViewerBackend : IVidgetBackend { }

    [BackendType(typeof(IDocumentSchemaViewerBackend))]
    public class DigidocViewer : Vidget, IZoomTarget {

        IDocumentSchemaViewerBackend _backend = null;
        public virtual IDocumentSchemaViewerBackend Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as IDocumentSchemaViewerBackend;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        IGraphSceneDisplay<IVisual, IVisualEdge> _pagesDisplay = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> PagesDisplay {
            get {
                if (_pagesDisplay == null) {
                    _pagesDisplay = new VisualsDisplay();
                }
                return _pagesDisplay;
            }
        }

        public virtual ContentStreamViewer ContentViewer { get; set; }

        public Action<ContentStreamViewer> AttachContentViewer { get; set; }

        protected virtual Content<Stream> PageContent {
            set {
                if (value != null) {
                    var viewerProvider = Registry.Pool.TryGetCreate<ContentViewerProvider>();
                    var viewer = viewerProvider.Supports(value.ContentType);
                    if (viewer != null) {
                        viewer.SetContent(value);
                        if (ContentViewer != viewer) {
                            ContentViewer = viewer;
                            OnAttachContentViewer(viewer);
                        }
                    }

                } else {
                    ContentViewer = null;
                }
            }
        }

        public void OnAttachContentViewer (ContentStreamViewer viewer) {
            if (viewer == null)
                return;

            viewer.BackColor = Colors.White;
            if (ContentViewer != viewer) {
                ContentViewer = viewer;
                if (AttachContentViewer != null)
                    AttachContentViewer(viewer);

                var display = viewer.Frontend as IDisplay;
                if (display == null)
                    return;

                display.ZoomState = ZoomState.FitToWidth;
                display.EventControler.Remove(display.EventControler.GetAction<KeyScrollAction>());

                var scroller = display.EventControler.GetAction<DigidocKeyScrollAction>();
                if (scroller == null) {
                    scroller = new DigidocKeyScrollAction();
                    scroller.Viewport = () => display.Viewport;
                    display.EventControler.Add(scroller);
                }
            }
        }

        public virtual void Compose () {
            ComposePagesDisplay(PagesDisplay);
        }

        protected int padding = 4;
        public virtual int GetDefaultWidth () {
            var utils = Registry.Pool.TryGetCreate<IDrawingUtils>();
            var size = utils.GetTextDimension("".PadLeft(padding, '9'), DefaultStyleSheet.BaseStyle);
            return (int)(size.Width + 32);
        }

        IStyleSheet _defaultStyleSheet = null;
        public IStyleSheet DefaultStyleSheet {
            get {
                if (_defaultStyleSheet == null) {
                    var styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
                    _defaultStyleSheet = styleSheets["WhiteGlass"];
                }
                return _defaultStyleSheet;
            }
        }

        protected virtual Size Border { get; set; }

        protected virtual void ComposePagesDisplay (IGraphSceneDisplay<IVisual, IVisualEdge> display) {

            display.SceneFocusChanged += (s, e) => {
                var docMan = new DigidocManager();
                var pageContent = docMan.PageContent(e.Scene.Graph, e.Item);
                if (pageContent != null) {
                    PageContent = pageContent;
                } else {
                    PageContent = null;
                }
                AttachScroller(display, ContentViewer.Frontend as IDisplay);
            };

            var layout = display.Layout;
            Border = new Size(0, -5);
            layout.StyleSheet = DefaultStyleSheet;

            var focusAction = display.EventControler.GetAction<GraphSceneFocusAction<IVisual, IVisualEdge>>();
            if (focusAction != null)
                focusAction.HitSize = -1;

            var folding = display.EventControler.GetAction<IGraphSceneFolding<IVisual, IVisualEdge>>();
            folding.Folder.RemoveOrhpans = false;
        }



        protected virtual void AttachScroller (IGraphSceneDisplay<IVisual, IVisualEdge> pagesDisplay, IDisplay contentDisplay) {
            if (contentDisplay == null)
                return;

            var scroller = contentDisplay.EventControler.GetAction<DigidocKeyScrollAction>();
            var scene = pagesDisplay.Data;
            var pages = scene.Elements.Where(e => !(e is IVisualEdge)).OrderBy(e => e.Location.Y).ToList();
            if (scroller != null) {
                scroller.KeyProcessed = (r) => {
                    var inc = (int)(r.X + r.Y + r.Bottom + r.Right);
                    if (scene.Focused != null && inc != 0) {
                        var iPage = pages.IndexOf(scene.Focused);
                        if (iPage != -1 && pages.Count > iPage + inc && iPage + inc >= 0) {
                            scene.Requests.Add(
                                new StateChangeCommand<IVisual>(scene.Focused, new Pair<UiState>(UiState.Focus, UiState.None))
                                );
                            scene.Selected.Clear();
                            scene.Focused = pages[iPage + inc];
                            scene.Requests.Add(
                                new StateChangeCommand<IVisual>(scene.Focused, new Pair<UiState>(UiState.None, UiState.Focus))
                                );
                            pagesDisplay.Execute();
                            pagesDisplay.OnSceneFocusChanged();
                        }

                    }
                };
            }
        }

        public virtual IVisual DocumentVisual { get; set; }
        public virtual void SetDocument (GraphCursor<IVisual, IVisualEdge> source) {

            var pagesDisplay = this.PagesDisplay;

            // bring the docpages into view:
            var docManager = new DigidocManager();
            var scene = new Scene();
            var targetGraph = new WiredDisplays().CreateTargetGraph(source.Graph);
            scene.Graph = targetGraph;
            pagesDisplay.Data = scene;

            var doc = source.Graph.ThingOf(source.Cursor);
            var targetDocument = targetGraph.VisualOf(doc);

            this.DocumentVisual = targetDocument;

            // get the pages and add them to scene:
            var pages = docManager.Pages(targetGraph, targetDocument).OrderBy(e => e, new VisualComparer()).ToList();
            pages.ForEach(page => pagesDisplay.Data.Add(page));

            var distance = pagesDisplay.Layout.Distance;
            pagesDisplay.Layout.Border = this.Border;

            var aligner = new Aligner<IVisual, IVisualEdge>(pagesDisplay.Data, pagesDisplay.Layout);
            var dd = this.Border.Height;
            var options = new AlignerOptions {
                Distance = new Size(dd, dd),
                AlignX = Alignment.End,
                AlignY = Alignment.Start,
                Dimension = Dimension.X,
                PointOrderDelta = 1
            };

            aligner.OneColumn(pages, (Point)this.Border, options);
            aligner.Locator.Commit(aligner.GraphScene.Requests);

            pagesDisplay.DataId = 0;
            new State { Hollow = true }.CopyTo(pagesDisplay.State);
            pagesDisplay.Text = source.Cursor.Data == null ? CommonSchema.NullString : source.Cursor.Data.ToString();
            pagesDisplay.Viewport.Reset();
            pagesDisplay.BackendRenderer.Render();

            // show first page:
            var firstPage = pages.FirstOrDefault();
            if (firstPage != null) {
                scene.Focused = firstPage;
                pagesDisplay.OnSceneFocusChanged();
            }
            pagesDisplay.Execute();

            var pageCache = new Set<IVisual>(pages);
            var moveResize = pagesDisplay.EventControler.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>>();
            moveResize.FocusFilter = e => pageCache.Contains(e) ? null : e;

        }

        public override void Dispose () {
            Clear();
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
                zoom.UpdateZoom();
        }

        #endregion
    }
}