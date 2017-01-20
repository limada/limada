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
    
    public class ComboBoxBackend : VidgetBackend<ComboBoxEx>, IComboBoxBackend {

        public ComboBoxBackend () : base () {
            Control.Margin = new SWF.Padding ();
            Control.FlatStyle = SWF.FlatStyle.Popup;
        }

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public new LVV.ComboBox Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (LVV.ComboBox)frontend;
        }


        public void ItemCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
           
            if (e.Action == NotifyCollectionChangedAction.Add) {
                var setGrahics = e.NewItems.Cast<object> ().Any (o => typeof (Xwt.Drawing.Image).IsAssignableFrom (o.GetType ()));
                Control.HasGraphics = setGrahics;
                Control.Items.AddRange (e.NewItems.Cast<object> ().ToArray ());
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                e.OldItems.Cast<object> ().ForEach (i => Control.Items.Remove (i));
            } else if (e.Action == NotifyCollectionChangedAction.Reset) {
                Control.Items.Clear ();
            }
        }

        public void AddSelectionChanged (EventHandler value) {
            Control.SelectedValueChanged += value;
        }

        public void RemoveSelectionChanged (EventHandler value) {
            Control.SelectedValueChanged -= value;
        }

        public void SetWidth (double value) {
            Control.Width = (int)value;
        }

        public int SelectedIndex {
            get { return Control.SelectedIndex; }
            set { Control.SelectedIndex = value; }
        }
    }
}