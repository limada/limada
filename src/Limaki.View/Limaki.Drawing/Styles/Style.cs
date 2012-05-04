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
            get { return Get(() => ParentStyle.FillColor, _fillColor, DrawingExtensions.EmptyColor); }
            set { Set(() => ParentStyle.FillColor, ref _fillColor, value); }
        }

        protected Color? _textColor = null;
        public virtual Color TextColor {
            get { return Get(() => ParentStyle.TextColor, _textColor, DrawingExtensions.EmptyColor); }
            set { Set(() => ParentStyle.TextColor, ref _textColor, value); }
        }

        protected Color? _penColor = null;
        public virtual Color PenColor {
            get { return Get(() => ParentStyle.PenColor, _penColor, DrawingExtensions.EmptyColor); }
            set {
                if (!value.Equals(PenColor)) {
                    Set(() => ParentStyle.PenColor, ref _penColor, value);
                    if (_penColor != null) {
                        if (_pen == null) {
                            _pen = (Pen) Pen.Clone();
                        }
                        _pen.Color = _penColor.Value;
                    } else {
                        if (_pen != null) {
                            _pen.Color = PenColor;
                            Pen = _pen;
                        }
                    }
                }
            }
        }

        protected Pen _pen = null;
        public virtual Pen Pen {
            get { return Get(() => ParentStyle.Pen, _pen); }
            set {
                Set(() => ParentStyle.Pen, ref _pen, value); 
                if (value != null)
                    this.PenColor = value.Color;
                else
                    this._penColor = null;
            }
        }

        protected Font _font=null;
        public virtual Font Font {
            get { return Get(() => ParentStyle.Font, _font); }
            set { Set(() => ParentStyle.Font, ref _font, value); }
        }

        public virtual TextDecoration TextDecoration { get; set; }

        public static Size NoSize = new Size (int.MaxValue, int.MaxValue);
        protected Size? _autoSize = null;
        public virtual Size AutoSize {
            get { return Get(() => ParentStyle.AutoSize, _autoSize, NoSize); }
            set { Set(()=>ParentStyle.AutoSize, ref _autoSize, value); }
        }

        protected bool? _paintData = null;
        public virtual bool PaintData {
            get { return Get(() => ParentStyle.PaintData, _paintData, true); }
            set { Set(()=>ParentStyle.PaintData, ref _paintData, value); }
        }
     

        #endregion

        #region cascading

        protected T Get<T>(Func<T> parentMemnber, T member) where T : class {
            if ((member == null) && (ParentStyle != null)) {
                return parentMemnber();
            } else {
                return member;
            }
        }

        protected void Set<T>(Func<T> parentMemnber, ref T member, T value) where T : class {
            if (ParentStyle != null && parentMemnber() != null && parentMemnber().Equals(value)) {
                member = null;
            } else {
                member = value;
            }
        }

        protected T Get<T>(Func<T> parentMemnber, T member, T deefault) where T : class {
            if (member == null)
                if (ParentStyle != null)
                    return parentMemnber();
                else
                    return deefault;
            return member;
        }

        protected T Get<T>(Func<Nullable<T>> parentMemnber, Nullable<T> member, T deefault) where T : struct {
            if (member == null)
                if (ParentStyle != null)
                    return parentMemnber().Value;
                else
                    return deefault;
            return member.Value;
        }

        protected void Set<T>(Func<Nullable<T>> parentMemnber, ref Nullable<T> member, T value) where T : struct {
            if (ParentStyle == null || !parentMemnber().Equals(value))
                member = value;
            if (ParentStyle != null && parentMemnber().Equals(value))
                member = null;
        }
        #endregion

        #region IDisposable Member

        ~Style() {
            Dispose(false);
        }

        public virtual void Dispose(bool disposing) {
            if (_font != null) {
                _font.Dispose ();
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
                   this.Pen == other.Pen &&
                   this.PenColor.Equals(other.PenColor) &&
                   this.TextColor.Equals(other.TextColor);
        }

        public override int GetHashCode() {
            int result =
                this.AutoSize.GetHashCode() ^
                this.FillColor.GetHashCode() ^
                this.Font.GetHashCode()^
                this.Name.GetHashCode() ^
                this.PaintData.GetHashCode() ^
                this.PenColor.GetHashCode() ^
                this.TextColor.GetHashCode();
            if (this.Font != null) {
                result ^= this.Font.GetHashCode ();
            }
            if (this.Pen != null) {
                result ^= this.Pen.GetHashCode();
            }
            return result;
        }


        public virtual void CopyTo(IStyle target) {
            target.AutoSize = this.AutoSize;
            target.FillColor = this.FillColor;
            target.Font = (Font)this.Font.Clone();
            target.PaintData = this.PaintData;
            target.Pen = (Pen)this.Pen.Clone();
            target.PenColor = this.PenColor;
            target.TextColor = this.TextColor;
        }

        public virtual object Clone() {
            var result = Activator.CreateInstance(this.GetType(), new object[] { "Clone." + this.Name }) as IStyle;
            CopyTo(result);
            return result; 

        }


    }
}
