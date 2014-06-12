using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using SWF = System.Windows.Forms;
using LVV = Limaki.View.Vidgets;
using SD = System.Drawing;
using System.ComponentModel;
using Xwt.Backends;
using System;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ToolStripButtonBackend : SWF.ToolStripButton, IToolStripButtonBackend {

        public ToolStripButtonBackend () {
            ImageScaling = ToolStripItemImageScaling.None;
            Click += ClickAction;
        }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt (); }
            set { base.Size = value.ToGdi (); }
        }

        #region IVidgetBackend Member

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public LVV.ToolStripButton Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStripButton)frontend;
        }

        void IVidgetBackend.Update () { }

        void IVidgetBackend.Invalidate() {
            this.Invalidate();
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
        }

        void IVidgetBackend.SetFocus () {  }

        #endregion

        public void SetImage (Xwt.Drawing.Image image) {
            if (this.Parent != null)
                this.Parent.SuspendLayout ();
            base.Image = image.ToGdi ();
            if (this.Parent != null)
                this.Parent.ResumeLayout ();
        }

        public void SetLabel (string value) {
            base.Text = value;
        }

        public void SetToolTip (string value) {
            base.ToolTipText = value;
        }


        protected System.Action<object> _action = null;
        public virtual void SetAction (System.Action<object> value) {
            _action = value;
        }

        protected virtual void ClickAction (object sender, System.EventArgs e) {
            if (_action != null) {
                _action (this);
            }
        }
    }

    public class ToolStripDropDownButtonBackend : SWF.ToolStripDropDownButton, IToolStripDropDownButtonBackend {

        public ToolStripDropDownButtonBackend () {
            ImageScaling = ToolStripItemImageScaling.None;
            DisplayStyle = ToolStripItemDisplayStyle.Image;
            Click += ClickAction;
            ((ToolStripDropDownMenu)this.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)this.DropDown).ShowCheckMargin = false;
        }

        protected bool DropDownClicked = false;
        protected override void OnClick (EventArgs e) {

            if (!DropDownClicked)
                base.OnClick (e);
        }

        protected override void OnMouseDown (MouseEventArgs e) {

            var w = ToolStripUtils.DropdownWidth;
            var area = new SD.Rectangle (this.Width - w, 0, w, this.Height);
            DropDownClicked = area.Contains (e.Location);
            this.DropDown.PerformLayout ();
            var size = DropDown.Size;
            this.DropDown.Visible = !DropDownClicked;
            if (Image != null && string.IsNullOrEmpty (Text) && DisplayStyle == ToolStripItemDisplayStyle.Image) {
                DropDown.AutoSize = false;
                DropDown.Width = (int)Size.Width;
                DropDown.Height = size.Height;
            }
            base.OnMouseDown (e);
            //this.DropDown.Visible = this.Pressed;

        }
        public new Xwt.Size Size {
            get { return base.Size.ToXwt (); }
            set { base.Size = value.ToGdi (); }
        }

        #region IVidgetBackend Member

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public LVV.ToolStripDropDownButton Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStripDropDownButton)frontend;
        }

        void IVidgetBackend.Update () { }

        void IVidgetBackend.Invalidate () {
            this.Invalidate ();
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
        }

        void IVidgetBackend.SetFocus () { }

        #endregion

        public void SetImage (Xwt.Drawing.Image image) {
            if (this.Parent != null)
                this.Parent.SuspendLayout ();
            base.Image = image.ToGdi ();
            if (this.Parent != null)
                this.Parent.ResumeLayout ();
        }

        public void SetLabel (string value) {
            base.Text = value;
        }

        public void SetToolTip (string value) {
            base.ToolTipText = value;
        }


        protected System.Action<object> _action = null;
        public virtual void SetAction (System.Action<object> value) {
            _action = value;
        }

        protected virtual void ClickAction (object sender, System.EventArgs e) {
            if (_action != null) {
                _action (this);
            }
        }

        public void InsertItem (int index, IToolStripItemBackend backend) {
            this.DropDownItems.Insert (index, (SWF.ToolStripItem) backend);
        }

        public void RemoveItem (IToolStripItemBackend backend) {
            this.DropDownItems.Remove ((SWF.ToolStripItem) backend);
        }
    }
}