using System.Collections.Generic;
using Limaki.Common;

namespace Limaki.Drawing {
    public class StyleSheets : Dictionary<string, IStyleSheet> {

        static ISystemFonts _systemfonts = null;
        protected static ISystemFonts SystemFonts {
            get {
                if (_systemfonts == null) {
                    _systemfonts = Registry.Factory.Create<ISystemFonts>();
                }
                return _systemfonts;
            }
        }

        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

        protected virtual Font CreateFont(string familiy, double size) {
            return drawingUtils.CreateFont(familiy, size);
        }

        public IList<string> StyleSheetNames = new string[] { "Desktop", "TealSmoke", "WhiteGlass" };

        public virtual IStyleSheet DefaultStyleSheet {
            get { return this[StyleSheetNames[1]]; }
        }

        public virtual IStyleSheet PredefinedStyleSheets(string name) {
            IStyleSheet _styleSheet = null;
            if (name == "Desktop") {
                var style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = name+"."+StyleNames.BaseStyle;
                //style.Pen.Color = style.PenColor;
                style.Font = (Font)SystemFonts.MessageBoxFont.Clone();

                _styleSheet = new StyleSheet(name, style);
                _styleSheet.ItemStyle.SelectedStyle.FillColor = KnownColors.FromKnownColor(KnownColor.ActiveCaption);
                
                _styleSheet.ItemStyle.SelectedStyle.TextColor = KnownColors.FromKnownColor(KnownColor.ActiveCaptionText);
                _styleSheet.ItemStyle.HoveredStyle.FillColor = KnownColors.FromKnownColor(KnownColor.GradientActiveCaption);

            }
            if (name == "TealSmoke") {
                var style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = name + "." + StyleNames.BaseStyle;
                style.FillColor = Color.FromArgb(200, Colors.WhiteSmoke);
                style.PenColor = Color.FromArgb(200, Colors.Teal);
                //style.Pen.Color = style.PenColor;
                style.Font = CreateFont(style.Font.Family, 10);
                _styleSheet = new StyleSheet(name, style);
                _styleSheet.ItemStyle.SelectedStyle.FillColor = Colors.Teal;
                _styleSheet.ItemStyle.SelectedStyle.TextColor = Colors.WhiteSmoke;
                _styleSheet.ItemStyle.HoveredStyle.FillColor = Colors.MintCream;

            }

            if (name == "WhiteGlass") {
                var style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = name + "." + StyleNames.BaseStyle;
                style.FillColor = Color.FromArgb(200, Colors.White);
                style.PenColor = Color.FromArgb(200, Colors.White);
                //style.Pen.Color = style.PenColor;
                style.Font = (Font)SystemFonts.MessageBoxFont.Clone();

                _styleSheet = new StyleSheet(name, style);

                _styleSheet.ItemStyle.SelectedStyle.PenColor=Color.FromArgb(50, 150, 150, 150);
                //_styleSheet.ItemStyle.SelectedStyle.Pen.Thickness = style.Pen.Thickness;

                var font = (Font)_styleSheet.BaseStyle.Font.Clone();
                font.Style = FontStyle.Underline;
                _styleSheet.ItemStyle.SelectedStyle.Font = font;

                _styleSheet.ItemStyle.HoveredStyle.PenColor = Color.FromArgb(50, 150, 150, 150);
                _styleSheet.ItemStyle.HoveredStyle.FillColor = style.FillColor;

                _styleSheet.EdgeStyle.TextColor = Color.FromArgb(150, 100, 100, 100);
                _styleSheet.EdgeStyle.FillColor = KnownColors.FromKnownColor(KnownColor.White);
                _styleSheet.EdgeStyle.Font = CreateFont(style.Font.Family, style.Font.Size - 2.0d);

                var pen = (Pen)_styleSheet.EdgeStyle.Pen.Clone();
                pen.Thickness = 0.5d;
                _styleSheet.EdgeStyle.Pen = pen;
                _styleSheet.EdgeStyle.PenColor = Color.FromArgb(150, 180, 180, 180);

                // this makes sense, cause CreateStyleGroup could make other colors:
                _styleSheet.EdgeStyle.HoveredStyle.FillColor = _styleSheet.EdgeStyle.FillColor;
                _styleSheet.EdgeStyle.HoveredStyle.PenColor = _styleSheet.EdgeStyle.PenColor;
                _styleSheet.EdgeStyle.HoveredStyle.TextColor = _styleSheet.EdgeStyle.TextColor;

                _styleSheet.EdgeStyle.SelectedStyle.TextColor = Color.FromArgb(200, 50, 50, 50);
                _styleSheet.EdgeStyle.SelectedStyle.PenColor = Color.FromArgb(50, 180, 180, 180);
                _styleSheet.EdgeStyle.SelectedStyle.FillColor = _styleSheet.EdgeStyle.FillColor;

            }
            _styleSheet.BackColor = KnownColors.FromKnownColor(KnownColor.Window);
            return _styleSheet;
        }

        public virtual void Init() {
            foreach (var name in StyleSheetNames) {
                var sheet = PredefinedStyleSheets(name);
                if (sheet != null) {
                    if (!this.ContainsKey(sheet.Name))
                        this.Add(sheet.Name, sheet);
                }
            }
        }
    }
}