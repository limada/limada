using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Limaki.App {
    using Limaki.Widgets;
    using Limaki.Widgets.Layout;
    using Converter = Limaki.Drawing.GDI.GDIExtensions;

    public partial class LayoutEditor : UserControl {
        public LayoutEditor() {
            InitializeComponent();
        }

        public event EventHandler PropertyValueChanged;
        bool disableChanges = false;
        void DoPropertyValueChanged() {
            if (PropertyValueChanged != null && !disableChanges) {
                PropertyValueChanged(this, new EventArgs());
            }
        }

        WidgetLayout<Scene,IWidget> _selectedObject = null;
        public WidgetLayout<Scene, IWidget> SelectedObject {
            get {
                if (_selectedObject == null) {
                    
                }
                return _selectedObject;
            }
            set {
                _selectedObject = value;
                SetEditorFromLayout(_selectedObject);
            }
        }

        void SetEditorFromLayout(WidgetLayout<Scene,IWidget>  layout) {
            disableChanges = true;
            this.Distance = Converter.Native(layout.Distance);
            this.Orientation = layout.Orientation;
            this.Centered = layout.Centered;
            disableChanges = false;
        }

        private Size _distance = new Size(30, 15);
        public Size Distance {
            get { return _distance; }
            set {
                _distance = value;
                heightDistanceDown.Value = value.Height;
                widthDistanceUpDown.Value = value.Width;
                this.SelectedObject.Distance = Converter.Toolkit (value);
                DoPropertyValueChanged ();
            }
        }

        protected Orientation _orientation = Orientation.LeftRight;
        public Orientation Orientation {
            get { return _orientation; }
            set {
                _orientation = value;
                if (value==Orientation.LeftRight) {
                    leftRightButton.Checked = true;
                } else {
                    topBottomButton.Checked = true;
                }
                this.SelectedObject.Orientation = value;
                DoPropertyValueChanged();
            }
        }

        protected bool _centered = false;
        public bool Centered {
            get { return _centered; }
            set {
                _centered = value;
                centeredCheckBox.Checked = value;
                this.SelectedObject.Centered = value;
                DoPropertyValueChanged();
            }
        }

        private void distanceUpDown_Click(object sender, EventArgs e) {
            var upDown = sender as NumericUpDown;
            if (upDown != null) {
                var value = (int)upDown.Value;
                if (value <= 0)
                    value = 10;
                if (upDown.Name.ToLower().StartsWith("height")) {
                    this.Distance = new Size(this.Distance.Width, value);
                } else if (upDown.Name.ToLower().StartsWith("width")) {
                    this.Distance = new Size(value,this.Distance.Height);
                }
            }
        }

        private void orientationButton_Click(object sender, EventArgs e) {
            var button = sender as RadioButton;
            if (button != null) {
                if (button.Name.ToLower().StartsWith("left")) {
                    this.Orientation = Orientation.LeftRight;
                } else if (button.Name.ToLower().StartsWith("top")) {
                    this.Orientation = Orientation.TopBottom;
                }

            }
        }

        private void centeredCheckBox_CheckedChanged(object sender, EventArgs e) {
            var checkBox = sender as CheckBox;
            if (checkBox != null) {
                this.Centered = checkBox.Checked;
            }
        }


    }
}
