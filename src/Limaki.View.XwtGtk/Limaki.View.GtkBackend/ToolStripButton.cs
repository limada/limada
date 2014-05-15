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

            SetContent (Xwt.ContentPosition.Bottom);
        }

        [GLib.ConnectBefore]
        protected virtual void ButtonReleased (object o, Gtk.ButtonReleaseEventArgs args) {
            Trace.WriteLine ("ButtonReleased");
        }


        [GLib.ConnectBefore]
        protected virtual void ButtonPressed (object o, Gtk.ButtonPressEventArgs args) {
            Trace.WriteLine ("ButtonPressed");
            
        }

        [GLib.ConnectBefore]
        protected virtual void DropDownPressed (object o, Gtk.ButtonPressEventArgs e) {
            Trace.WriteLine ("DropDownPressed");
            if (e.Event.Button != 1)
                return;
            ShowDropDown ();
        }

        private void ShowDropDown () {
            PopoverWindow.Show (this.ButtonWidget, Xwt.Rectangle.Zero, null);
            var menu = CreateMenu ();

            if (menu != null) {
                isOpen = true;
                var button = this.ButtonWidget as Gtk.Button;
                var oldRelief = Gtk.ReliefStyle.Normal;
                if (button != null) {
                    //make sure the button looks depressed
                    oldRelief = button.Relief;
                    button.Relief = Gtk.ReliefStyle.Normal;
                }
                this.ButtonWidget.State = Gtk.StateType.Active;
                //clean up after the menu's done
                menu.Hidden += delegate {
                                   if (button != null) {
                                       button.Relief = oldRelief;
                                   }
                                   isOpen = false;
                    this.ButtonWidget.State = Gtk.StateType.Normal;

                    //FIXME: for some reason the menu's children don't get activated if we destroy 
                    //directly here, so use a timeout to delay it
                    GLib.Timeout.Add (100, delegate {
                        //menu.Destroy ();
                        return false;
                    });
                };
                menu.Popup (null, null, PositionFunc, 1, Gtk.Global.CurrentEventTime);
            }
        }

        void PositionFunc (Gtk.Widget mn, out int x, out int y, out bool push_in) {
            Gtk.Widget w = (Gtk.Widget)this;
            w.GdkWindow.GetOrigin (out x, out y);
            Gdk.Rectangle rect = w.Allocation;
            x += rect.X;
            y += rect.Y + rect.Height;

            //if the menu would be off the bottom of the screen, "drop" it upwards
            if (y + mn.Requisition.Height > w.Screen.Height) {
                y -= mn.Requisition.Height;
                y -= rect.Height;
            }

            //let GTK reposition the button if it still doesn't fit on the screen
            push_in = true;
        }

        private Gtk.Menu CreateMenu () {
            return null;
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
                    _imageWidget = new ImageBox (Xwt.Toolkit.Engine<GtkEngine> ().Context);
                }
                if (this.Image != null && _imageWidget.Image.Backend != this.Image.GetBackend ()) {
                    _imageWidget.Image = this.Image.ToImageDescription ();
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
                    box.PackStart (new Gtk.VSeparator (), true, true, 0);
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