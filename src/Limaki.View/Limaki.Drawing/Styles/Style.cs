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
 */

using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Styles {
    /// <summary>
    /// Zusammenfassung für Style.
    /// </summary>
    public class Style : IStyle, IDisposable {
        public Style(string name) {
            this.Name = name;
        }
        public Style(string name, IStyle parentStyle):this(name) {
            ParentStyle = parentStyle;
        }
        #region IStyle Member


        public virtual string Name { get; set; }

        protected IStyle _parentStyle = null;
        public virtual IStyle ParentStyle {
            get { return _parentStyle; }
            set {
                if (value != _parentStyle) {
                    _parentStyle = value;
                    CopyTo(this);
                }
            }
        }

        protected Color? _fillColor = null;
        public virtual Color FillColor {
            get { return _fillColor ?? Get (() => ParentStyle.FillColor, DrawingExtensions.EmptyColor); }
            set { Set (() => ParentStyle.FillColor, ref _fillColor, value); }
        }

        protected Color? _textColor = null;
        public virtual Color TextColor {
            get { return _textColor ?? Get (() => ParentStyle.TextColor, DrawingExtensions.EmptyColor); }
            set { Set(() => ParentStyle.TextColor, ref _textColor, value); }
        }

        protected Color? _lineColor = null;
        public virtual Color StrokeColor {
            get { return _lineColor ?? Get (() => ParentStyle.StrokeColor, DrawingExtensions.EmptyColor); }
            set { Set (() => ParentStyle.StrokeColor, ref _lineColor, value); }
        }


        protected Pen _pen = null;
        [Obsolete()]
        public virtual Pen Pen {
            get { return _pen ?? Get (() => ParentStyle.Pen); }
            set {
                Set(() => ParentStyle.Pen, ref _pen, value);
                if (value != null) {
                    this.PenColor = value.Color;
                    this.PenThickness = value.Thickness;
                } 
                //else {
                //    this._penColor = null;
                //    this._penThickness = null;
                //}
            }
        protected double? _lineWidth = null;
        public virtual double LineWidth {
            get { return _lineWidth ?? Get (() => ParentStyle.LineWidth, 1d); }
            set { Set (() => ParentStyle.LineWidth, ref _lineWidth, value); }
        }

        protected Font _font=null;
        public virtual Font Font {
            get { return _font ?? GetC (() => ParentStyle.Font, _font); }
            set { Set(() => ParentStyle.Font, ref _font, value); }
        }

        public virtual TextDecoration TextDecoration { get; set; }

        public static Size NoSize = new Size (int.MaxValue, int.MaxValue);
        protected Size? _autoSize = null;
        public virtual Size AutoSize {
            get { return _autoSize ?? Get (() => ParentStyle.AutoSize, NoSize); }
            set { Set(()=>ParentStyle.AutoSize, ref _autoSize, value); }
        }

        protected bool? _paintData = null;
        public virtual bool PaintData {
            get { return _paintData ?? Get (() => ParentStyle.PaintData, true); }
            set { Set(()=>ParentStyle.PaintData, ref _paintData, value); }
        }
     

        #endregion

        #region cascading

        protected T Get<T> (Func<T> parentMemnber) where T : class {
            if (_parentStyle == null)
                return null;
            return parentMemnber ();

        }

        protected void Set<T>(Func<T> parentMemnber, ref T member, T value) where T : class {
            if (ParentStyle != null && parentMemnber() != null && parentMemnber().Equals(value)) {
                member = null;
            } else {
                member = value;
            }
        }

        protected T GetC<T> (Func<T> parentMemnber, T deefault) where T : class {
            if (_parentStyle != null)
                return parentMemnber ();
            else
                return deefault;
        }

        protected T Get<T> (Func<Nullable<T>> parentMember, T deefault) where T : struct {
            if (_parentStyle != null)
                return parentMember ().Value;
            else
                return deefault;
        }

        protected void Set<T>(Func<Nullable<T>> parentMember, ref Nullable<T> member, T value) where T : struct {
            if (ParentStyle == null || !parentMember().Equals(value))
                member = value;
            if (ParentStyle != null && parentMember().Equals(value))
                member = null;
        }
        #endregion

        #region IDisposable Member

        ~Style() {
            Dispose(false);
        }

        public virtual void Dispose(bool disposing) {
            if (_font != null) {
                _font = null;
            }
            if (_pen != null) {
                _pen.Dispose ();
                _pen = null;
            }
        }

        public virtual void Dispose() {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        #endregion


        public override string ToString() {
            return this.Name;
        }

        public override bool Equals(object obj) {
            if (!(obj is IStyle))
                return false;
            var other = (IStyle)obj;
            return
                   this.AutoSize == other.AutoSize &&
                   this.FillColor.Equals(other.FillColor) &&
                   this.Font == other.Font &&
                   this.Name == other.Name &&
                   this.PaintData == other.PaintData &&
                   this.LineWidth.Equals (other.LineWidth) &&
                   this.StrokeColor.Equals(other.StrokeColor) &&
                   this.TextColor.Equals(other.TextColor);
        }

        public override int GetHashCode() {
            int result =
                this.AutoSize.GetHashCode() ^
                this.FillColor.GetHashCode() ^
                this.Font.GetHashCode()^
                this.Name.GetHashCode() ^
                this.PaintData.GetHashCode() ^
                this.StrokeColor.GetHashCode() ^
                this.LineWidth.GetHashCode() ^
                this.TextColor.GetHashCode();
            if (this.Font != null) {
                result ^= this.Font.GetHashCode ();
            }

            return result;
        }


        public virtual void CopyTo(IStyle target) {
            target.AutoSize = this.AutoSize;
            target.FillColor = this.FillColor;
            target.Font = this.Font;
            target.PaintData = this.PaintData;
            target.StrokeColor = this.StrokeColor;
            target.LineWidth = this.LineWidth;
            target.TextColor = this.TextColor;
        }

        public virtual object Clone() {
            var result = Activator.CreateInstance(this.GetType(), new object[] { "Clone." + this.Name }) as IStyle;
            CopyTo(result);
            return result; 

        }


    }
}
