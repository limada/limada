using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Limaki.App {
    public partial class TextDialog : Form {
        public TextDialog() {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close ();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}