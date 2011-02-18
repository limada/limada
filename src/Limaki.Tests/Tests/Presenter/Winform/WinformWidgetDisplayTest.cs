using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.Presenter.Winform.Display;
using Limaki.Tests.Presenter.Display;
using Limaki.Widgets;
using NUnit.Framework;
using System.Windows.Forms;

namespace Limaki.Tests.Presenter.Winform {
    public class WinformWidgetDisplayTest<T>:DomainTest
    where T : DisplayTest<IGraphScene<IWidget, IEdgeWidget>>, new() {
        public T Test { get; set; }
        public override void Setup() {
            base.Setup();
            var test = new T();
            var testinst = new WinformDisplayTestComposer<IGraphScene<IWidget, IEdgeWidget>>();

            //testinst.Factory = () => new WinformWidgetDisplay().Display;
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