using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IComboBoxBackend))]
    public class ComboBox : Vidget {

        private IComboBoxBackend _backend = null;
        public new virtual IComboBoxBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IComboBoxBackend); }
            set { _backend = value; }
        }

        ItemCollection _items = null;

        public ItemCollection Items {
            get {
                if (_items == null) {
                    _items = new ItemCollection ();
                    _items.CollectionChanged += ItemCollectionChanged;
                }
                return _items ;
            }
        }

        protected void ItemCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            Backend.ItemCollectionChanged (sender, e);
        }

        [DefaultValue (-1)]
        public int SelectedIndex {
            get { return Backend.SelectedIndex; }
            set { Backend.SelectedIndex = value; }
        }

        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public object SelectedItem {
            get {
                if (Backend.SelectedIndex == -1)
                    return null;
                return Items[Backend.SelectedIndex];
            }
            set {
                SelectedIndex = Items.IndexOf (value);
            }
        }

        EventHandler selectionChanged;

        public event EventHandler SelectionChanged {
            add {
                Backend.AddSelectionChanged (value);
                selectionChanged += value;
            }
            remove {
                selectionChanged -= value;
                Backend.RemoveSelectionChanged (value);
            }
        }

        public double Width {
            get { return Size.Width; }
            set { Backend.SetWidth (value); }
        }

        public override void Dispose () {
            
        }
    }

    public class ItemCollection : ObservableCollection<object> {
        
    }

    public interface IComboBoxBackend : IVidgetBackend {

        void ItemCollectionChanged (object sender, NotifyCollectionChangedEventArgs e);

        int SelectedIndex { get; set; }

        void AddSelectionChanged (EventHandler value);

        void RemoveSelectionChanged (EventHandler value);

        void SetWidth (double value);
    }
}