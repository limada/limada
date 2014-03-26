/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing.WpfBackend;
using Limaki.View.Vidgets;
using System.Windows.Controls;
using Xwt.WPFBackend;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Limaki.View.WpfBackends {

    public class ToolStripButton : Button, IToolStripCommandItem, IToolStripItem {

        public ToolStripButton() {
            Compose();
        }

        protected virtual void Compose () {
            Style = (Style) FindResource (ToolBar.ButtonStyleKey);
            this.Content = WpfImage;
        }

        protected ToolStripCommand _command = null;
        public new ToolStripCommand Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand (this, ref _command, value); }
        }

        public IToolStripCommandItem ToggleOnClick { get; set; }

        public virtual Xwt.Size Size {
            get { return new Xwt.Size (this.Width, this.Height); }
            set { 
                //this.Width = value.Width;
                //this.Height = value.Height;
            }
        }

        protected FixedBitmap _wpfImage = null;
        protected FixedBitmap WpfImage {
            get { return _wpfImage ?? (_wpfImage = new FixedBitmap ()); }
        }

        protected Xwt.Drawing.Image _image = null;
        public virtual Xwt.Drawing.Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                    WpfImage.Source = _image.ToWpf() as BitmapSource;
                    //WpfImage.Width = _image.Width;
                }
            }
        }

        public virtual string Text { get; set; }

        public virtual string ToolTipText { get { return base.ToolTip.ToString(); } set { base.ToolTip = value; } }

        public  virtual new event System.EventHandler Click {
            add { base.Click += (s, e) => value (s, e); }
            remove { }
        }
   
    }
}
