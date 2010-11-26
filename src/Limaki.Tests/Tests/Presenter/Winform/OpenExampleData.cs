using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Limada.Schemata;
using Limada.Tests.View;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Widget;
using Limaki.Presenter.Winform;

namespace Limaki.Tests.Presenter.Winform {
    public class ExampleData {
        public interface ITypeChoose {
            ISceneFactory Data { get; }
        }
        public class TypeChoose<T> : ITypeChoose where T : class, ISceneFactory, new() {
            public TypeChoose() { }
            private T _data = default(T);

            public T Data {
                get {
                    if (_data == default(T)) {
                        _data = new T();
                    }
                    return _data;
                }
            }

            public override string ToString() {
                return this.Data.Name;
            }


            ISceneFactory ITypeChoose.Data {
                get { return this.Data; }
            }

        }

        public IList<ITypeChoose> _examples = null;
        public IList<ITypeChoose> examples {
            get {
                if(_examples ==null) {
                    _examples = new List<ITypeChoose> ();
                    _examples.Add(new TypeChoose<SceneFactory<ProgrammingLanguageFactory>>());
                    _examples.Add(new TypeChoose<SceneFactory<BinaryTreeFactory>>());
                    _examples.Add(new TypeChoose<SceneFactory<BinaryGraphFactory>>());
                    _examples.Add(new TypeChoose<SceneFactory<GCJohnBostonGraphFactory>>());
                    _examples.Add(new TypeChoose<SceneFactory<WordGameGraphFactory>>());
                    _examples.Add(new TypeChoose<SceneFactory<LimakiShortHelpFactory>>());
                    _examples.Add(new TypeChoose<BenchmarkOneSceneFactory>());
                    _examples.Add(new TypeChoose<SchemaViewTestData<DocumentSchema>>());
                    Selected = _examples[0];
                }
                return _examples;
            }
        }

        public Limaki.UseCases.Viewers.DialogResult DialogResult { get; set; }
        public ITypeChoose Selected { get; set; }
    }

    public partial class OpenExampleData : Form {
        public ExampleData ExampleData = new ExampleData ();
        public OpenExampleData() {
            
            InitializeComponent();


            this.comboBox1.DataSource = ExampleData.examples;
            this.comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);

        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            ExampleData.Selected = ExampleData.examples[comboBox1.SelectedIndex];
        }

        private void openButton_Click(object sender, EventArgs e) {
            this.ExampleData.DialogResult = Limaki.UseCases.Viewers.DialogResult.OK;
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.ExampleData.DialogResult = Limaki.UseCases.Viewers.DialogResult.Cancel;
            this.DialogResult = DialogResult.Cancel;
        }
    }
}