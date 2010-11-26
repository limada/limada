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

        private string _name = string.Empty;
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

		///<directed>True</directed>
		private IStyle _parentStyle=null;
        public virtual IStyle ParentStyle {
            get { return _parentStyle; }
            set { _parentStyle = value; }
        }

        private Color _fillColor = Color.Empty;
        public Color FillColor {
            get {
                if ((_fillColor == Color.Empty) && (_parentStyle != null)) {
                    return _parentStyle.FillColor;
                } else {
                    return _fillColor;
                }
            }
            set { _fillColor = value; }
        }
        private Color _textColor = Color.Empty;
        public Color TextColor {
            get {
                if ((_textColor == Color.Empty) && (_parentStyle != null)) {
                    return _parentStyle.TextColor;
                } else {
                    return _textColor;
                }
            }
            set { _textColor = value; }
        }

        private Color _penColor = default(Color);
        public Color PenColor {
            get {
                if ((_penColor == default(Color)) && (_parentStyle != null)) {
                    return _parentStyle.PenColor;
                } else {
                    return _penColor;
                }
            }
            set {
                _penColor = value;
                if (_pen != null) {
                    _pen.Color = PenColor;
                } else if (_parentStyle!=null && _parentStyle.PenColor != value) {
                    _pen = (Pen)_parentStyle.Pen.Clone();
                    _pen.Color = _penColor;
                }
            }
        }

        private Pen _pen= default(Pen);
        public Pen Pen {
            get {
                if ((_pen == default(Pen)) && (_parentStyle != null)) {
                    return _parentStyle.Pen;
                } else {
                    return _pen;
                }
            }
            set {
                if (value == null ||
                    _parentStyle == null ||
                    _parentStyle.Pen == null ||
                    !_parentStyle.Pen.Equals(value)) {
                    _pen = value;
                }
            }
        }
        private Font _font=null;
        public Font Font {
            get {
                if ((_font == null) && (_parentStyle != null)) {
                    return _parentStyle.Font;
                } else {
                    return _font;
                }
            }
            set { 
                 if  (value==null ||
                     _parentStyle == null ||
                     _parentStyle.Font == null ||
                     ! _parentStyle.Font.Equals(value)) {
                     _font = value;    
                 }
            }
        }

        public static SizeI NoSize = new SizeI (int.MaxValue, int.MaxValue);
        private SizeI _autoSize = NoSize;
        public SizeI AutoSize {
            get {
                if (object.ReferenceEquals(_autoSize,NoSize) && (_parentStyle != null)) {
                    return _parentStyle.AutoSize;
                } else {
                    return _autoSize;
                }
            }
            set { _autoSize = value; }
        }

        private bool? _showText = true;
        public bool PaintData {
            get {
                if ((_showText==null) && (_parentStyle != null)) {
                    return _parentStyle.PaintData;
                } else {
                    return _showText.Value;
                }
            }
            set { _showText = value; }
        }
        #endregion

        #region IDisposable Member
        
        ~Style() {
            Dispose(false);
        }

        public void Dispose(bool disposing) {
            if (_font != null) {
                _font.Dispose ();
            }
            if (_pen != null) {
                _pen.Dispose ();
            }
        }

        public void Dispose() {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        #endregion


        public override string ToString() {
            return this.Name;
        }


    }
}
