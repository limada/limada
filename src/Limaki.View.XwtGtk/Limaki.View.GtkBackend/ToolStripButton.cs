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

using System;
using Limaki.View.Vidgets;
using Xwt.Backends;
using Xwt.GtkBackend;

namespace Limaki.View.GtkBackend {

    public class ToolStripButton : Gtk.ToolButton, IToolStripCommandItem, IToolStripItem {

        public ToolStripButton (): base ("") {
            Compose ();
           
        }

        protected virtual void Compose () {
            base.Clicked -= OnToolStripItemClick;
            base.Clicked += OnToolStripItemClick;
            ButtonType = Xwt.ButtonType.Normal;
            SetContent (false, Xwt.ContentPosition.Bottom);
        }

        public virtual string Text {
            get { return this.Label; } 
            set { this.Label = value; }
        }

        public IToolStripCommandItem ToggleOnClick { get; set; }

        protected ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set {
                VidgetUtils.SetCommand (this, ref _command, value);
                Compose ();
            }
        }

        protected Xwt.Drawing.Image _image = null;
        public virtual Xwt.Drawing.Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                }
            }
        }

        Xwt.GtkBackend.ImageBox _imageWidget = null;

        protected Gtk.Widget ImageWidget {
            get {
                if (_imageWidget == null) {
                    _imageWidget = new Xwt.GtkBackend.ImageBox (
                   Xwt.Toolkit.Engine<Xwt.GtkBackend.GtkEngine> ().Context
                   );
                }
                if (this.Image!=null && _imageWidget.Image.Backend != this.Image.GetBackend()) {
                    _imageWidget.Image = this.Image.ToImageDescription();
                }
                return _imageWidget;
            }
        }

        public string ToolTipText {
            get { return base.TooltipText; }
            set { base.TooltipText = value; }
        }

        public Xwt.Size Size {
            get { return this.VidgetBackendSize (); } 
            set { this.VidgetBackendSize (value); }
        }

        protected event System.EventHandler _click;
        public virtual new event System.EventHandler Click {
            add { _click += value; }
            remove { _click -= value; }
        }

        protected void OnToolStripItemClick (object sender, EventArgs e) {
            if (_click != null)
                _click (this, e);
        }

        public void SetContent (bool useMnemonic, Xwt.ContentPosition position) {
            base.UseUnderline = useMnemonic;
            var label = base.Label;
            if (label != null && label.Length == 0)
                label = null;

            if (label != null && Image == null && ButtonType == Xwt.ButtonType.Normal) {
                return;
            }

            if (ButtonType == Xwt.ButtonType.Disclosure) {
                base.Label = null;
                base.IconWidget = new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out);
                base.IconWidget.ShowAll ();
                return;
            }

            Gtk.Widget contentWidget = null;
            

            if (label != null && Image == null) {
                contentWidget = new Gtk.Label (label) { UseUnderline = useMnemonic };
            } else if (label == null && Image != null) {
                contentWidget = ImageWidget;
            } else if (label != null && Image != null) {
                var box = position == Xwt.ContentPosition.Left || position == Xwt.ContentPosition.Right ? (Gtk.Box) new Gtk.HBox (false, 3) : (Gtk.Box) new Gtk.VBox (false, 3);
                var lab = new Gtk.Label (label) { UseUnderline = useMnemonic };

                if (position == Xwt.ContentPosition.Left || position == Xwt.ContentPosition.Top) {
                    box.PackStart (ImageWidget, false, false, 0);
                    box.PackStart (lab, false, false, 0);
                } else {
                    box.PackStart (lab, false, false, 0);
                    box.PackStart (ImageWidget, false, false, 0);
                }

                contentWidget = box;
            }
            if ( ButtonType == Xwt.ButtonType.DropDown) {
                if (contentWidget != null) {
                    var box = new Gtk.HBox (false, 3);
                    box.PackStart (contentWidget, true, true, 3);
                    box.PackStart (new Gtk.VSeparator (), true, true, 0);
                    box.PackStart (new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out), false, false, 0);
                    contentWidget = box;
                } else
                    contentWidget = new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out);
            }
            if (contentWidget != null) {
                contentWidget.ShowAll ();
                base.Label = null;
                base.IconWidget = contentWidget;
            } else
                base.Label = null;
        }

        public Xwt.ButtonType ButtonType { get; set; }
    }
}