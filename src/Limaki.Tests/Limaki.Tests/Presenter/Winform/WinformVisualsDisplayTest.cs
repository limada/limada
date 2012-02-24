using Limaki.Drawing;
using Limaki.View.Display;
using Limaki.View.Winform.Display;
using Limaki.Tests.Presenter.Display;
using Limaki.Visuals;
using NUnit.Framework;
using System.Windows.Forms;

namespace Limaki.Tests.Presenter.Winform {
    public class WinformVisualsDisplayTest<T>:DomainTest
    where T : DisplayTest<IGraphScene<IVisual, IVisualEdge>>, new() {
        public T Test { get; set; }
        public override void Setup() {
            base.Setup();
            var test = new T();
            var testinst = new WinformDisplayTestComposer<IGraphScene<IVisual, IVisualEdge>>();

            //testinst.Factory = () => new WinformVisualsDisplay().Display;
            testinst.Factor(test);
            testinst.Compose(test);


            test.Setup();
            (test.TestForm as Form).Show();
            this.Test = test;
        }
        public override void TearDown() {
            Test.TearDown ();
            base.TearDown();
        }
    }
}