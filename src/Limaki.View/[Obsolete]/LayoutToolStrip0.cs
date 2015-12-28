using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.View.Visuals;
using Xwt.Backends;

namespace Limaki.View.Viz.Visualizers.ToolStrips {

    [Obsolete]
    [BackendType(typeof(ILayoutToolStripBackend0))]
    public class LayoutToolStrip0 : DisplayToolStrip<IGraphSceneDisplay<IVisual, IVisualEdge>> {

        public new virtual ILayoutToolStripBackend0 Backend {
            get { return base.Backend as ILayoutToolStripBackend0; }
            set { base.Backend = value; }
        }

        public IStyleSheet CurrentStyleSheet { get; set; }

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
                CurrentStyleSheet = styleSheet;
            }
        }

        public void ShapeChange (IShape shape) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                foreach (var visual in currentDisplay.Data.Selected.Elements) {
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
                foreach (var visual in currentDisplay.Data.Selected.Elements) {
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
                this.CurrentStyleSheet = display.StyleSheet;
                Backend.AttachStyleSheet(display.StyleSheet.Name);
            }
        }

        public override void Detach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                Backend.DetachStyleSheet(display.StyleSheet.Name);
            }
            this.CurrentDisplay = null;
            this.CurrentStyleSheet = null;
        }
    }

    [Obsolete]
    public interface ILayoutToolStripBackend0 : IDisplayToolStripBackend {
        void AttachStyleSheet (string sheetName);
        void DetachStyleSheet (string sheetName);
    }
}