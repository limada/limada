using System;
using Xwt.Backends;

namespace Xwt.GtkBackend {
    public static partial class CellUtil {
        public static Gtk.Widget CreateCellRenderer (ApplicationContext actx, CellView view)
        {
            if (view is TextCellView textCellView) {
                Gtk.Label lab = new Gtk.Label();
                lab.Xalign = 0;
                if (textCellView.Markup != null) {
                    FormattedText tx = FormattedText.FromMarkup(textCellView.Markup);
                    lab.Text = tx.Text;
                    var atts = new FastPangoAttrList();
                    atts.AddAttributes(new TextIndexer(tx.Text), tx.Attributes);
                    lab.Attributes = new Pango.AttrList(atts.Handle);
                    atts.Dispose();
                } else {
                    lab.Text = textCellView.Text;
                    if (false) // todo
                        lab.Attributes = new Pango.AttrList();
                }
                return lab;
            }
          
            if (view is CheckBoxCellView) {
                var chkBox = new Gtk.CheckButton();
                chkBox.Clicked += (s, e) => ((CheckBoxCellView)view).RaiseToggled ();
                return chkBox;
            }
            throw new NotImplementedException ();
        }

    }
}