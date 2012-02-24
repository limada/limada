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
using Limaki.Common;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.Styles;
using Limaki.Presenter.Layout;


namespace Limaki.UseCases.Winform.Viewers {
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
                    _shapeLayout = new ShapeLayout(styleSheets.DefaultStyleSheet);
                }
                return _shapeLayout;
            }
            set { _shapeLayout = value; }
        }

        protected virtual void drawShape(Graphics g, Rectangle bounds, int index, DrawItemState state) {
            if (index != -1) {
                var shape = this.Items[index] as IShape;
                if (shape != null) {
                    var rect = bounds;
                    using (Brush b = new SolidBrush(this.BackColor)) {
                        g.FillRectangle(b, bounds);
                    }
                    if ((state & DrawItemState.ComboBoxEdit) == 0) {
                        rect.Inflate(-5, -3);
                    } else {
                        rect.Inflate(-3, -3);
                    }
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    shape.Location = GDIConverter.Convert(rect.Location);
                    shape.Size = GDIConverter.Convert(rect.Size);
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

            Rectangle buttonRect = new Rectangle(Width - 15, 0, 15, Height);
            ControlPaint.DrawComboButton(e.Graphics, buttonRect, ButtonState.Normal | ButtonState.Flat);
        }
    }
}