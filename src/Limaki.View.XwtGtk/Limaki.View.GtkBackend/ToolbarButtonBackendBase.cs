using System.Diagnostics;
using Gdk;
using Limaki.View.Vidgets;
using Xwt.Backends;
using Xwt.GtkBackend;

namespace Limaki.View.GtkBackend {

    public class ToolbarButtonBackendBase<T> : ToolbarItemBackend<T>, IToolbarButtonBackend where T : Gtk.ToolItem, new () {

        public new Vidgets.ToolbarButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (Vidgets.ToolbarButton) frontend;
        }

        public virtual bool? IsChecked { get; set; }
        public bool IsCheckable { get; set; }

        public virtual bool UseUnderline { get; set; }

        protected virtual Xwt.ButtonType ButtonType { get { return Xwt.ButtonType.Normal; } }

        public override bool IsEnabled {
            get { return base.IsEnabled; }
            set {
                if (value != base.IsEnabled) {
                    if (ImageWidget != null) {
                        var id = _imageWidget.Image;
                        id.Alpha = value ? 1 : .5d;
                        _imageWidget.Image = id;
                    }
                    base.IsEnabled = value;
                }
            }
        }

        public Color Notifycolor { get; set; }

        protected override void Compose () {
            base.Compose ();
            SetContent (Xwt.ContentPosition.Bottom);
#if XWT_GTKSHARP3
            Notifycolor = Xwt.GtkBackend.GtkSharpWorkarounds.Background (Widget.Style, Gtk.StateType.Prelight);
#else
            Notifycolor = Widget.Style.Background (Gtk.StateType.Prelight);
#endif
        }

        protected virtual Gtk.Widget ContentWidget {
            get { return Widget.Child; }
            set {
                if (Widget.Child != value) {
                    Widget.Remove (Widget.Child);
                    Widget.Child = value;
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

        private Xwt.GtkBackend.ImageBox _imageWidget = null;

        protected Gtk.Widget ImageWidget {
            get {
                if (_imageWidget == null) {
                    _imageWidget = new ImageBox (Xwt.Toolkit.Engine<GtkEngine> ().Context) {
                        Yalign = 0,
                        Xalign = 0,
                    };
                }
                if (this.Image != null && _imageWidget.Image.Backend != this.Image.GetBackend ()) {
                    _imageWidget.Image = this.Image.ToImageDescription (Xwt.Toolkit.Engine<GtkEngine>().Context);
                    _imageWidget.QueueDraw ();
                }
                return _imageWidget;
            }
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
                contentWidget = AllocEventBox (ImageWidget,true);
                contentWidget.AddEvents ((int) Gdk.EventMask.ButtonPressMask);
                contentWidget.ButtonPressEvent -= this.ButtonPressed;
                contentWidget.ButtonPressEvent += this.ButtonPressed;
                Gdk.Color? background = null;

                contentWidget.FocusInEvent += (s, e) => {
                    var w = s as Gtk.Widget;
                };
                contentWidget.EnterNotifyEvent += (s, e) => {
                    var w = s as Gtk.Widget;
                    if (background == null)
                        background = w.Style.Background (Gtk.StateType.Normal);
#if XWT_GTKSHARP3
                   
#else
                     w.ModifyBg (Gtk.StateType.Normal, Notifycolor);
                     w.QueueDraw ();
#endif
                   
                };
                contentWidget.LeaveNotifyEvent += (s, e) => {
                    var w = s as Gtk.Widget;
#if XWT_GTKSHARP3
                   
#else                    
                    w.ModifyBg (Gtk.StateType.Normal, background.Value);
                    w.QueueDraw ();
#endif
                };
                
               

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

            if (ButtonType == Xwt.ButtonType.DropDown) {
                Gtk.Widget dropDownArrow = new Gtk.Arrow (Gtk.ArrowType.Down, Gtk.ShadowType.Out);
                dropDownArrow = AllocEventBox (dropDownArrow);
                dropDownArrow.AddEvents ((int) Gdk.EventMask.ButtonPressMask);
                dropDownArrow.ButtonPressEvent += this.DropDownPressed;
#if XWT_GTKSHARP3
                   
#else                   
                dropDownArrow.ModifyBg(Gtk.StateType.Normal, Widget.Style.Background (Gtk.StateType.Normal));
#endif
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
            button.AddEvents ((int) Gdk.EventMask.ButtonPressMask);
            button.ButtonPressEvent += this.ButtonPressed;
            button.Clicked += this.OnClick;
            GtkEngine.ReplaceChild (widget, button);

            return button;
        }


    }
}