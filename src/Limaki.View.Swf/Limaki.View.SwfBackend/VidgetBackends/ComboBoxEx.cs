using Limaki.View.GdiBackend;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Xwt.GdiBackend;

namespace Limaki.View.SwfBackend.VidgetBackends {

    /// <summary>
    /// ComboBox accepting xwt.image as item
    /// </summary>
    public class ComboBoxEx:ComboBox {
        private bool _hasGraphics = false;
        public bool HasGraphics {
            get { return _hasGraphics;}
            set {
                if (_hasGraphics != value) {
                    _hasGraphics = value;
                    if (_hasGraphics) {
                        this.DrawMode = DrawMode.OwnerDrawFixed;
                        DropDownStyle = ComboBoxStyle.DropDownList;

                        SetStyle (ControlStyles.UserPaint, true);
                        SetStyle (ControlStyles.AllPaintingInWmPaint, true);
                        SetStyle (ControlStyles.OptimizedDoubleBuffer, true);
                        SetStyle (ControlStyles.ResizeRedraw, true);
                        SetStyle (ControlStyles.SupportsTransparentBackColor, true);
                    }
                }
            }
        }

        protected virtual void DrawShape (Graphics g, Rectangle bounds, int index, DrawItemState state) {
            if (index != -1) {
                var shape = this.Items[index] as Xwt.Drawing.Image;
                if (shape != null) {

                    var color = state.HasFlag (DrawItemState.Selected) ? SystemColors.Highlight : BackColor;
                    using (var b = new SolidBrush (color)) {
                        g.FillRectangle (b, bounds);
                    }
                    if (state.HasFlag (DrawItemState.ComboBoxEdit)) {
                        bounds.Inflate (-5, -3);
                    } else {
                        bounds.Inflate (-5, -3);
                    }
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    var location = bounds.Location.ToXwt ();
                    var size = bounds.Size.ToXwt ();
                    var surface = new GdiSurface () {Graphics = g};
                    surface.Context.SetColor (color.ToXwt ());
                    surface.Context.DrawImage (shape, location, 1);
                }
            }
           
        }
        protected override void OnDrawItem (DrawItemEventArgs e) {
            DrawShape (e.Graphics, e.Bounds, e.Index, e.State);
        }

        protected override void OnPaint (PaintEventArgs e) {
            var r = Rectangle.Empty;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            OnDrawItem (new DrawItemEventArgs (e.Graphics, Font, e.ClipRectangle, SelectedIndex, DrawItemState.ComboBoxEdit));

            var buttonRect = new Rectangle (Width - 15, 0, 15, Height);
            ControlPaint.DrawComboButton (e.Graphics, buttonRect, ButtonState.Normal | ButtonState.Flat);
            
        }
    }
}