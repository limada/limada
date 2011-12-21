using System;
using System.Collections.Generic;
using System.Linq;
using Limada.Model;
using Limada.Schemata;
using Limada.UseCases;
using Limada.View;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.Presenter.Visuals;
using Limaki.Presenter.Visuals.UI;
using Limaki.UseCases.Viewers;
using Limaki.Visuals;
using Limaki.Presenter.Layout;

namespace Limaki.Presenter.Winform.Controls {
    public class DocumentSchemaController : ThingViewerController {

        public IGraphSceneDisplay<IVisual, IVisualEdge> GraphSceneDisplay { get; set; }
        public IDisplay<System.Drawing.Image> ImageDisplay { get; set; }

        public virtual void Compose() {
            // link GraphSceneDisplay with ImageDisplay:
            GraphSceneDisplay.SceneFocusChanged += (s, e) => {
                var docMan = new DocumentSchemaManager();
                var pageStream = docMan.PageStream(e.Scene.Graph, e.Item);
                if (pageStream != null) {
                    var stream = pageStream.Data;
                    if (stream != null)
                        ImageDisplay.Data = System.Drawing.Image.FromStream(stream);
                    else
                        ImageDisplay.Data = null;
                } else {
                    ImageDisplay.Data = null;
                }
            };

            Adjust(GraphSceneDisplay);

            ImageDisplay.ZoomState = Drawing.ZoomState.FitToWidth;
            ImageDisplay.EventControler.Remove(ImageDisplay.EventControler.GetAction<KeyScrollAction>());

            var scroller = new DocumentSchemaKeyScrollAction();
            scroller.Viewport = () => ImageDisplay.Viewport;
            ImageDisplay.EventControler.Add(scroller);
        }

        public int GetDefaultWidth() {
            var utils = Registry.Pool.TryGetCreate<Limaki.Drawing.IDrawingUtils>();
            var size = utils.GetTextDimension("".PadLeft(padding, '9'), DefaultStyleSheet.BaseStyle);
            return (int)(size.Width + 32);
        }

        Limaki.Drawing.IStyleSheet _defaultStyleSheet = null;
        public Limaki.Drawing.IStyleSheet DefaultStyleSheet {
            get {
                if (_defaultStyleSheet == null) {
                    var styleSheets = Registry.Pool.TryGetCreate<Limaki.Drawing.StyleSheets>();
                    _defaultStyleSheet = styleSheets["WhiteGlass"];
                }
                return _defaultStyleSheet;
            }
        }

        Limaki.Drawing.SizeI Border { get; set; }
        public void Adjust(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var layout = display.Layout;
            Border = new Limaki.Drawing.SizeI(0, -5);
            layout.StyleSheet = DefaultStyleSheet;

            var focusAction = GraphSceneDisplay.EventControler.GetAction<GraphSceneFocusAction<IVisual, IVisualEdge>>();
            if (focusAction != null)
                focusAction.HitSize = -1;

            var folding = GraphSceneDisplay.EventControler.GetAction<IGraphSceneFolding<IVisual, IVisualEdge>>();
            folding.Folder.RemoveOrhpans = false;
        }

        private int padding = 4;
        public IVisual Document { get; protected set; }
        public override void SetContent(IGraph<IVisual, IVisualEdge> sourceGraph, IVisual sourceDocument) {
            var display = this.GraphSceneDisplay;
            // bring the docpages into view:
            var docManager = new DocumentSchemaManager();
            var scene = new Scene();
            var targetGraph = new WiredDisplays().CreateTargetGraph(sourceGraph);
            scene.Graph = targetGraph;
            this.GraphSceneDisplay.Data = scene;

            var doc = sourceGraph.ThingOf(sourceDocument);
            var targetDocument = targetGraph.VisualOf(doc);

            this.Document = targetDocument;

            // get the pages and add them to scene:
            var pages = docManager.Pages(targetGraph, targetDocument).OrderBy(e => e, new VisualComparer()).ToList();
            pages.ForEach(page => display.Data.Add(page));

            var distance = display.Layout.Distance;
            display.Layout.Border = this.Border;

            var alligner = new Alligner<IVisual, IVisualEdge>(display.Data, display.Layout);
            alligner.OneColumn(pages, new PointI(this.Border),this.Border.Height);
            alligner.Proxy.Commit(alligner.Data);

            display.DataId = 0;
            new State { Hollow = true }.CopyTo(display.State);
            display.Text = sourceDocument.Data.ToString();
            display.Viewport.Reset();
            display.DeviceRenderer.Render();

            // show first page:
            var firstPage = pages.FirstOrDefault();
            if (firstPage != null) {
                scene.Focused = firstPage;
                display.OnSceneFocusChanged();
            }
            display.Execute();
            // wrong: display.Layout.Distance = distance;
            // TODO: Layout.Margin (Top,Left) and Layout.Padding (distance between visuals)

            var scroller = ImageDisplay.EventControler.GetAction<DocumentSchemaKeyScrollAction>();
            if (scroller != null) {
                scroller.KeyProcessed = (r) => {
                    var inc = r.X + r.Y + r.Bottom + r.Right;
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

        DocumentSchemaControl _control = null;
        public override object Control {
            get {
                if (_control == null) {
                    _control = new DocumentSchemaControl(this);
                    OnAttach(_control);
                }
                return _control;
            }
        }

        public override void Clear() {
            var control = _control as DocumentSchemaControl;
            if (control != null) {
                control.GraphSceneDisplay.Data = null;
                control.ImageDisplay.Data = null;
            }
            base.Clear();
        }

        public override void Dispose() {
            Clear();
        }

        public override bool Supports(IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var docManager = new DocumentSchemaManager();
            return docManager.HasPages(graph, visual);
        }


    }
}