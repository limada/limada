using System;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.Styles;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.XwtBackend;
using Xwt.Backends;

namespace Limaki.View.Viz.Visualizers.ToolStrips {

    [BackendType (typeof (ILayoutToolStripBackend))]
    public class LayoutToolStrip : DisplayToolStrip<IGraphSceneDisplay<IVisual, IVisualEdge>> {

        public LayoutToolStrip () {
            Compose ();
        }

        protected virtual void Compose () {

            StyleSheetCombo = new ComboBox {
                Width = 100,
                ToolTipText = "Stylesheets",
            };

            var styleSheetComboHost = new ToolStripItemHost { Child = StyleSheetCombo };
            
            StyleSheetCombo.SelectionChanged += (s, e) => {
                var selectedItem = StyleSheetCombo.SelectedItem;
                if (selectedItem != null)
                    StyleSheetChange (selectedItem.ToString ());
            };

            Registry.Pooled<StyleSheets> ().Keys.ForEach (s => StyleSheetCombo.Items.Add (s));

            var size = new Xwt.Size (60, 15);

            ShapeCombo = new ComboBox {
                Width = 100,
                ToolTipText = "Shapes",
            };

            var shapeComboHost = new ToolStripItemHost { Child = ShapeCombo };

            var shapes = ShapeFactory.Shapes ().ToArray ();

            shapes.ForEach (shape => {
                var img =
                    XwtDrawingExtensions.AsImage (
                            XwtDrawingExtensions.Render (shape, size, UiState.None, CurrentStyleSheet),
                            size);
                ShapeCombo.Items.Add (img);
            });

            ShapeCombo.SelectionChanged += (s, e) => {
                if (ShapeCombo.SelectedIndex == -1)
                    return;
                var shape = shapes[ShapeCombo.SelectedIndex];
                var currentDisplay = this.CurrentDisplay;
                if (currentDisplay != null) {
                    foreach (var visual in currentDisplay.Data.Selected.Elements) {
                        SceneExtensions.ChangeShape (currentDisplay.Data, visual, shape);
                    }
                    currentDisplay.Perform ();
                }
            };

            this.AddItems (styleSheetComboHost);
			this.AddItems (shapeComboHost);

        }

        public IStyleSheet CurrentStyleSheet { get; set; }
        public ComboBox StyleSheetCombo { get; private set; }
        public ComboBox ShapeCombo { get; private set; }

        public void StyleSheetChange (string sheetName) {

            IStyleSheet styleSheet = null;
            var styleSheets = Registry.Pooled<StyleSheets> ();
            if (!styleSheets.TryGetValue (sheetName, out styleSheet)) {
                var style = StyleSheet.CreateStyleWithSystemSettings ();
                styleSheet = new StyleSheet (sheetName, style);
                styleSheets.Add (styleSheet.Name, styleSheet);
            }
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                currentDisplay.StyleSheet = styleSheet;
                currentDisplay.BackendRenderer.Render ();
                CurrentStyleSheet = styleSheet;
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
                        result = (IStyleGroup) result.Clone ();
                        result.Name = visual.ToString ();
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
                    SceneExtensions.ChangeStyle (currentDisplay.Data, visual, style);
                }
                currentDisplay.Perform ();
                currentDisplay.Backend.QueueDraw ();

            }
        }

        public void ShapeChange (IShape shape) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                foreach (var visual in currentDisplay.Data.Selected.Elements) {
                    SceneExtensions.ChangeShape (currentDisplay.Data, visual, shape);
                }
                currentDisplay.Perform ();
            }
        }

        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                this.CurrentDisplay = display;
                this.CurrentStyleSheet = display.StyleSheet;
                StyleSheetCombo.SelectedItem = display.StyleSheet.Name;
            }
        }

        public override void Detach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                StyleSheetCombo.SelectedItem = null;
            }
            this.CurrentDisplay = null;
            this.CurrentStyleSheet = null;
        }
    }

    public interface ILayoutToolStripBackend : IDisplayToolStripBackend {

    }
}