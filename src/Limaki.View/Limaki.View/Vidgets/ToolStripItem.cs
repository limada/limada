using System;
using Xwt;
using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolStripItemBackend))]
    public class ToolStripItem : Vidget, IToolStripCommand {

        private IToolStripItemBackend _backend = null;
        public new virtual IToolStripItemBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripItemBackend); }
            set { _backend = value; }
        }

        public override void Dispose () { }

        public Action<object> Action { get; set; }

        protected Image _image = null;
        public virtual Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                    Backend.SetImage (value);
                }
            }
        }

        protected string _label = null;
        public virtual string Label {
            get { return _label; }
            set {
                if (_label != value) {
                    _label = value;
                    Backend.SetLabel (value);
                }
            }
        }

        protected string _toolTipText = null;
        public virtual string ToolTipText {
            get { return _toolTipText; }
            set {
                if (_toolTipText != value) {
                    _toolTipText = value;
                    Backend.SetToolTip (value);
                }
            }
        }


        public Size Size { get; set; }

        public void SetCommand (IToolStripCommand command) {
            VidgetUtils.SetCommand (this, command); 
        }
    }

    public interface IToolStripItemContainer {
        void InsertItem (int index, ToolStripItem item);
        void RemoveItem (ToolStripItem item);
    }

    [BackendType (typeof (IToolStripButtonBackend))]
    public class ToolStripButton : ToolStripItem {

        private IToolStripButtonBackend _backend = null;
        public new virtual IToolStripButtonBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripButtonBackend); }
            set { _backend = value; }
        }


        public override void Dispose () { }
    }

    [BackendType (typeof (IToolStripDropDownButtonBackend))]
    public class ToolStripDropDownButton : ToolStripButton, IToolStripItemContainer {

        private IToolStripDropDownButtonBackend _backend = null;
        public new virtual IToolStripDropDownButtonBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripDropDownButtonBackend); }
            set { _backend = value; }
        }

        private ToolStripItemCollection _items;
        public ToolStripItemCollection Items {
            get { return _items ?? (_items = new ToolStripItemCollection (this)); }
        }

        public void AddItems (params ToolStripItem[] items) {
            foreach (var item in items)
                Items.Add (item);

        }

        void IToolStripItemContainer.InsertItem (int index, ToolStripItem item) {
            Backend.InsertItem (index, (IToolStripItemBackend)item.Backend);
        }

        void IToolStripItemContainer.RemoveItem (ToolStripItem item) {
            Backend.RemoveItem ((IToolStripItemBackend)item.Backend);
        }
        public override void Dispose () { }
    }

    public interface IToolStripItemBackend : IVidgetBackend {
        void SetImage (Image image);

        void SetLabel (string value);

        void SetToolTip (string value);
    }

    public interface IToolStripButtonBackend : IToolStripItemBackend {
        
    }

    public interface IToolStripDropDownButtonBackend : IToolStripButtonBackend { 
        void InsertItem (int index, IToolStripItemBackend backend);
        void RemoveItem (IToolStripItemBackend backend);
    }
}