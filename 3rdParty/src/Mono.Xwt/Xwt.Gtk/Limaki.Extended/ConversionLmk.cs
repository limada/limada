namespace Xwt.GtkBackend {
    public static class ConversionLmk {
        
        
        public static Gtk.Justification ToGtkValue (this Alignment value) {
            if (value == Alignment.Center)
                return Gtk.Justification.Center;
            if (value == Alignment.Start)
                return Gtk.Justification.Left;
            // if (value == Alignment.End)
            return Gtk.Justification.Right;
        }

        public static Alignment ToXwtValue (this Gtk.Justification value) {
            if (value == Gtk.Justification.Center)
                return Alignment.Center;
            if (value == Gtk.Justification.Left)
                return Alignment.Start;
            //if (value == Gtk.Justification.Right)
            return Alignment.End;
        }
    }
}