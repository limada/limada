using System;
namespace Limaki.View.GtkBackend {

    public class ImageComboBoxBackend : VidgetBackend<Gtk.ComboBox> {

        public override void InitializeBackend (IVidget frontend, Vidgets.VidgetApplicationContext context) {
            throw new System.NotImplementedException ();
        }

        /// <summary>
        /// 
        /// </summary>
        void prototype () {
            var pixbuf = new Gtk.CellRendererPixbuf ();

            // no need for text!:
            //var text = new Gtk.CellRendererText ();

            var store = new Gtk.ListStore (
                typeof (Gdk.Pixbuf), 
                //typeof (string),
                typeof (int));

            var combo = new Gtk.ComboBox (store);

            combo.PackStart (pixbuf, false);
            //combo.PackStart (text, false);

            combo.AddAttribute (pixbuf, "pixbuf", 0);
            //combo.AddAttribute (text, "text", 1);

            int id = 0;
            var stockIds = Gtk.Stock.ListIds ();

            foreach (var stockItemName in stockIds) {

                Gdk.Pixbuf image = combo.RenderIcon (stockItemName, Gtk.IconSize.SmallToolbar, "");

                store.AppendValues (image, /* stockItemName,*/ id);
                id++;

            }
            Gtk.TreeIter iter;
            if (store.GetIterFirst (out iter))
                combo.SetActiveIter (iter);
        }


    }
}