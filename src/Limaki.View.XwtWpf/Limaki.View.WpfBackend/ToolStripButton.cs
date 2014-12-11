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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Limaki.Drawing.WpfBackend;

namespace Limaki.View.WpfBackend {

    public class ToolStripButton : ToggleButton {

        public ToolStripButton () {
            Compose ();
            base.Click += OnButtonClicked;
        }

        protected virtual void Compose () {
            ComposeStyle ();
            this.Content = ButtonImage;
        }

        protected virtual void ComposeStyle () {
            var style = new Style ();

            // set opacity to 50% if not enabled:
            var trigger = new DataTrigger {
                Value = false,
                Binding = WpfExtensions.Binding (this, o => o.IsEnabled, BindingMode.OneWay),
            };

            var setter = new Setter {
                Property = UIElement.OpacityProperty,
                Value = 0.5,
            };

            trigger.Setters.Add (setter);
            style.Triggers.Add (trigger);

            var baseStyle = ToolStripUtils.ToolbarItemStyle(this);
            style.TargetType = baseStyle.TargetType;
            style.BasedOn = baseStyle;
            this.Style = style;
        }

        public bool IsCheckable { get; set; }

        protected override void OnChecked (RoutedEventArgs e) {
            if (!IsCheckable && (!IsChecked.HasValue || IsChecked.Value)) {
                IsChecked = false;
            }
            base.OnChecked (e);
        }

        protected FixedBitmap _innerButton = null;
        protected FixedBitmap ButtonImage {
            get {
                if (_innerButton == null) {
                    _innerButton = new FixedBitmap ();
                }
                return _innerButton;
            }
        }

        protected Xwt.Drawing.Image _image = null;
        public virtual Xwt.Drawing.Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                    ButtonImage.Source = _image.ToWpf () as BitmapSource;
                    ButtonImage.InvalidateMeasure ();
                    ButtonImage.InvalidateVisual ();
                    //ButtonImage.Width = _image.Width;
                }
            }
        }

        public virtual string Label { get; set; }

        public virtual string ToolTipText {
            get {
                if (base.ToolTip != null)
                    return base.ToolTip.ToString ();
                else
                    return string.Empty;
            }
            set {
                if (!string.IsNullOrEmpty (value))
                    base.ToolTip = new ToolTip { Content = value };
            }
        }

        protected event System.EventHandler _click;
        public virtual new event System.EventHandler Click {
            add { _click += value; }
            remove { _click -= value; }
        }

        protected virtual void OnButtonClicked (object sender, RoutedEventArgs e) {
            if (_click != null)
                _click (this, e);
        }

        public virtual Xwt.Size Size {
            get { return new Xwt.Size (this.Width, this.Height); }
            set {
                //this.Width = value.Width;
                //this.Height = value.Height;
            }
        }
    }
}