using System;
using System.Windows.Forms;
using Limaki.View.Swf;
using Limaki.Tests.View;

namespace Limaki.Tests.View.Winform {

    public partial class OpenExampleData : Form {
        public SceneExamples SceneExamples = new SceneExamples ();
        public OpenExampleData() {
            
            InitializeComponent();

            this.comboBox1.DataSource = SceneExamples.Examples;

            this.comboBox1.SelectedIndexChanged += (s, o) =>
                SceneExamples.Selected = SceneExamples.Examples[comboBox1.SelectedIndex];

            this.openButton.Click += (s, o) => {
                this.SceneExamples.DialogResult = Limaki.View.Vidgets.DialogResult.Ok;
                this.DialogResult = DialogResult.OK;
            };

            this.cancelButton.Click += (s, o) => {
                this.SceneExamples.DialogResult = Limaki.View.Vidgets.DialogResult.Cancel;
                this.DialogResult = DialogResult.Cancel;
            };
        }

    }
}