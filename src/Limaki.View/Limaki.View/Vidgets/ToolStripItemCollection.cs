using System.Collections.ObjectModel;

namespace Limaki.View.Vidgets {

    public class ToolStripItemCollection : Collection<ToolStripItem> {
        ToolStrip parent;

        internal ToolStripItemCollection (ToolStrip parent) {
            this.parent = parent;
        }

        protected override void InsertItem (int index, ToolStripItem item) {
            base.InsertItem (index, item);
            parent.InsertItem (index, item);
        }

        protected override void RemoveItem (int index) {
            var item = this[index];
            base.RemoveItem (index);
            parent.RemoveItem (item);
        }

        protected override void SetItem (int index, ToolStripItem item) {
            var oldItem = this[index];
            base.SetItem (index, item);
            parent.RemoveItem (oldItem);
            parent.InsertItem (index, item);
        }

        protected override void ClearItems () {
            foreach (var item in this)
                parent.RemoveItem (item);
            base.ClearItems ();
        }
    }
}