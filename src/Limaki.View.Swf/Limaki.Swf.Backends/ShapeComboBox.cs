/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.Styles;
using Limaki.View.Viz.Modelling;
using Xwt.Gdi.Backend;

namespace Limaki.Swf.Backends.Viewers {

    public partial class ShapeComboBox : ComboBox {

        public ShapeComboBox() {

            this.DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

            ShapeFactory.Shapes ().ForEach (s => Items.Add (s));

        }


        private ShapeLayout _shapeLayout = null;
        public ShapeLayout ShapeLayout {
            get {
                if (_shapeLayout == null) {
                    var styleSheets = Registry.Pooled<StyleSheets>();
                    _shapeLayout = new ShapeLayout(styleSheets.DefaultStyleSheet);
                }
                return _shapeLayout;
            }
            set { _shapeLayout = value; }
        }

        protected virtual void DrawShape(Graphics g, Rectangle bounds, int index, DrawItemState state) {
            if (index != -1) {
                var shape = this.Items[index] as IShape;
                if (shape != null) {
                    using (var b = new SolidBrush(this.BackColor)) {
                        g.FillRectangle(b, bounds);
                    }
                    if ((state & DrawItemState.ComboBoxEdit) == 0) {
                        bounds.Inflate(-5, -3);
                    } else {
                        bounds.Inflate(-3, -3);
                    }
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    shape.Location = bounds.Location.ToXwt ();
                    shape.Size = bounds.Size.ToXwt ();
                    var painter = ShapeLayout.GetPainter(shape.GetType());
                    var uiState = UiState.None;
                    if ((state & DrawItemState.Focus) != 0) {
                        uiState = UiState.Hovered;
                    }
                    if ((state & (DrawItemState.Selected | DrawItemState.Default)) != 0) {
                        uiState = UiState.Selected;
                    }
                    painter.Style = ShapeLayout.GetStyle(shape, uiState);
                    painter.Shape = shape;
                    painter.Render (new GdiSurface () { Graphics = g });
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e) {
            DrawShape(e.Graphics, e.Bounds, e.Index, e.State);
        }

        protected override void OnPaint(PaintEventArgs e) {
            var r = Rectangle.Empty;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var buttonRect = new Rectangle(Width - 15, 0, 15, Height);
            ControlPaint.DrawComboButton(e.Graphics, buttonRect, ButtonState.Normal | ButtonState.Flat);
        }
    }
}