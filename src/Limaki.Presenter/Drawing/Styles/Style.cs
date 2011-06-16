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
 */

using System;
namespace Limaki.Drawing {
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
        
        public virtual IStyle ParentStyle { get; set; }

        private Color? _fillColor = null;
        public virtual Color FillColor {
            get {
                if (_fillColor == null) {
                    if (ParentStyle != null)
                        return ParentStyle.FillColor;
                    return Color.Empty;
                }
                return _fillColor.Value;
                
            }
            set {
                if (ParentStyle == null || ParentStyle.FillColor != value)
                    _fillColor = value;
            }
        }

        private Color? _textColor = null;
        public virtual Color TextColor {
            get {
                if (_textColor == null){
                    if (ParentStyle != null)
                        return ParentStyle.TextColor;
                    return Color.Empty;
                } 
                return _textColor.Value;
            }
            set {
                if (ParentStyle == null || ParentStyle.TextColor != value)
                    _textColor = value;
            }
        }

        private Color? _penColor = null;
        public virtual Color PenColor {
            get {
                if (_penColor == null) {
                    if (ParentStyle != null)
                        return ParentStyle.PenColor;
                    return Color.Empty;
                }
                return _penColor.Value;
            }
            set {
                _penColor = value;
                if (_pen != null) {
                    _pen.Color = PenColor;
                } else if (ParentStyle!=null && ParentStyle.PenColor != value &&
                    ParentStyle.Pen != null) {
                    _pen = (Pen)ParentStyle.Pen.Clone();
                    _pen.Color = _penColor.Value;
                }
            }
        }

        private Pen _pen= null;
        public Pen Pen {
            get {
                if ((_pen == null) && (ParentStyle != null)) {
                    return ParentStyle.Pen;
                } else {
                    return _pen;
                }
            }
            set {
                if (value == null ||
                    ParentStyle == null ||
                    ParentStyle.Pen == null ||
                    !ParentStyle.Pen.Equals(value)) {
                    _pen = value;
                }
            }
        }
        private Font _font=null;
        public Font Font {
            get {
                if ((_font == null) && (ParentStyle != null)) {
                    return ParentStyle.Font;
                } else {
                    return _font;
                }
            }
            set { 
                 if  (value==null ||
                     ParentStyle == null ||
                     ParentStyle.Font == null ||
                     ! ParentStyle.Font.Equals(value)) {
                     _font = value;    
                 }
            }
        }

        public static SizeI NoSize = new SizeI (int.MaxValue, int.MaxValue);
        private SizeI? _autoSize = null;
        public SizeI AutoSize {
            get {
                if (_autoSize == null)
                    if (ParentStyle != null)
                        return ParentStyle.AutoSize;
                    else
                        return NoSize;
                return _autoSize.Value;
            }
            set {
                if (ParentStyle == null || ParentStyle.AutoSize != value)
                    _autoSize = value;
            }
        }

        private bool? _paintData = null;
        public bool PaintData {
            get {
                if (_paintData == null)
                    if (ParentStyle != null)
                        return ParentStyle.PaintData;
                    else
                        return true;
                
                return _paintData.Value;
                
            }
            set {
                if (ParentStyle == null || ParentStyle.PaintData != value)
                    _paintData = value;
            }
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
                   this.FillColor == other.FillColor &&
                   this.Font == other.Font &&
                   this.Name == other.Name &&
                   this.PaintData == other.PaintData &&
                   this.Pen == other.Pen &&
                   this.PenColor == other.PenColor &&
                   this.TextColor == other.TextColor;
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


        
        public virtual object Clone() {
            var result = Activator.CreateInstance(this.GetType(), new object[] { "Clone." + this.Name, this.ParentStyle }) as IStyle;
            result.AutoSize = this.AutoSize;
            result.FillColor = this.FillColor;
            result.Font = (Font)this.Font.Clone();
            result.PaintData = this.PaintData;
            result.Pen = (Pen)this.Pen.Clone();
            result.PenColor = this.PenColor;
            result.TextColor = this.TextColor;
            return result; 

        }


    }
}
