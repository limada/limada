using System.Windows.Forms;
using Limaki.Tests.View.Display;
using Limaki.View;
using Limaki.View.Visualizers;

namespace Limaki.Tests.View.Winform {
    public class WinformTestDevice<T> : ITestDevice
        where T : class {

        public object CreateForm(IDisplay display) {
            var disp = display as Display<T>;
            var device = disp.Backend as Control;

            var form = new Form();
            form.Controls.Add(device);
            device.Dock = DockStyle.Fill;
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            form.BackColor = System.Drawing.Color.WhiteSmoke;
            DoEvents();
            form.Show();
            DoEvents();
            return form;
        }

        public object FindForm(IDisplay display) {
            var disp = display as Display<T>;
            var device = disp.Backend as Control;
            return device.FindForm();
        }

        public void DoEvents() {
            Application.DoEvents();
        }


    }
}