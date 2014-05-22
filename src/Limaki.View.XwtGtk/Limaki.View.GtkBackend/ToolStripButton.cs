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
using System.Diagnostics;


namespace Limaki.View.GtkBackend {

    public class ToolStripButton : Gtk.ToolItem, IToolStripCommandItem, IToolStripItem {

        public ToolStripButton (): base () {
            Compose ();
        }
        
        public bool UseUnderline { get; set; }

        public string Label { get; set; }

        public Gtk.Widget ButtonWidget {
            get { return base.Child; }
            set {
                if (base.Child != value) {
                    base.Remove (base.Child);
                    base.Child = value;
                }
            }
        }

        protected virtual void Compose () {
            AddEvents ((int) Gdk.EventMask.FocusChangeMask);
            SetContent (Xwt.ContentPosition.Bottom);
        }

        [GLib.ConnectBefore]
        protected virtual void ButtonReleased (object o, Gtk.ButtonReleaseEventArgs args) {
            Trace.WriteLine ("ButtonReleased");
        }


        [GLib.ConnectBefore]
        protected virtual void ButtonPressed (object o, Gtk.ButtonPressEventArgs args) {
            Trace.WriteLine ("ButtonPressed");
            OnToolStripItemClick (o, new EventArgs ());
        }

        [GLib.ConnectBefore]
        protected virtual void DropDownPressed (object o, Gtk.ButtonPressEventArgs e) {
            Trace.WriteLine ("DropDownPressed");
            if (e.Event.Button != 1)
                return;
        }

        protected virtual Xwt.ButtonType ButtonType { get { return Xwt.ButtonType.Normal; } }

        public virtual string Text {
            get { return this.Label; } 
            set { this.Label = value; }
        }

        public IToolStripCommandItem ToggleOnClick { get; set; }

        protected ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set {
                var first = _command == null;
                VidgetUtils.SetCommand (this, ref _command, value);
                if (first)
                    Compose ();
            }
        }

        protected Xwt.Drawing.Image _image = null;
        public virtual Xwt.Drawing.Image Image {
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
                                                                                               Yalign=0,
                                                                                               Xalign=0,
                                                                                           };
                }
                if (this.Image != null && _imageWidget.Image.Backend != this.Image.GetBackend ()) {
                    _imageWidget.Image = this.Image.ToImageDescription ();
                    _imageWidget.QueueDraw();
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

        protected virtual void SetContent ( Xwt.ContentPosition position) {

            var label = this.Label;
            if (label != null && label.Length == 0)
                label = null;


            if (ButtonType == Xwt.ButtonType.Disclosure) {
                this.Label = null;
                this.ButtonWidget = new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out);
                this.ButtonWidget.ShowAll ();
                return;
            }

            Gtk.Widget contentWidget = null;
            
            if (label != null && Image == null) {
                contentWidget = new Gtk.Label (label) { UseUnderline = this.UseUnderline };

            } else if (label == null && Image != null) {
                contentWidget = AllocEventBox( ImageWidget);
                contentWidget.AddEvents ((int)Gdk.EventMask.ButtonPressMask);
                contentWidget.ButtonPressEvent += this.ButtonPressed;

            } else if (label != null && Image != null) {
                var box = position == Xwt.ContentPosition.Left || position == Xwt.ContentPosition.Right ? (Gtk.Box) new Gtk.HBox (false, 3) : (Gtk.Box) new Gtk.VBox (false, 3);
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

            if ( ButtonType == Xwt.ButtonType.DropDown) {
                Gtk.Widget dropDownArrow = new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out);
                dropDownArrow = AllocEventBox (dropDownArrow);
                dropDownArrow.AddEvents ((int)Gdk.EventMask.ButtonPressMask);
                dropDownArrow.ButtonPressEvent += this.DropDownPressed;

                if (contentWidget != null) {
                    var box = new Gtk.HBox (false,3);
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
                this.ButtonWidget = contentWidget;

            } else
                this.Label = null;
        }

        public Gtk.Widget WrapWithButton (Gtk.Widget widget) {
            if (widget is Gtk.Button) {
                return widget;
            }

            var button = new Gtk.Button () {
                Image = widget,
                Label =this.Label,
                Visible = widget.Visible,
                Sensitive = widget.Sensitive,
                UseUnderline = this.UseUnderline
            };
            button.AddEvents ((int)Gdk.EventMask.ButtonPressMask);
            button.ButtonPressEvent += this.ButtonPressed;
            button.Clicked += this.OnToolStripItemClick;
            GtkEngine.ReplaceChild (widget, button);

            return button;
        }

        public Gtk.Widget AllocEventBox (Gtk.Widget widget, bool visibleWindow = false) {
            // Wraps the widget with an event box. Required for some
            // widgets such as Label which doesn't have its own gdk window
            
            if (widget is Gtk.EventBox) {
                ((Gtk.EventBox)widget).VisibleWindow = true;
                return widget;
            }

            if (widget.IsNoWindow) {

                var eventBox = new Gtk.EventBox ();
                eventBox.Visible = widget.Visible;
                eventBox.Sensitive = widget.Sensitive;
                eventBox.VisibleWindow = visibleWindow;
                GtkEngine.ReplaceChild (widget, eventBox);
                eventBox.Add (widget);
                return eventBox;
            }
            return widget;
        }




        public bool isOpen { get; set; }
    }
}