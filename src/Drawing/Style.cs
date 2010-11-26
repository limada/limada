/*
 * Limaki 
 * Version 0.063
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

using System.Drawing;

namespace Limaki.Drawing {
    /// <summary>
    /// Zusammenfassung für Style.
    /// </summary>
    public class Style : IStyle {
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
            set { _pen = value; }
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
            set { _font = value; }
        }

        public static Size NoSize = new Size (int.MaxValue, int.MaxValue);
        private Size _autoSize = NoSize;
        public Size AutoSize {
            get {
                if (object.ReferenceEquals(_autoSize,NoSize) && (_parentStyle != null)) {
                    return _parentStyle.AutoSize;
                } else {
                    return _autoSize;
                }
            }
            set { _autoSize = value; }
        }

        #endregion

        #region IDisposable Member

        public void Dispose() { }

        #endregion





    }
}
