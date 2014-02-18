using System.Windows.Forms;
using Limaki.Tests.View.Display;
using Limaki.View;
using Limaki.View.Visualizers;

namespace Limaki.Tests.View.Winform {
    public class WinformTestDevice<T> : ITestDevice
        where T : class {

        public object CreateForm(IDisplay display) {
            var disp = display as Display<T>;
            var backend = disp.Backend as Control;

            var form = new Form();
            form.Controls.Add(backend);
            backend.Dock = DockStyle.Fill;
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            form.BackColor = System.Drawing.Color.WhiteSmoke;
            DoEvents();
            form.Show();
            DoEvents();
            return form;
        }

        public object FindForm(IDisplay display) {
            var disp = display as Display<T>;
            var backend = disp.Backend as Control;
            return backend.FindForm();
        }

        public void DoEvents() {
            Application.DoEvents();
        }


    }
}