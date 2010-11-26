using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Common;

namespace Limaki.Drawing {
    public class StyleSheets:Dictionary<string,IStyleSheet> {

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
            return drawingUtils.CreateFont(familiy,size);
        }

        public string[] StyleSheetNames = { "Desktop", "TealSmoke", "WhiteGlass" };

        public virtual IStyleSheet DefaultStyleSheet {
            get { return this[StyleSheetNames[1]];} 
        }

        public virtual IStyleSheet PredefinedStyleSheets(string name) {
            IStyleSheet _styleSheet = null;
            if (name == "Desktop") {
                IStyle style = Limaki.Drawing.StyleSheet.CreateStyleWithSystemSettings();
                style.Name = StyleNames.DefaultStyle;
                style.Pen.Color = style.PenColor;
                
                style.Font = 
                    (Font)SystemFonts.MessageBoxFont.Clone();

                _styleSheet = new StyleSheet(name, style);
                _styleSheet[StyleNames.SelectedStyle].FillColor =
                    KnownColors.FromKnownColor (KnownColor.ActiveCaption);
                _styleSheet[StyleNames.SelectedStyle].TextColor = 
                    KnownColors.FromKnownColor (KnownColor.ActiveCaptionText);
                _styleSheet[StyleNames.HoveredStyle].FillColor = 
                    KnownColors.FromKnownColor (KnownColor.GradientActiveCaption);

            }
            if (name == "TealSmoke") {
                IStyle style = Limaki.Drawing.StyleSheet.CreateStyleWithSystemSettings();
                style.Name = StyleNames.DefaultStyle;
                style.FillColor = Color.FromArgb(200, Color.WhiteSmoke);
                style.PenColor = Color.FromArgb (200, Color.Teal);
                style.Pen.Color = style.PenColor;
                style.Font = CreateFont(style.Font.FontFamily, 10);
                _styleSheet = new StyleSheet(name, style);
                _styleSheet[StyleNames.SelectedStyle].FillColor = Color.Teal;
                _styleSheet[StyleNames.SelectedStyle].TextColor = Color.WhiteSmoke;
                _styleSheet[StyleNames.HoveredStyle].FillColor = Color.MintCream;
                    
            }

            if (name == "WhiteGlass") {
                IStyle style = Limaki.Drawing.StyleSheet.CreateStyleWithSystemSettings();
                style.Name = StyleNames.DefaultStyle;
                style.FillColor = Color.FromArgb (200, Color.White);
                style.PenColor = Color.FromArgb(200,Color.White);
                style.Pen.Color = style.PenColor;
                style.Font = (Font)SystemFonts.MessageBoxFont.Clone();
                
                _styleSheet = new StyleSheet(name, style);

                //_styleSheet[StyleNames.SelectedStyle].PenColor=Color.FromArgb(50, 150, 150, 150);
                _styleSheet[StyleNames.SelectedStyle].Pen.Thickness = style.Pen.Thickness;

                var font = (Font)_styleSheet[StyleNames.DefaultStyle].Font.Clone();
                font.Style = FontStyle.Underline;
                _styleSheet[StyleNames.SelectedStyle].Font = font;
                    
                _styleSheet[StyleNames.HoveredStyle].PenColor =
                    Color.FromArgb(50, 150, 150, 150);
                _styleSheet[StyleNames.HoveredStyle].FillColor = style.FillColor;

                _styleSheet[StyleNames.EdgeStyle].TextColor = 
                    Color.FromArgb(150, 100, 100, 100);
                _styleSheet[StyleNames.EdgeStyle].PenColor = 
                    Color.FromArgb(150, 180, 180, 180);
                _styleSheet[StyleNames.EdgeStyle].FillColor = 
                    KnownColors.FromKnownColor (KnownColor.White);
                _styleSheet[StyleNames.EdgeStyle].Font =
                    CreateFont(style.Font.FontFamily, style.Font.Size - 2.0d);

                Pen pen = _styleSheet[StyleNames.EdgeStyle].Pen;
                pen.Thickness = 0.5d;
                _styleSheet[StyleNames.EdgeStyle].Pen = pen;

                _styleSheet[StyleNames.EdgeHoveredStyle].FillColor = 
                    _styleSheet[StyleNames.EdgeStyle].FillColor;
                _styleSheet[StyleNames.EdgeHoveredStyle].PenColor = 
                    _styleSheet[StyleNames.EdgeStyle].PenColor;
                _styleSheet[StyleNames.EdgeHoveredStyle].TextColor = 
                    _styleSheet[StyleNames.EdgeStyle].TextColor;

                _styleSheet[StyleNames.EdgeSelectedStyle].TextColor =
                    Color.FromArgb(200, 50, 50, 50);
                _styleSheet[StyleNames.EdgeSelectedStyle].PenColor =
                    Color.FromArgb(50, 180, 180, 180);
                _styleSheet[StyleNames.EdgeSelectedStyle].FillColor = 
                    _styleSheet[StyleNames.EdgeStyle].FillColor;

            }
            _styleSheet.BackColor = KnownColors.FromKnownColor (KnownColor.Window);
            return _styleSheet;
        }
        public virtual void Init() {
            foreach (string name in StyleSheetNames) {
                IStyleSheet sheet = PredefinedStyleSheets (name);
                if (sheet != null) {
                    if (!this.ContainsKey (sheet.Name))
                        this.Add (sheet.Name, sheet);
                }
            }
        }
    }
}