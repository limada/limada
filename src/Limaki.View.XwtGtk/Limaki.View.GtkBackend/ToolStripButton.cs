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

using System.Diagnostics;
using Xwt.Backends;
using Xwt.GtkBackend;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {

    public class ToolStripButton : ToolItem {

        public ToolStripButton (): base () {
            Compose ();
        }

        public bool UseUnderline { get; set; }

        protected virtual Gtk.Widget ContentWidget {
            get { return base.Child; }
            set {
                if (base.Child != value) {
                    base.Remove (base.Child);
                    base.Child = value;
                }
            }
        }

        public override Xwt.Drawing.Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                    ImageWidget.QueueDraw ();
                }
            }
        }

        Xwt.GtkBackend.ImageBox _imageWidget = null;
        protected Gtk.Widget ImageWidget {
            get {
                if (_imageWidget == null) {
                    _imageWidget = new ImageBox (Xwt.Toolkit.Engine<GtkEngine> ().Context) {
                        Yalign = 0,
                        Xalign = 0,
                    };
                }
                if (this.Image != null && _imageWidget.Image.Backend != this.Image.GetBackend ()) {
                    _imageWidget.Image = this.Image.ToImageDescription ();
                    _imageWidget.QueueDraw ();
                }
                return _imageWidget;
            }
        }

        protected virtual Xwt.ButtonType ButtonType { get { return Xwt.ButtonType.Normal; } }

        protected override void Compose () {
            base.Compose ();
            SetContent (Xwt.ContentPosition.Bottom);
        }

        [GLib.ConnectBefore]
        protected virtual void DropDownPressed (object o, Gtk.ButtonPressEventArgs e) {
            Trace.WriteLine ("DropDownPressed");
            if (e.Event.Button != 1)
                return;
        }

        protected virtual void SetContent (Xwt.ContentPosition position) {

            var label = this.Label;
            if (label != null && label.Length == 0)
                label = null;


            if (ButtonType == Xwt.ButtonType.Disclosure) {
                this.Label = null;
                this.ContentWidget = new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out);
                this.ContentWidget.ShowAll ();
                return;
            }

            Gtk.Widget contentWidget = null;

            if (label != null && Image == null) {
                contentWidget = new Gtk.Label (label) { UseUnderline = this.UseUnderline };

            } else if (label == null && Image != null) {
                contentWidget = AllocEventBox (ImageWidget);
                contentWidget.AddEvents ((int)Gdk.EventMask.ButtonPressMask);
                contentWidget.ButtonPressEvent += this.ButtonPressed;

            } else if (label != null && Image != null) {
                var box = position == Xwt.ContentPosition.Left || position == Xwt.ContentPosition.Right ? (Gtk.Box)new Gtk.HBox (false, 3) : (Gtk.Box)new Gtk.VBox (false, 3);
                var lab = new Gtk.Label (label) { UseUnderline = this.UseUnderline };

                if (position == Xwt.ContentPosition.Left || position == Xwt.ContentPosition.Top) {
                    box.PackStart (ImageWidget, false, false, 0);
                    box.PackStart (lab, false, false, 0);
                } else {
                    box.PackStart (lab, false, false, 0);
                    box.PackStart (ImageWidget, false, false, 0);
                }

                contentWidget = box;
            }

            if (ButtonType == Xwt.ButtonType.DropDown) {
                Gtk.Widget dropDownArrow = new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out);
                dropDownArrow = AllocEventBox (dropDownArrow);
                dropDownArrow.AddEvents ((int)Gdk.EventMask.ButtonPressMask);
                dropDownArrow.ButtonPressEvent += this.DropDownPressed;

                if (contentWidget != null) {
                    var box = new Gtk.HBox (false, 3);
                    box.PackStart (contentWidget, true, true, 3);
                    //box.PackStart (new Gtk.VSeparator (), true, true, 0);
                    box.PackStart (dropDownArrow, false, false, 0);
                    contentWidget = box;
                } else
                    contentWidget = dropDownArrow;
            }

            if (contentWidget != null) {
                contentWidget.ShowAll ();
                this.Label = null;
                this.ContentWidget = contentWidget;

            } else
                this.Label = null;
        }

        public Gtk.Widget WrapWithButton (Gtk.Widget widget) {
            if (widget is Gtk.Button) {
                return widget;
            }

            var button = new Gtk.Button () {
                Image = widget,
                Label = this.Label,
                Visible = widget.Visible,
                Sensitive = widget.Sensitive,
                UseUnderline = this.UseUnderline
            };
            button.AddEvents ((int)Gdk.EventMask.ButtonPressMask);
            button.ButtonPressEvent += this.ButtonPressed;
            button.Clicked += this.OnButtonClicked;
            GtkEngine.ReplaceChild (widget, button);

            return button;
        }


    }
}