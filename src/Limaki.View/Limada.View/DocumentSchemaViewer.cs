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

namespace Limada.View {

    public abstract class DocumentSchemaViewer : ContentVisualViewer {

        public IGraphSceneDisplay<IVisual, IVisualEdge> GraphSceneDisplay { get; set; }

        /// <summary>
        /// TODO: refactor to IDisplay#Xwt.Image#
        /// </summary>
        public IDisplay ImageDisplay { get; set; }
        /// <summary>
        /// sets ImageDisplay.Data
        /// </summary>
        protected abstract Stream ImageStream { set; }

        public virtual void Compose () {
            // link GraphSceneDisplay with ImageDisplay:
            GraphSceneDisplay.SceneFocusChanged += (s, e) => {
                var docMan = new DocumentSchemaManager();
                var pageStream = docMan.PageStream(e.Scene.Graph, e.Item);
                if (pageStream != null) {
                    ImageStream = pageStream.Data;
                } else {
                    ImageStream = null;
                }
            };

            Adjust(GraphSceneDisplay);

            ImageDisplay.ZoomState = ZoomState.FitToWidth;
            ImageDisplay.EventControler.Remove(ImageDisplay.EventControler.GetAction<KeyScrollAction>());

            var scroller = new DocumentSchemaKeyScrollAction();
            scroller.Viewport = () => ImageDisplay.Viewport;
            ImageDisplay.EventControler.Add(scroller);
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
        public void Adjust (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var layout = display.Layout;
            Border = new Size(0, -5);
            layout.StyleSheet = DefaultStyleSheet;

            var focusAction = GraphSceneDisplay.EventControler.GetAction<GraphSceneFocusAction<IVisual, IVisualEdge>>();
            if (focusAction != null)
                focusAction.HitSize = -1;

            var folding = GraphSceneDisplay.EventControler.GetAction<IGraphSceneFolding<IVisual, IVisualEdge>>();
            folding.Folder.RemoveOrhpans = false;
        }

        private int padding = 4;

        public IVisual DocumentVisual { get; protected set; }

        public override void SetContent (IGraph<IVisual, IVisualEdge> sourceGraph, IVisual sourceDocument) {
            var display = this.GraphSceneDisplay;
            // bring the docpages into view:
            var docManager = new DocumentSchemaManager();
            var scene = new Scene();
            var targetGraph = new WiredDisplays().CreateTargetGraph(sourceGraph);
            scene.Graph = targetGraph;
            this.GraphSceneDisplay.Data = scene;

            var doc = sourceGraph.ThingOf(sourceDocument);
            var targetDocument = targetGraph.VisualOf(doc);

            this.DocumentVisual = targetDocument;

            // get the pages and add them to scene:
            var pages = docManager.Pages(targetGraph, targetDocument).OrderBy(e => e, new VisualComparer()).ToList();
            pages.ForEach(page => display.Data.Add(page));

            var distance = display.Layout.Distance;
            display.Layout.Border = this.Border;

            var aligner = new Aligner<IVisual, IVisualEdge>(display.Data, display.Layout);
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

            display.DataId = 0;
            new State { Hollow = true }.CopyTo(display.State);
            display.Text = sourceDocument.Data == null ? CommonSchema.NullString : sourceDocument.Data.ToString();
            display.Viewport.Reset();
            display.BackendRenderer.Render();

            // show first page:
            var firstPage = pages.FirstOrDefault();
            if (firstPage != null) {
                scene.Focused = firstPage;
                display.OnSceneFocusChanged();
            }
            display.Execute();

            var scroller = ImageDisplay.EventControler.GetAction<DocumentSchemaKeyScrollAction>();
            if (scroller != null) {
                scroller.KeyProcessed = (r) => {
                    var inc = (int) (r.X + r.Y + r.Bottom + r.Right);
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
                            GraphSceneDisplay.Execute();
                            GraphSceneDisplay.OnSceneFocusChanged();
                        }

                    }
                };
            }

            var pageCache = new Set<IVisual>(pages);
            var moveResize = display.EventControler.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>>();
            moveResize.FocusFilter = e => pageCache.Contains(e) ? null : e;
        }
        
        public override void Dispose () {
            Clear();
        }

        public override bool Supports (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var docManager = new DocumentSchemaManager();
            return docManager.HasPages(graph, visual);
        }
    }
}