using System;
using System.Linq;
using System.Collections.Specialized;
using System.ComponentModel;
using Limaki.View.Vidgets;
using Limaki.Common.Linqish;
using Xwt.GdiBackend;
using SWF = System.Windows.Forms;
using LVV = Limaki.View.Vidgets;
using SD = System.Drawing;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ComboBoxBackend : SWF.ComboBox, IComboBoxBackend {

        public ComboBoxBackend () : base () {
            this.Margin = new SWF.Padding ();
            this.FlatStyle = SWF.FlatStyle.Popup;
        }

        #region IVidgetBackend Member

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public LVV.ComboBox Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ComboBox)frontend;
        }

        void IVidgetBackend.Update () {
            this.Update ();
        }

        void IVidgetBackend.Invalidate () {
            this.Invalidate ();
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
        }

        void IVidgetBackend.SetFocus () {
            this.Focus ();
        }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt (); }
            set { base.Size = value.ToGdi (); }
        }

        #endregion

        public void ItemCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                this.Items.AddRange (e.NewItems.Cast<object>().ToArray());
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                e.OldItems.Cast<object> ().ForEach (i => this.Items.Remove (i));
            } else if (e.Action == NotifyCollectionChangedAction.Reset) {
                this.Items.Clear ();
            }
        }

        public void AddSelectionChanged (EventHandler value) {
            this.SelectedValueChanged += value;
        }

        public void RemoveSelectionChanged (EventHandler value) {
            this.SelectedValueChanged -= value;
        }

        public void SetWidth (double value) {
            this.Width = (int) value;
        }
    }
}