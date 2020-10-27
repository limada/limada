using System;
using Gtk;
using Xwt.Backends;

namespace Xwt.GtkBackend {
    public partial class ListStoreBackend : TableStoreBackend, IListStoreBackend {
        public virtual void InitializeModelFix(Type[] columnTypes, Gtk.ListStore store) {
            store.RowsReordered += (o, args) => {
                if (RowsReordered != null)
                {
                    var indices = args.NewChildOrder;
                    RowsReordered(this, new ListRowOrderEventArgs(-1, indices));
                }
            };
        }
    }
}