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
    /// 
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

        protected Spacing? _padding = null;
        public virtual Spacing Padding {
            get { return _padding ?? Get (() => ParentStyle.Padding, new Spacing(5)); }
            set { Set (() => ParentStyle.Padding, ref _padding, value); }
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
            return deefault;
        }

        protected T Get<T> (Func<T?> parentMember, T deefault) where T : struct {
            if (_parentStyle != null)
                return parentMember ().Value;
            return deefault;
        }

        protected void Set<T>(Func<T?> parentMember, ref T? member, T value) where T : struct {
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
            var other = obj as IStyle;
            if (other == null)
                return false;
            return
                   this.AutoSize == other.AutoSize &&
                   this.FillColor.Equals(other.FillColor) &&
                   this.Font.Equals(other.Font) &&
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

        public static IStyle MakeCopy (IStyle source, IStyle baseStyle) {
            IStyle sink = null;
            if (source != null) {
                sink = new Style (source.Name, baseStyle);
                source.CopyTo (sink);
            }
            return sink;
        }

        public virtual void CopyTo(IStyle other) {
            other.AutoSize = this.AutoSize;
            other.FillColor = this.FillColor;
            other.Font = this.Font;
            other.PaintData = this.PaintData;
            other.StrokeColor = this.StrokeColor;
            other.LineWidth = this.LineWidth;
            other.TextColor = this.TextColor;
        }

        public virtual object Clone() {
            var result = Activator.CreateInstance(this.GetType(), new object[] { "Clone." + this.Name }) as IStyle;
            CopyTo(result);
            return result; 

        }


    }
}
