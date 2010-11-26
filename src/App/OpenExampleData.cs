using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Limaki.Tests.Widget;

namespace Limaki.App {
    public partial class OpenExampleData : Form {
        public OpenExampleData() {
            
            InitializeComponent();

            algos.Add(new TypeChoose<ProgammingLanguageTestData>());
            algos.Add(new TypeChoose<SceneTestBinaryTreeData>());
            algos.Add(new TypeChoose<SceneTestWithLatticeData>());
            algos.Add(new TypeChoose<WordGameTestData>());
            algos.Add(new TypeChoose<BenchmarkOneData>());
            this.comboBox1.DataSource = algos;

        }

        public List<ITypeChoose> algos = new List<ITypeChoose>();


        public interface ITypeChoose {
            ISceneTestData Data { get;}
        }
        public class TypeChoose<T>:ITypeChoose where T:class,ISceneTestData,new()  {
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

            ISceneTestData ITypeChoose.Data {
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