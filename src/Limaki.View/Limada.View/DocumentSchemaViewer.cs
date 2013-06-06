/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.IO;
using System.Linq;
using Limada.Schemata;
using Limada.Usecases;
using Limada.VisualThings;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.Layout;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.Viewers;
using Limaki.Visuals;
using Xwt;
using Limaki.Model.Content;
using System.Collections.Generic;
using System;

namespace Limada.View {

    public interface IDocumentSchemaBackend : IVidgetBackend { }

    public abstract class DocumentSchemaViewer : ContentVisualViewer {

        public IGraphSceneDisplay<IVisual, IVisualEdge> PagesDisplay { get; set; }

        public IDisplay ContentDisplay { get; set; }
        public Action<ContentStreamViewer> AttachContentViewerBackend { get; set; }
        ContentStreamViewer ContentViewer { get; set; }
        
        protected virtual Content<Stream> PageContent {
            set {
                if (value != null) {
                    var viewerProvider = Registry.Pool.TryGetCreate<ContentViewerProvider>();
                    var viewer = viewerProvider.Supports(value.ContentType);

                    if (viewer != null) {
                        viewer.BackColor = Xwt.Drawing.Colors.White;
                        viewer.SetContent(value);
                        if (ContentViewer != viewer) {
                            ContentViewer = viewer;
                            AttachContentViewerBackend(viewer);
                        }
                        var displayBackend = viewer.Backend as IDisplayBackend;
                        if (displayBackend != null) {
                            var display = displayBackend.Frontend;
                            if (ContentDisplay != display) {
                                ContentDisplay = display;
                                AttachContentDisplay(display);
                            }
                        }

                    }
                } else {
                    ContentDisplay = null;
                }
            }
        }

        public virtual void Compose () {
            ComposePagesDisplay(PagesDisplay);
           
        }

        public int GetDefaultWidth () {
            var utils = Registry.Pool.TryGetCreate<IDrawingUtils>();
            var size = utils.GetTextDimension("".PadLeft(padding, '9'), DefaultStyleSheet.BaseStyle);
            return (int) (size.Width + 32);
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

        Size Border { get; set; }
        
        public void ComposePagesDisplay (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            
            display.SceneFocusChanged += (s, e) => {
                var docMan = new DocumentSchemaManager();
                var pageContent = docMan.PageContent(e.Scene.Graph, e.Item);
                if (pageContent != null) {
                    PageContent = pageContent;
                } else {
                    PageContent = null;
                }
                AttachScroller(display, ContentDisplay);
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

        public void AttachContentDisplay (IDisplay contentDisplay) {
            if (contentDisplay == null)
                return;
            contentDisplay.ZoomState = ZoomState.FitToWidth;
            contentDisplay.EventControler.Remove(contentDisplay.EventControler.GetAction<KeyScrollAction>());

            var scroller = contentDisplay.EventControler.GetAction<DocumentSchemaKeyScrollAction>();
            if (scroller == null) {
                scroller = new DocumentSchemaKeyScrollAction();
                scroller.Viewport = () => contentDisplay.Viewport;
                contentDisplay.EventControler.Add(scroller);
            }
        }

        private int padding = 4;

        public IVisual DocumentVisual { get; protected set; }

        protected void AttachScroller (IGraphSceneDisplay<IVisual, IVisualEdge> pagesDisplay, IDisplay contentDisplay) {
            if (contentDisplay == null)
                return;

            var scroller = contentDisplay.EventControler.GetAction<DocumentSchemaKeyScrollAction>();
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

        public override void SetContent (IGraph<IVisual, IVisualEdge> sourceGraph, IVisual sourceDocument) {

            var pagesDisplay = this.PagesDisplay;
           
            // bring the docpages into view:
            var docManager = new DocumentSchemaManager();
            var scene = new Scene();
            var targetGraph = new WiredDisplays().CreateTargetGraph(sourceGraph);
            scene.Graph = targetGraph;
            pagesDisplay.Data = scene;

            var doc = sourceGraph.ThingOf(sourceDocument);
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

            aligner.OneColumn(pages, (Point) this.Border, options);
            aligner.Locator.Commit(aligner.GraphScene.Requests);

            pagesDisplay.DataId = 0;
            new State { Hollow = true }.CopyTo(pagesDisplay.State);
            pagesDisplay.Text = sourceDocument.Data == null ? CommonSchema.NullString : sourceDocument.Data.ToString();
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

        public override bool Supports (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var docManager = new DocumentSchemaManager();
            return docManager.HasPages(graph, visual);
        }

        public override void Clear () {
            if (PagesDisplay != null) {
                PagesDisplay.Data = null;
                PageContent = null;
            }
         
            base.Clear();
        }
        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return ContentDisplay.ZoomState; }
            set { ContentDisplay.ZoomState = value; }
        }

        public double ZoomFactor {
            get { return ContentDisplay.Viewport.ZoomFactor; }
            set { ContentDisplay.Viewport.ZoomFactor = value; }
        }

        public void UpdateZoom () {
            ContentDisplay.Viewport.UpdateZoom();
        }

        #endregion
    }
}