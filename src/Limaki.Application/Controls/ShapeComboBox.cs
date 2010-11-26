/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.Shapes;
using Limaki.Common;

namespace Limaki.Winform.Controls {

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
            fillWithShapes();
        }

        void fillWithShapes() {
            Items.Add(new RectangleShape());
            Items.Add(new RoundedRectangleShape());
            Items.Add(new VectorShape());
            Items.Add (new BezierShape ());
        }



        private ShapeLayout _shapeLayout = null;

        public ShapeLayout ShapeLayout {
            get {
                if (_shapeLayout == null) {
                    var styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
                    _shapeLayout = new ShapeLayout(delegate() { return null; },
                                                    styleSheets.DefaultStyleSheet);
                }
                return _shapeLayout;
            }
            set { _shapeLayout = value; }
        }

        protected virtual void drawShape(Graphics g, Rectangle bounds, int index, DrawItemState state) {
            if (index != -1) {
                IShape shape = this.Items[index] as IShape;
                if (shape != null) {
                    Rectangle rect = bounds;
                    using (Brush b = new SolidBrush(this.BackColor)) {
                        g.FillRectangle(b, bounds);
                    }
                    if ((state & DrawItemState.ComboBoxEdit) == 0) {
                        rect.Inflate(-5, -3);
                    } else {
                        rect.Inflate(-3, -3);
                    }
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    shape.Location = GDIExtensions.Toolkit(rect.Location);
                    shape.Size = GDIExtensions.Toolkit(rect.Size);
                    IPainter painter = ShapeLayout.GetPainter(shape.GetType());
                    UiState uiState = UiState.None;
                    if ((state & DrawItemState.Focus) != 0) {
                        uiState = UiState.Hovered;
                    }
                    if ((state & (DrawItemState.Selected | DrawItemState.Default)) != 0) {
                        uiState = UiState.Selected;
                    }
                    painter.Style = ShapeLayout.GetStyle(shape, uiState);
                    painter.Shape = shape;
                    painter.Render(new GDISurface(){Graphics=g});
                }
            }
        }
        protected override void OnDrawItem(DrawItemEventArgs e) {
            drawShape(e.Graphics, e.Bounds, e.Index, e.State);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Rectangle r = Rectangle.Empty;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            SolidBrush sb = new SolidBrush(BackColor);
            //if (Focused)
            //    sb.Color = SystemColors.Highlight;

            //e.Graphics.FillRectangle(sb, ClientRectangle);

            if (Items.Count != 0) {
                r = new Rectangle(
                    ClientRectangle.Left, ClientRectangle.Top,
                    ClientRectangle.Width - 15, ClientRectangle.Height - 1);
                //drawShape(e.Graphics, r, SelectedIndex, DrawItemState.Selected);

            }
            Rectangle buttonRect = new Rectangle(Width - 15, 0, 15, Height);
            ControlPaint.DrawComboButton(e.Graphics, buttonRect, ButtonState.Normal | ButtonState.Flat);

            sb.Dispose();
        }
    }
}
