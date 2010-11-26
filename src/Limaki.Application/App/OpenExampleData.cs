using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Limada.Schemata;
using Limada.Tests.View;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Widget;

namespace Limaki.App {
    public partial class OpenExampleData : Form {
        public OpenExampleData() {
            
            InitializeComponent();

            examples.Add(new TypeChoose<SceneFactory<ProgrammingLanguageFactory>>());
            examples.Add(new TypeChoose<SceneFactory<BinaryTreeFactory>>());
            examples.Add(new TypeChoose<SceneFactory<BinaryGraphFactory>>());
            examples.Add(new TypeChoose<SceneFactory<GCJohnBostonGraphFactory>>());
            examples.Add(new TypeChoose<SceneFactory<WordGameGraphFactory>>());
            examples.Add(new TypeChoose<SceneFactory<LimakiShortHelpFactory>>());
            examples.Add(new TypeChoose<BenchmarkOneSceneFactory>());
            examples.Add(new TypeChoose<SchemaViewTestData<DocumentSchema>>());
            this.comboBox1.DataSource = examples;

        }

        public List<ITypeChoose> examples = new List<ITypeChoose>();


        public interface ITypeChoose {
            ISceneFactory Data { get;}
        }
        public class TypeChoose<T>:ITypeChoose where T:class,ISceneFactory,new()  {
            public TypeChoose() {}
            private T _data = default( T );

            public T Data {
                get {
                    if (_data == default(T)) {
                        _data = new T ();
                    }
                    return _data;
                }
            }

            public override string ToString() {
                return this.Data.Name;
            }

            #region ITypeChoose Member

            ISceneFactory ITypeChoose.Data {
                get { return this.Data; }
            }

            #endregion
        }

        private void openButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}