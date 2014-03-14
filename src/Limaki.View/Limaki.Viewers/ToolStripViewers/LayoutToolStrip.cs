using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt.Backends;

namespace Limaki.Viewers.ToolStripViewers {

    [BackendType(typeof(ILayoutToolStripBackend))]
    public class LayoutToolStrip : ToolStripViewer<IGraphSceneDisplay<IVisual, IVisualEdge>, ILayoutToolStripBackend> {
    

        public void StyleSheetChange (string sheetName) {
            IStyleSheet styleSheet = null;
            var styleSheets = Registry.Pooled<StyleSheets>();
            if (!styleSheets.TryGetValue(sheetName, out styleSheet)) {
                var style = StyleSheet.CreateStyleWithSystemSettings();
                styleSheet = new StyleSheet(sheetName, style);
                styleSheets.Add(styleSheet.Name, styleSheet);
            }
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                currentDisplay.StyleSheet = styleSheet;
                currentDisplay.BackendRenderer.Render();
            }
        }

        public void ShapeChange (IShape shape) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                foreach (IVisual visual in currentDisplay.Data.Selected.Elements) {
                    SceneExtensions.ChangeShape(currentDisplay.Data, visual, shape);
                }
                currentDisplay.Perform();
            }
        }

        public IStyleGroup StyleToChange () {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                var visual = currentDisplay.Data.Focused;
                if (visual != null) {
                    if (visual.Style != null)
                        return visual.Style;
                    else {
                        var result = visual is IVisualEdge ? currentDisplay.StyleSheet.EdgeStyle : currentDisplay.StyleSheet.ItemStyle;
                        result = (IStyleGroup)result.Clone();
                        result.Name = visual.ToString();
                        return result;

                    }
                }

            }
            return null;
        }

        public void StyleChange (IStyleGroup style) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                foreach (IVisual visual in currentDisplay.Data.Selected.Elements) {
                    SceneExtensions.ChangeStyle(currentDisplay.Data, visual, style);
                }
                currentDisplay.Perform();
                currentDisplay.Backend.Invalidate();

            }
        }

        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                this.CurrentDisplay = display;
                Backend.AttachStyleSheet(display.StyleSheet.Name);
            }
        }

        public override void Detach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                Backend.DetachStyleSheet(display.StyleSheet.Name);
            }
            this.CurrentDisplay = null;
        }
    }


    public interface ILayoutToolStripBackend : IToolStripViewerBackend {
        void AttachStyleSheet (string sheetName);
        void DetachStyleSheet (string sheetName);
    }
}